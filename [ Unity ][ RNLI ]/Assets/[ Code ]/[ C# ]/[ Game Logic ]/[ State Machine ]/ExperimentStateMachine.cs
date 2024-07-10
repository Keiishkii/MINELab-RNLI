using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExperimentStateMachine
{
    #region [ Unserialised Fields ]
    private ExperimentController _experimentController;
    #endregion
    
    #region [ States ]
    private IExperimentState _state;
    public IExperimentState State
    {
        get => _state;
        set
        {
            if (!ReferenceEquals(_state, null)) _state.OnStateExit();
            _state = value;
            _state.OnStateEnter();
            Debug.Log($"State Entered: {value}");
        }
    }

    [SerializeField] public ExperimentState_Introduction Introduction = new ();
    [SerializeField] public ExperimentState_Session Session = new ();
    [SerializeField] public ExperimentState_End End = new ();
    #endregion

    public void Initialise(ExperimentController experimentController, InputManager inputManager, ExperimentUXML experimentUxml)
    {
        Introduction.Initialise(this, experimentController, inputManager, experimentUxml);
        Session.Initialise(this, experimentController, inputManager, experimentUxml);
        End.Initialise(this, experimentController, inputManager, experimentUxml);
    }

    public void Start() => State = Introduction;
}
