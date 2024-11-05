using System.Collections;
using UnityEngine;

public class ExperimentState_ControllerTutorial : IExperimentState
{
    #region [ Unserialised Fields ]
    private IEnumerator _sessionControllerTutorialCoroutine;
    private Vector2 _target;
    #endregion
    
    public ExperimentState_ControllerTutorial(string stateName) : base(stateName) { }

    public override void OnStateEnter() => _experimentController.StartCoroutine(_sessionControllerTutorialCoroutine = SessionControllerTutorialCoroutine());
    public override void OnStateExit() { if (!ReferenceEquals(_sessionControllerTutorialCoroutine, null)) _experimentController.StopCoroutine(_sessionControllerTutorialCoroutine); }

    private IEnumerator SessionControllerTutorialCoroutine()
    {
        if (!_experimentController.sessionProgressionFlags.HasFlag(SessionStateFlags.ControllerTutorial)) goto StateEnd;
        
        #region [ Controller Tutorial: 1 ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Controller Tutorial 1"], 
            _inputManager.AwaitResearcherConformationCoroutine());
        #endregion
        
        #region [ Controller Tutorial: 2 ]
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Controller Tutorial 2"], 
            _inputManager.AwaitResearcherConformationCoroutine());
        #endregion
        
        #region [ Controller Tutorial: 3 ]
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Controller Tutorial 3"], 
            _inputManager.AwaitParticipantConformationCoroutine());
        #endregion
        
        #region [ Controller Tutorial: 4 ]
        yield return _tutorialAvScaleContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Controller Tutorial 4"], 
            ControllerAVValueTestCoroutine());
        #endregion

        StateEnd:
        _experimentStateMachine.State = _experimentStateMachine.Calibration;
        
        yield break;
        
        IEnumerator ControllerAVValueTestCoroutine()
        {
            _tutorialAvScaleContainer.DisplayTarget(_target = new Vector2(0.25f, 0.61f));
            yield return _inputManager.AwaitMatchingAVRatingCoroutine(_target);
            
            _tutorialAvScaleContainer.DisplayTarget(_target = new Vector2(0, -0.75f));
            yield return _inputManager.AwaitMatchingAVRatingCoroutine(_target);
            
            _tutorialAvScaleContainer.DisplayTarget(_target = new Vector2(-0.3f, 0.3f));
            yield return _inputManager.AwaitMatchingAVRatingCoroutine(_target);
            
            _tutorialAvScaleContainer.DisplayTarget(_target = new Vector2(0.95f, 0));
            yield return _inputManager.AwaitMatchingAVRatingCoroutine(_target);
        }
    }
}