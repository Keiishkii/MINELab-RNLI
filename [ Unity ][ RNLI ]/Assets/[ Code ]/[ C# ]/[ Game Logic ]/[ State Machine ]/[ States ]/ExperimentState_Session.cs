using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class ExperimentState_Session : IExperimentState
{
    #region [ Serialised Fields ]
    [SerializeField] private SessionDataScriptableObject _sessionData;
    #endregion
    
    #region [ Unserialised Fields ]
    private readonly List<SceneDataScriptableObject> _scenes = new List<SceneDataScriptableObject>();
    private IEnumerator _experimentCoroutine;
    private VideoPlayer _videoPlayer;
    #endregion


    public override void Initialise(ExperimentStateMachine experimentStateMachine, ExperimentController experimentController, InputManager inputManager, ExperimentUXML experimentUxml)
    {
        base.Initialise(experimentStateMachine, experimentController, inputManager, experimentUxml);
        _videoPlayer = Object.FindObjectOfType<VideoPlayer>();
    }

    public override void OnStateEnter()
    {
        _experimentUxml.ShowVideoPlayerVisualElement();
        _experimentController.StartCoroutine(_experimentCoroutine = Experiment());
    }
    
    private IEnumerator Experiment()
    {
        _scenes.Clear();
        foreach (var trial in _sessionData.trials) _scenes.Add(trial);

        while (_scenes.Count > 0)
        {
            int videoIndex = Random.Range(0, _scenes.Count);

            SceneDataScriptableObject scene = _scenes[videoIndex];
            _scenes.RemoveAt(videoIndex);

            yield return PlayClip(scene);
        }

        _experimentStateMachine.State = _experimentStateMachine.End;
    }

    private IEnumerator PlayClip(SceneDataScriptableObject scene)
    {
        _videoPlayer.clip = scene.clip;
        _videoPlayer.Play();
        yield return new WaitForSeconds((float) scene.clip.length);
    }
}
