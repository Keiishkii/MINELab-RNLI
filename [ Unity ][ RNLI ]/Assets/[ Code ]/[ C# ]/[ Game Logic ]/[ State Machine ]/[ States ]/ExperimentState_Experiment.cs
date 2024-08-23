using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        ExperimentUXML.VideoPlayerContainer videoPlayer = _experimentUxml.videoPlayerContainer;
        List<SceneDataScriptableObject> scenes = _sessionData.experimentVideos.ToList();
        while (scenes.Count > 0)
        {
            _experimentUxml.videoPlayerContainer.IsExperiment = true;
            yield return _experimentUxml.videoPlayerContainer.DisplayCoroutine();
            yield return new WaitForSeconds(1f);

            #region [ Video Playback ]
            int videoIndex = Random.Range(0, scenes.Count);

            SceneDataScriptableObject scene = scenes[videoIndex];
            scenes.RemoveAt(videoIndex);
            
            videoPlayer.PlayVideo(scene, DataManager.Activity.Experiment);
            yield return videoPlayer.Finished();
            #endregion
            
            yield return new WaitForSeconds(1f);
            yield return _experimentUxml.videoPlayerContainer.HideCoroutine();
            
            if (scenes.Count > 0) yield return _timer.StartTimer(_sessionData.transitionalWaitDuration);
        }

        _experimentStateMachine.State = _experimentStateMachine.End;
    }

}
