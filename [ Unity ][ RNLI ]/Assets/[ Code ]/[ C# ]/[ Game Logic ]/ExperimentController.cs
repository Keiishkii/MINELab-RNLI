using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class ExperimentController : MonoBehaviour
{
    #region [ Serialised Fields ]
    public SessionDataScriptableObject sessionData;
    public SessionStateFlags sessionProgressionFlags;
    [SerializeField] private ExperimentStateMachine _stateMachine = new ExperimentStateMachine();
    #endregion

    #region [ Unserialised Fields ]
    private InputManager _inputManager;
    private ExperimentUXML _experimentUxml;
    private readonly List<SceneDataScriptableObject> _scenes = new List<SceneDataScriptableObject>();
    #endregion


    
    private void Awake()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _experimentUxml = FindObjectOfType<ExperimentUXML>();
        
        _stateMachine.Initialise(this, _inputManager, _experimentUxml);
    }

    private void Start() => _stateMachine.Start();
}

[Flags]
public enum SessionStateFlags
{
    None = 0,
    Introduction = 1,
    ControllerTutorial = 2,
    Calibration = 4,
    ExperimentTutorial = 8,
    Experiment = 16,
}