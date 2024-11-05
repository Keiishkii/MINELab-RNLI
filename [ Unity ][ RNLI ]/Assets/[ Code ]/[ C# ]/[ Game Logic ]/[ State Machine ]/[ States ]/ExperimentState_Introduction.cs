using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class ExperimentState_Introduction : IExperimentState
{
    #region [ Unserialised Fields ]
    private IEnumerator _sessionIntroductionCoroutine;
    #endregion
    
    public ExperimentState_Introduction(string stateName) : base(stateName) { }
    
    public override void OnStateEnter() => _experimentController.StartCoroutine(_sessionIntroductionCoroutine = SessionIntroductionCoroutine());
    public override void OnStateExit()
    {
        if (!ReferenceEquals(_sessionIntroductionCoroutine, null)) _experimentController.StopCoroutine(_sessionIntroductionCoroutine);
    }

    private IEnumerator SessionIntroductionCoroutine()
    {
        if (!_experimentController.sessionProgressionFlags.HasFlag(SessionStateFlags.Introduction)) goto StateEnd;
        
        yield return _experimentUxml.instructionContainer.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Introduction"], _inputManager.AwaitResearcherConformationCoroutine());
        DataManager.StartDataCollection(_sessionData);
        
        StateEnd:
        _experimentStateMachine.State = _experimentStateMachine.ControllerTutorial;
    }
}
