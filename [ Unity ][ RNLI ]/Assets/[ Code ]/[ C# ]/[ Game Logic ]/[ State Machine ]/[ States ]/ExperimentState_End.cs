using System;
using System.Collections;

[Serializable]
public class ExperimentState_End : IExperimentState
{
    #region [ Unserialised Properties ]
    private IEnumerator _sessionEndCoroutine;
    #endregion
    
    public ExperimentState_End(string stateName) : base(stateName) { }

    public override void OnStateEnter() => _experimentController.StartCoroutine(_sessionEndCoroutine = SessionIntroductionCoroutine());
    
    public override void OnStateExit()
    {
        if (!ReferenceEquals(_sessionEndCoroutine, null)) _experimentController.StopCoroutine(_sessionEndCoroutine);
    }

    public IEnumerator SessionIntroductionCoroutine()
    {
        DataManager.EndDataCollection();
        
        _experimentUxml.instructionContainer.SetContent(_sessionData.instructionUIContentDictionary["Session Complete"]);
        yield return _experimentUxml.instructionContainer.DisplayCoroutine();
    }
}
