using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region [ Serialised Fields ]
    public InputActionReference spaceInputActionReference;
    #endregion

    private void Awake()
    {
        spaceInputActionReference.action.performed += (context) => { Debug.Log("SPACE"); };
    }
}
