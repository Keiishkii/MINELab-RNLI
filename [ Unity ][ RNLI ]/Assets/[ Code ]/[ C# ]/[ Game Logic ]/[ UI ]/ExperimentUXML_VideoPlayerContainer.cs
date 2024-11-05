using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.Video;

public partial class ExperimentUXML
{
    public VideoPlayerContainer videoPlayerContainer;
    [Serializable] public class VideoPlayerContainer : IContainer
    {
        #region [ Unserialised Fields ]
        private const string _videoPlayerContainerID = "VideoPlayerContainer";
        private const string _videoPlayerDisplayID = "VideoPlayer";
        private const string _avScalePointerID = "AVScalePointer";
        private const string _avScaleReplayPointerID = "AVScaleReplayPointer";
        private const string _avScaleTargetID = "AVScaleTarget";
        private const string _avScaleRingID = "AVScaleRing";
        
        private readonly VisualElement _videoPlayerVisualElement;
        private readonly VisualElement _avScalePointerVisualElement;
        private readonly VisualElement _avScaleReplayPointerVisualElement;
        private readonly VisualElement _avScaleTargetVisualElement;
        private readonly VisualElement _avScaleRingVisualElement;
        
        private readonly VideoPlayer _videoPlaybackController;
        private readonly RenderTexture _videoOutputRenderTexture;
        private readonly InputManager _inputManager;
        
        public bool PointerDisplayed = true;
        private bool _completed = true;
        private bool _playing;
        #endregion
        
        
        
        public VideoPlayerContainer(ExperimentUXML experimentUxml, VisualElement root) : base(experimentUxml, root, _videoPlayerContainerID)
        {
            _videoPlayerVisualElement = _containerVisualElement.Q<VisualElement>(_videoPlayerDisplayID);
            _avScalePointerVisualElement = _containerVisualElement.Q<VisualElement>(_avScalePointerID);
            _avScaleReplayPointerVisualElement = _containerVisualElement.Q<VisualElement>(_avScaleReplayPointerID);
            _avScaleTargetVisualElement = _containerVisualElement.Q<VisualElement>(_avScaleTargetID);
            _avScaleRingVisualElement = _containerVisualElement.Q<VisualElement>(_avScaleRingID);
            
            _videoPlaybackController = FindObjectOfType<VideoPlayer>();
            _inputManager = FindObjectOfType<InputManager>();
            
            _videoPlayerVisualElement.style.visibility = Visibility.Hidden;
            _videoOutputRenderTexture = _videoPlaybackController.targetTexture;
            
            HidePointer();
            HideTarget();
        }
        
        #region [ Container Controls ]
        public override IEnumerator DisplayCoroutine()
        {
            _videoOutputRenderTexture.Release();
            _inputManager.participantConformationActionReference.action.performed += OnParticipantConformationPressed;
            _inputManager.participantConformationActionReference.action.canceled += OnParticipantConformationReleased;
            
            _inputManager.avScaleInputActionReference.action.performed += DisplayPointer;
            _inputManager.avScaleInputActionReference.action.canceled += HidePointer;

            _avScaleRingVisualElement.style.display = (_inputManager.participantConformationActionReference.action.ReadValue<float>() > 0.5f) ? DisplayStyle.Flex : DisplayStyle.None;
            
            yield return base.DisplayCoroutine();
        }

        public override IEnumerator HideCoroutine()
        {
            yield return base.HideCoroutine();
            
            _videoOutputRenderTexture.Release();
            _inputManager.participantConformationActionReference.action.performed -= OnParticipantConformationPressed;
            _inputManager.participantConformationActionReference.action.canceled -= OnParticipantConformationReleased;
            
            _inputManager.avScaleInputActionReference.action.performed -= DisplayPointer;
            _inputManager.avScaleInputActionReference.action.canceled -= HidePointer;
        }
        #endregion

        #region [ Target Controls ]
        public void HideTarget() => _avScaleTargetVisualElement.style.display = DisplayStyle.None;
        public void DisplayTarget(Vector2 targetPosition)
        {
            _avScaleTargetVisualElement.style.translate = new Translate(targetPosition.x * 200, targetPosition.y * -200);
            _avScaleTargetVisualElement.style.display = DisplayStyle.Flex;
        }
        #endregion

        #region [ Input Logic - Conformation ]
        private void OnParticipantConformationPressed(InputAction.CallbackContext callbackContext) => _avScaleRingVisualElement.style.display = DisplayStyle.Flex;
        private void OnParticipantConformationReleased(InputAction.CallbackContext callbackContext) => _avScaleRingVisualElement.style.display = DisplayStyle.None;
        #endregion

        #region [ Input Logid - AV Inputs ]
        private void HidePointer(InputAction.CallbackContext callbackContext) => HidePointer();
        private void HidePointer() => _avScalePointerVisualElement.style.display = DisplayStyle.None;
        private void DisplayPointer(InputAction.CallbackContext callbackContext)
        {
            if (!PointerDisplayed)
            {
                _avScalePointerVisualElement.style.display = DisplayStyle.None;
                return;
            }
            _avScalePointerVisualElement.style.display = DisplayStyle.Flex;
            Vector2 position = callbackContext.ReadValue<Vector2>();

            _avScalePointerVisualElement.style.translate = new Translate(position.x * 72.5f, position.y * -72.5f);
            Debug.Log($"Position: {position.ToString()}");
        }
        #endregion

        #region [ Image Display Controls ]
        public void DisplayImage(PhotoDataScriptableObject photoData)
        {
            _videoPlayerVisualElement.style.visibility = Visibility.Visible;
            RenderTexture.active = _videoOutputRenderTexture;
            Graphics.Blit(photoData.photo, _videoOutputRenderTexture);
        }

        public void HideImage()
        {
            _videoOutputRenderTexture.Release();
            _videoPlayerVisualElement.style.visibility = Visibility.Hidden;
        }

        public void ReplayPointerVisibility(bool visibility) => _avScaleReplayPointerVisualElement.style.display = visibility ? DisplayStyle.Flex : DisplayStyle.None;
        public IEnumerator ReplaySampleData(List<Vector2> samples)
        {
            if (samples.Count <= 0) yield break;
            
            float duration = samples.Count * Time.fixedDeltaTime;
            for (float timeElapsed = 0; timeElapsed < duration; timeElapsed += Time.deltaTime)
            {
                int index = Mathf.Min(samples.Count - 1, Mathf.RoundToInt(timeElapsed / Time.fixedDeltaTime));
                Vector2 position = samples[index];

                _avScaleReplayPointerVisualElement.style.translate = new Translate(position.x * 72.5f, position.y * -72.5f);
                yield return new WaitForFixedUpdate();
            }
        }

        #endregion
        
        #region [ Video Player Controls ]
        public void PlayVideo(SceneDataScriptableObject scene, DataManager.Activity activity) => _experimentUxml.StartCoroutine(PlayVideoCoroutine(scene, activity));
        private IEnumerator PlayVideoCoroutine(SceneDataScriptableObject scene, DataManager.Activity activity)
        {
            _videoPlaybackController.targetTexture.Release();
            _videoPlaybackController.clip = scene.clip;
            
            _videoPlaybackController.Play();
            _completed = false;
            _playing = true;
            
            _videoPlayerVisualElement.style.visibility = Visibility.Visible;
            _videoPlaybackController.loopPointReached += VideoCompleted;
            
            Debug.Log($"Playing {_videoPlaybackController.time}");
            
            DataManager.AddVideoEvent(scene, DataManager.VideoEvent.VideoStart, activity);
            yield return new WaitUntil(() => _completed);
            DataManager.AddVideoEvent(scene, DataManager.VideoEvent.VideoEnd, activity);
            
            Debug.Log($"VIDEO ENDED {_videoPlaybackController.time}");
            
            _videoPlaybackController.loopPointReached -= VideoCompleted;
            _videoPlayerVisualElement.style.visibility = Visibility.Hidden;
            
            yield break;

            void VideoCompleted(VideoPlayer source)
            {
                _completed = true;
                _playing = false;
            }
        }

        public IEnumerator Finished()
        {
            yield return new WaitUntil(() => _completed);
        }
        
        public IEnumerator AwaitPlayback(float time)
        {
            yield return new WaitUntil(() => _videoPlaybackController.time >= time);
        }
        
        public void Play()
        {
            if (_playing) return;
            _playing = true;
            
            NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{"Video Played"});
            _videoPlaybackController.Play();
        }

        public void Pause()
        {
            if (!_playing) return;
            _playing = false;
            
            NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{"Video Paused"});
            _videoPlaybackController.Pause();
        }
        #endregion
    }
}