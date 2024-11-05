using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[Serializable]
public class ExperimentState_Experiment : IExperimentState
{
    #region [ Unserialised Fields ]
    private IEnumerator _sessionExperimentCoroutine;
    #endregion

    public ExperimentState_Experiment(string stateName) : base(stateName) { }

    public override void OnStateEnter()
    {
        _experimentController.StartCoroutine(_sessionExperimentCoroutine = SessionExperimentCoroutine());
    }

    public override void OnStateExit()
    {
        if (!ReferenceEquals(_sessionExperimentCoroutine, null)) _experimentController.StopCoroutine(_sessionExperimentCoroutine);
    }

    private IEnumerator SessionExperimentCoroutine()
    {
        if (!_experimentController.sessionProgressionFlags.HasFlag(SessionStateFlags.Experiment)) goto StateEnd;

        int videoFrameCount, nonZeroSamples;
        InputActionReference avInput = InputManager.Instance.avScaleInputActionReference;
        List<SceneDataScriptableObject> scenes = _sessionData.experimentVideos.ToList();
        while (scenes.Count > 0)
        {
            _experimentUxml.videoPlayerContainer.PointerDisplayed = false;
            yield return _experimentUxml.videoPlayerContainer.DisplayCoroutine();
            yield return new WaitForSeconds(1f);

            videoFrameCount = 0; nonZeroSamples = 0;
            RuntimeManager.Instance.fixedUpdate += ProcessAVSamples;
            
            #region [ Video Playback ]
            int videoIndex = Random.Range(0, scenes.Count);

            SceneDataScriptableObject scene = scenes[videoIndex];
            scenes.RemoveAt(videoIndex);
            
            _videoPlayer.PlayVideo(scene, DataManager.Activity.Experiment);
            yield return _videoPlayer.Finished();
            #endregion
            
            RuntimeManager.Instance.fixedUpdate -= ProcessAVSamples;
            Debug.Log($"Frames: {videoFrameCount}, Samples: {nonZeroSamples}");
            float inputProportions = (float)nonZeroSamples / videoFrameCount;
            
            yield return new WaitForSeconds(1f);
            yield return _experimentUxml.videoPlayerContainer.HideCoroutine();

            Debug.Log($"Transition Time: {_sessionData.transitionalWaitDuration}");
            
            if (scenes.Count > 0)
            {
                yield return _timer.StartTimer(_sessionData.transitionalWaitDuration, true);
                yield return DisplayEndOfVideoMessageCoroutine(inputProportions);
            }
        }

        StateEnd:
        _experimentStateMachine.State = _experimentStateMachine.End;

        yield break;

        void ProcessAVSamples()
        {
            Vector2 sample = avInput.action.ReadValue<Vector2>();
            if (!Mathf.Approximately(sample.x, 0) && !Mathf.Approximately(sample.y, 0)) nonZeroSamples++;
            videoFrameCount++;
        }
    }
    
    
    private IEnumerator DisplayEndOfVideoMessageCoroutine(float activeInputProportion)
    {
        yield return _instructionDisplay.DisplayTextFieldCoroutine(activeInputProportion switch
        {
            >= 0.2f => _sessionData.instructionUIContentDictionary["Post Video Notification [ 50% -> 75% ]"],
            _ => _sessionData.instructionUIContentDictionary["Post Video Notification [ 0% -> 10% ]"],
        }, Wait());
        
        yield break;

        IEnumerator Wait() { yield return new WaitForSeconds(5); }
    }
}
