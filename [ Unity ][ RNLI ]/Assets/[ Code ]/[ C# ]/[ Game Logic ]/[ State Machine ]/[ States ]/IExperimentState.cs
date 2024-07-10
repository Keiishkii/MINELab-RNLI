using System;
using Unity.VisualScripting;

[Serializable]
public abstract class IExperimentState
{
    #region [ Unserialised Feilds ]
    [DoNotSerialize] protected ExperimentStateMachine _experimentStateMachine;
    [DoNotSerialize] protected ExperimentController _experimentController;
    [DoNotSerialize] protected ExperimentUXML _experimentUxml;
    [DoNotSerialize] protected InputManager _inputManager;
    #endregion

    public virtual void Initialise(ExperimentStateMachine experimentStateMachine, ExperimentController experimentController, InputManager inputManager, ExperimentUXML experimentUxml)
    {
        _experimentStateMachine = experimentStateMachine;
        _experimentController = experimentController;
        _experimentUxml = experimentUxml;
        _inputManager = inputManager;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateStay() { }
    public virtual void OnStateExit() { }
}
