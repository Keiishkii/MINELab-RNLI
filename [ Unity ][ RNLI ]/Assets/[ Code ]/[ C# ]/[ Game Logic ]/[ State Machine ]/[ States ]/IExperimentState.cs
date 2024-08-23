using System;
using Unity.VisualScripting;

[Serializable]
public abstract class IExperimentState
{
    #region [ Unserialised Feilds ]
    [NonSerialized] public readonly string StateName;
    [NonSerialized] protected SessionDataScriptableObject _sessionData;
    [NonSerialized] protected ExperimentStateMachine _experimentStateMachine;
    [NonSerialized] protected ExperimentController _experimentController;
    [NonSerialized] protected ExperimentUXML _experimentUxml;
    [NonSerialized] protected InputManager _inputManager;
    [NonSerialized] protected ExperimentUXML.VideoPlayerContainer _videoPlayer;
    [NonSerialized] protected ExperimentUXML.InstructionContainer _instructionDisplay;
    [NonSerialized] protected ExperimentUXML.CalibrationContainer _calibrator;
    [NonSerialized] protected ExperimentUXML.TimerContainer _timer;
    [NonSerialized] protected ExperimentUXML.TutorialAVScaleContainer _tutorialAvScaleContainer;
    #endregion

    protected IExperimentState(string stateName) => StateName = stateName;
    
    public virtual void Initialise(ExperimentStateMachine experimentStateMachine, ExperimentController experimentController, InputManager inputManager, ExperimentUXML experimentUxml)
    {
        _experimentStateMachine = experimentStateMachine;
        _experimentController = experimentController;
        _experimentUxml = experimentUxml;
        _inputManager = inputManager;

        _sessionData = _experimentController.sessionData;
        _calibrator = _experimentUxml.calibrationContainer;
        _timer = _experimentUxml.timerContainer;
        _videoPlayer = _experimentUxml.videoPlayerContainer;
        _instructionDisplay = _experimentUxml.instructionContainer;
        _tutorialAvScaleContainer = _experimentUxml.tutorialAVScaleContainer;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateStay() { }
    public virtual void OnStateExit() { }
}
