using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region [ Instance ]
    private static InputManager _instance;
    public static InputManager Instance => _instance ? _instance : _instance = FindObjectOfType<InputManager>();
    #endregion
    
    #region [ Serialised Fields ]
    public InputActionReference researcherConformationActionReference;
    public InputActionReference participantConformationActionReference;
    public InputActionReference avScaleInputActionReference;
    public InputActionReference skipInputActionReference;
    #endregion

    private void Awake()
    {
        avScaleInputActionReference.action.canceled += (_) => NetworkManager.Instance.AVDataStreamWriter.WriteMarker(new []{0f, 0f});
        avScaleInputActionReference.action.performed += (context) =>
        {
            Vector2 vector = context.ReadValue<Vector2>();
            //NetworkManager.Instance.AVDataStreamWriter.WriteMarker(new []{vector.x, vector.y});
        };
    }

    public IEnumerator AwaitResearcherConformationCoroutine()
    {
        bool conformation = false;
        
        researcherConformationActionReference.action.performed += Test;
        skipInputActionReference.action.performed += Skip;
        yield return new WaitUntil(() => conformation);
        skipInputActionReference.action.performed -= Skip;
        researcherConformationActionReference.action.performed -= Test;
        
        yield break;

        void Test(InputAction.CallbackContext callbackContext) => conformation = true;
        void Skip(InputAction.CallbackContext callbackContext) => conformation = true;
    }

    public IEnumerator AwaitParticipantConformationCoroutine()
    {
        bool conformation = false;
        
        participantConformationActionReference.action.performed += Test;
        skipInputActionReference.action.performed += Skip;
        yield return new WaitUntil(() => conformation);
        skipInputActionReference.action.performed -= Skip;
        participantConformationActionReference.action.performed -= Test;
        
        yield break;

        void Test(InputAction.CallbackContext callbackContext) => conformation = true;
        void Skip(InputAction.CallbackContext callbackContext) => conformation = true;
    }

    public IEnumerator AwaitMatchingAVRatingCoroutine(Vector2 targetRating)
    {
        bool conformation = false;
        
        avScaleInputActionReference.action.performed += Test;
        skipInputActionReference.action.performed += Skip;
        yield return new WaitUntil(() => conformation);
        skipInputActionReference.action.performed -= Skip;
        avScaleInputActionReference.action.performed -= Test;
        
        yield break;

        void Test(InputAction.CallbackContext callbackContext) => conformation = Vector2.Distance(targetRating, callbackContext.ReadValue<Vector2>()) < 0.2f;
        void Skip(InputAction.CallbackContext callbackContext) => conformation = true;
    }
}
