using System;
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
            
            NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{$"State Change: {_state.StateName}"});
            _state.OnStateEnter();
        }
    }

    [SerializeField] public ExperimentState_Introduction Introduction = new ("Introduction");
    [SerializeField] public ExperimentState_ControllerTutorial ControllerTutorial = new ("Controller Tutorial");
    [SerializeField] public ExperimentState_Calibration Calibration = new ("Calibration");
    [SerializeField] public ExperimentState_ExperimentTutorial ExperimentTutorial = new ("Experiment Tutorial");
    [SerializeField] public ExperimentState_Experiment Experiment = new ("Experiment");
    [SerializeField] public ExperimentState_End End = new ("End");
    #endregion

    public void Initialise(ExperimentController experimentController, InputManager inputManager, ExperimentUXML experimentUxml)
    {
        Introduction.Initialise(this, experimentController, inputManager, experimentUxml);
        ControllerTutorial.Initialise(this, experimentController, inputManager, experimentUxml);
        Calibration.Initialise(this, experimentController, inputManager, experimentUxml);
        ExperimentTutorial.Initialise(this, experimentController, inputManager, experimentUxml);
        Experiment.Initialise(this, experimentController, inputManager, experimentUxml);
        End.Initialise(this, experimentController, inputManager, experimentUxml);
    }

    public void Start() => State = Introduction;
}
