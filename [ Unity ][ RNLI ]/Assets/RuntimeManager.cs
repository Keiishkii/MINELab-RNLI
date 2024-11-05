using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeManager : MonoBehaviour
{
    #region [ Instance ]
    private static RuntimeManager _instance;
    public static RuntimeManager Instance => _instance ? _instance : _instance = FindObjectOfType<RuntimeManager>();
    #endregion

    #region [ Actions ]
    public Action update;
    public Action lateUpdate;
    public Action fixedUpdate;
    #endregion

    private void Update() => update?.Invoke();
    private void LateUpdate() => update?.Invoke();
    private void FixedUpdate() => fixedUpdate?.Invoke();
}
