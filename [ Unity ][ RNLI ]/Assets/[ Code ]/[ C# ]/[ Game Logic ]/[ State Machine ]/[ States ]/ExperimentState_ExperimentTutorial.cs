using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExperimentState_ExperimentTutorial : IExperimentState
{
    #region [ Unserialised Fields ]
    private IEnumerator _sessionTutorialCoroutine;
    private Vector2 _target;
    #endregion
    
    public ExperimentState_ExperimentTutorial(string stateName) : base(stateName) { }

    public override void OnStateEnter() => _experimentController.StartCoroutine(_sessionTutorialCoroutine = SessionTutorialCoroutine());

    public override void OnStateExit()
    {
        if (!ReferenceEquals(_sessionTutorialCoroutine, null)) _experimentController.StopCoroutine(_sessionTutorialCoroutine);
    }
    
    private IEnumerator SessionTutorialCoroutine()
    {
        #region [ Tutorial Start ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial Start"], 
            _inputManager.AwaitResearcherConformationCoroutine());
        #endregion
        
        
        
        #region [ Panda Video Introduction ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda Introduction"], 
            _inputManager.AwaitParticipantConformationCoroutine());
        #endregion

        #region [ Panda Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        _videoPlayer.PlayVideo(_sessionData.tutorialVideos[0], DataManager.Activity.Tutorial);
        {
            #region [ Match AV Rating Tutorial: 1 ]
            yield return _videoPlayer.AwaitPlayback(8f);
            _videoPlayer.Pause();

            _target = new Vector2(0.5f, -0.5f);
            
            _tutorialAvScaleContainer.DisplayTarget(_target);
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda AV Rating 1"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion

            #region [ Match AV Rating Tutorial: 2 ]
            yield return _videoPlayer.AwaitPlayback(19f);
            _videoPlayer.Pause();
            
            _target = new Vector2(0.6f, 0.5f);
            
            _tutorialAvScaleContainer.DisplayTarget(_target);
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda AV Rating 2"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion

            #region [ Match AV Rating Tutorial: 3 ]
            yield return _videoPlayer.AwaitPlayback(25f);
            _videoPlayer.Pause();
            
            _target = new Vector2(0.5f, -0.5f);
            
            _tutorialAvScaleContainer.DisplayTarget(_target);
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda AV Rating 3"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion
        }
        yield return _videoPlayer.Finished();
        yield return _videoPlayer.HideCoroutine();
        #endregion
        
        
        
        #region [ Spider Video Introduction ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Spider Introduction"], 
            _inputManager.AwaitParticipantConformationCoroutine());
        #endregion
        
        #region [ Spider Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        _videoPlayer.PlayVideo(_sessionData.tutorialVideos[1], DataManager.Activity.Tutorial);
        {
            #region [ Match AV Rating Tutorial: 1 ]
            yield return _videoPlayer.AwaitPlayback(10f);
            _videoPlayer.Pause();

            _target = new Vector2(-0.35f, -0.45f);
            
            _tutorialAvScaleContainer.DisplayTarget(_target);
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Spider AV Rating 1"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion

            #region [ Match AV Rating Tutorial: 2 ]
            yield return _videoPlayer.AwaitPlayback(25f);
            _videoPlayer.Pause();
            
            _target = new Vector2(-0.6f, 0.75f);
            
            _tutorialAvScaleContainer.DisplayTarget(_target);
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Spider AV Rating 2"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion
        }
        yield return _videoPlayer.Finished();
        yield return _videoPlayer.HideCoroutine();
        #endregion
        
        
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Continuous AV Reminder"], _inputManager.AwaitResearcherConformationCoroutine());
        
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial End"], _inputManager.AwaitResearcherConformationCoroutine());
        _experimentStateMachine.State = _experimentStateMachine.Experiment;
    }
}