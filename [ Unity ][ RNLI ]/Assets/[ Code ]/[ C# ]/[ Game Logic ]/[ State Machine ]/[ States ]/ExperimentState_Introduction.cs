using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class ExperimentState_Introduction : IExperimentState
{
    #region [ Unserialised Fields ]
    private IEnumerator _sessionConformationCoroutine;
    #endregion
    
    public override void OnStateEnter()
    {
        _experimentUxml.ShowIntroductionVisualElement();
        _experimentController.StartCoroutine(_sessionConformationCoroutine = SessionConformationCoroutine());
    }

    public override void OnStateExit()
    {
        if (!ReferenceEquals(_sessionConformationCoroutine, null)) _experimentController.StopCoroutine(_sessionConformationCoroutine);
    }

    private IEnumerator SessionConformationCoroutine()
    {
        bool conformation = false;
        
        _inputManager.spaceInputActionReference.action.performed += Test;
        yield return new WaitUntil(() => conformation);
        _inputManager.spaceInputActionReference.action.performed -= Test;

        _experimentUxml.HideIntroductionVisualElement();
        _experimentStateMachine.State = _experimentStateMachine.Session;
        
        yield break;

        void Test(InputAction.CallbackContext callbackContext) => conformation = true;
    }
}
