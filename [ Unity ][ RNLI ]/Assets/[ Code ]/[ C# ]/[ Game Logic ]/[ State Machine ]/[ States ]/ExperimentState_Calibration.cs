using System.Collections;
using UnityEngine;

public class ExperimentState_Calibration : IExperimentState
{
    #region [ Unserialised Fields ]
    private IEnumerator _sessionCalibrationCoroutine;
    private ExpressionDataScriptableObject _expressionData;
    #endregion
    
    public ExperimentState_Calibration(string stateName) : base(stateName) { }

    public override void OnStateEnter() => _experimentController.StartCoroutine(_sessionCalibrationCoroutine = SessionCalibration());
    public override void OnStateExit() { if (!ReferenceEquals(_sessionCalibrationCoroutine, null)) _experimentController.StopCoroutine(_sessionCalibrationCoroutine); }

    private IEnumerator SessionCalibration()
    {
        #region [ Calibration Instructions ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Calibration Introduction"], 
            _inputManager.AwaitParticipantConformationCoroutine());
        #endregion


        #region [ Heartbeat Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Heartbeat Calibration"], 
            _expressionData = _sessionData.expressionDataDictionary["Heartbeat"],
            RecordExpression(_expressionData, _sessionData.baselineHeartbeatCalibrationDuration));
        #endregion
        
        
        #region [ Neutral Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Expression Neutral"], 
            _expressionData = _sessionData.expressionDataDictionary["Neutral"],
            RecordExpression(_expressionData, _sessionData.expressionCalibrationDuration));
        #endregion

        #region [ Smile Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Expression Smile"], 
            _expressionData = _sessionData.expressionDataDictionary["Smile"],
            RecordExpression(_expressionData, _sessionData.expressionCalibrationDuration));
        #endregion

        #region [ Neutral Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Expression Neutral"], 
            _expressionData = _sessionData.expressionDataDictionary["Neutral"],
            RecordExpression(_expressionData, _sessionData.expressionCalibrationDuration));
        #endregion

        #region [ Frown Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Expression Frown"], 
            _expressionData = _sessionData.expressionDataDictionary["Frown"],
            RecordExpression(_expressionData, _sessionData.expressionCalibrationDuration));
        #endregion

        #region [ Neutral Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Expression Neutral"], 
            _expressionData = _sessionData.expressionDataDictionary["Neutral"],
            RecordExpression(_expressionData, _sessionData.expressionCalibrationDuration));
        #endregion

        #region [ Raised Eyebrow Calibration ]
        yield return _calibrator.DisplayExpressionCoroutine(_sessionData.instructionUIContentDictionary["Expression Raised Eyebrow"], 
            _expressionData = _sessionData.expressionDataDictionary["Raised Eyebrow"],
            RecordExpression(_expressionData, _sessionData.expressionCalibrationDuration));
        #endregion
        
        #region [ Calibration Instructions ]
        yield return _instructionDisplay.DisplayTextFieldCoroutine(_sessionData.instructionUIContentDictionary["Calibration Complete"], 
            _inputManager.AwaitParticipantConformationCoroutine());
        #endregion

        _experimentStateMachine.State = _experimentStateMachine.ExperimentTutorial;
        
        yield break;

        IEnumerator RecordExpression(ExpressionDataScriptableObject expressionData, float duration)
        {
            yield return _inputManager.AwaitParticipantConformationCoroutine();
                
            NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{$"{expressionData.marker} - Start"});
            yield return _timer.StartTimer(duration);
            NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{$"{expressionData.marker} - End"});
        }
    }
}