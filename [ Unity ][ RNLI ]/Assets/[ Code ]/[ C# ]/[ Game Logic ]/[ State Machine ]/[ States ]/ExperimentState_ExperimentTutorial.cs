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
        if (!_experimentController.sessionProgressionFlags.HasFlag(SessionStateFlags.ExperimentTutorial)) goto StateEnd;
        
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial Start"], _inputManager.AwaitResearcherConformationCoroutine());

        yield return TutorialPhaseOne();
        yield return TutorialPhaseTwo();
        yield return TutorialPhaseThree();
        
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Continuous AV Reminder"], _inputManager.AwaitResearcherConformationCoroutine());
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial End"], _inputManager.AwaitResearcherConformationCoroutine());
        
        StateEnd:
        _experimentStateMachine.State = _experimentStateMachine.Experiment;
    }

    private IEnumerator TutorialPhaseOne()
    {
        yield return _videoPlayer.DisplayCoroutine();
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial Start - Phase One"], _inputManager.AwaitResearcherConformationCoroutine());
        
        // Photo 1 - (Positive)
        #region [ Photo 1 ]
        _videoPlayer.DisplayImage(_sessionData.TutorialPhotos["Photo - 1"]);
        yield return _timer.StartTimer(4, false);
        _tutorialAvScaleContainer.DisplayTarget(new Vector2(0.35f, -0.4575f));
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Photo 1 - AV Rating"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
        #endregion
        
        // Photo 3 - (Neutral)
        #region [ Photo 3 ]
        _videoPlayer.DisplayImage(_sessionData.TutorialPhotos["Photo - 3"]);
        yield return _timer.StartTimer(4, false);
        _tutorialAvScaleContainer.DisplayTarget(new Vector2(-0.005f, -0.325f));
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Photo 3 - AV Rating"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
        #endregion
        
        // Photo 5 - (Negative)
        #region [ Photo 5 ]
        _videoPlayer.DisplayImage(_sessionData.TutorialPhotos["Photo - 5"]);
        yield return _timer.StartTimer(4, false);
        _tutorialAvScaleContainer.DisplayTarget(new Vector2(-0.7725f, 0.5925f));
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Photo 5 - AV Rating"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
        #endregion
        
        // Photo 2 - (Positive)
        #region [ Photo 2 ]
        _videoPlayer.DisplayImage(_sessionData.TutorialPhotos["Photo - 2"]);
        yield return _timer.StartTimer(4, false);
        _tutorialAvScaleContainer.DisplayTarget(new Vector2(0.755f, 0.1325f));
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Photo 2 - AV Rating"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
        #endregion
        
        // Photo 4 - (Neutral)
        #region [ Photo 4 ]
        _videoPlayer.DisplayImage(_sessionData.TutorialPhotos["Photo - 4"]);
        yield return _timer.StartTimer(4, false);
        _tutorialAvScaleContainer.DisplayTarget(new Vector2(0.1075f, -0.165f));
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Photo 4 - AV Rating"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
        #endregion
        
        // Photo 6 - (Negative)
        #region [ Photo 6 ]
        _videoPlayer.DisplayImage(_sessionData.TutorialPhotos["Photo - 6"]);
        yield return _timer.StartTimer(4, false);
        _tutorialAvScaleContainer.DisplayTarget(new Vector2(-0.6625f, 0.25f));
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Photo 6 - AV Rating"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
        #endregion
        
        yield return _videoPlayer.HideCoroutine();
    }

    private IEnumerator TutorialPhaseTwo()
    {
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial Start - Phase One"], _inputManager.AwaitResearcherConformationCoroutine());
        
        // Panda Video - First Time (with instructions)
        #region [ Panda Video Introduction ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda Introduction"], _inputManager.AwaitParticipantConformationCoroutine());
        #endregion
        #region [ Panda Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        _videoPlayer.PlayVideo(_sessionData.TutorialVideos["Tutorial - Panda"], DataManager.Activity.Tutorial);
        {
            #region [ Match AV Rating Tutorial: 1 ]
            yield return _videoPlayer.AwaitPlayback(8f);
            _videoPlayer.Pause();
            
            _tutorialAvScaleContainer.DisplayTarget(new Vector2(0.5f, -0.5f));
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda AV Rating 1"], _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion

            #region [ Match AV Rating Tutorial: 2 ]
            yield return _videoPlayer.AwaitPlayback(19f);
            _videoPlayer.Pause();
            
            _tutorialAvScaleContainer.DisplayTarget(new Vector2(0.6f, 0.5f));
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda AV Rating 2"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion

            #region [ Match AV Rating Tutorial: 3 ]
            yield return _videoPlayer.AwaitPlayback(25f);
            _videoPlayer.Pause();
            
            _tutorialAvScaleContainer.DisplayTarget(new Vector2(0.5f, -0.5f));
            yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Panda AV Rating 3"], 
                _inputManager.AwaitMatchingAVRatingCoroutine(_target));
            _videoPlayer.Play();
            #endregion
        }
        yield return _videoPlayer.Finished();
        yield return _videoPlayer.HideCoroutine();
        #endregion
        
        // Spider Video - First Time (with instructions)
        #region [ Spider Video Introduction ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Spider Introduction"], 
            _inputManager.AwaitParticipantConformationCoroutine());
        #endregion
        #region [ Spider Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        _videoPlayer.PlayVideo(_sessionData.TutorialVideos["Tutorial - Spiders"], DataManager.Activity.Tutorial);
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
    }

    private IEnumerator TutorialPhaseThree()
    {
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Tutorial Start - Phase Three"], _inputManager.AwaitResearcherConformationCoroutine());

        List<Vector2> samples = new ();
        InputActionReference avInput = InputManager.Instance.avScaleInputActionReference;
        _videoPlayer.PointerDisplayed = false;
        
        // Panda Video - Second Time (no instructions)
        #region [ Panda Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        yield return new WaitForSeconds(1f);
        
        samples.Clear();
        RuntimeManager.Instance.fixedUpdate += RecordSamples;
        
        _videoPlayer.PlayVideo(_sessionData.TutorialVideos["Tutorial - Dog"], DataManager.Activity.Experiment);
        yield return _videoPlayer.Finished();

        List<Vector2> pandaVideoSamples = new List<Vector2>(samples);
        RuntimeManager.Instance.fixedUpdate -= RecordSamples;
        
        yield return new WaitForSeconds(1f);
        yield return _videoPlayer.HideCoroutine();
        #endregion
        
        yield return _timer.StartTimer(5, true);
        
        // Spider Video - Second Time (no instructions)
        #region [ Spider Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        yield return new WaitForSeconds(1f);
        
        samples.Clear();
        RuntimeManager.Instance.fixedUpdate += RecordSamples;
        
        _videoPlayer.PlayVideo(_sessionData.TutorialVideos["Tutorial - Plane Crash"], DataManager.Activity.Experiment);
        yield return _videoPlayer.Finished();
        
        List<Vector2> spiderVideoSamples = new List<Vector2>(samples);
        RuntimeManager.Instance.fixedUpdate -= RecordSamples;
        
        yield return new WaitForSeconds(1f);
        yield return _videoPlayer.HideCoroutine();
        #endregion

        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Replaying Hidden Inputs"], _inputManager.AwaitResearcherConformationCoroutine());

        
        // Panda Video - Third Time (replay)
        #region [ Panda Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        yield return new WaitForSeconds(1f);
        
        _videoPlayer.ReplayPointerVisibility(true);
        _experimentController.StartCoroutine(_videoPlayer.ReplaySampleData(pandaVideoSamples));
        _videoPlayer.PlayVideo(_sessionData.TutorialVideos["Tutorial - Dog"], DataManager.Activity.Experiment);
        yield return _videoPlayer.Finished();
        _videoPlayer.ReplayPointerVisibility(false);
        
        yield return new WaitForSeconds(1f);
        yield return _videoPlayer.HideCoroutine();
        #endregion
        
        yield return _timer.StartTimer(5, true);
        
        // Spider Video - Third Time (replay)
        #region [ Spider Video Playback ]
        yield return _videoPlayer.DisplayCoroutine();
        yield return new WaitForSeconds(1f);

        _videoPlayer.ReplayPointerVisibility(true);
        _experimentController.StartCoroutine(_videoPlayer.ReplaySampleData(spiderVideoSamples));
        _videoPlayer.PlayVideo(_sessionData.TutorialVideos["Tutorial - Plane Crash"], DataManager.Activity.Experiment);
        yield return _videoPlayer.Finished();
        _videoPlayer.ReplayPointerVisibility(false);
        
        yield return new WaitForSeconds(1f);
        yield return _videoPlayer.HideCoroutine();
        #endregion
        
        Debug.Log($"Panda Samples Count: {pandaVideoSamples.Count}");
        Debug.Log($"Spider Samples Count: {spiderVideoSamples.Count}");
        
        yield break;

        void RecordSamples()
        {
            samples.Add(avInput.action.ReadValue<Vector2>());
        }
    }
}