using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
[CreateAssetMenu(fileName = "Session", menuName = "[ MINE Lab ]/[ Experiment ]/Session")]
public class SessionDataScriptableObject : ScriptableObject
{
    #region [ Serialised Fields ]
    public float expressionCalibrationDuration;
    public float baselineHeartbeatCalibrationDuration;
    public float transitionalWaitDuration;

    public List<SceneDataScriptableObject> tutorialVideos = new ();
    public List<SceneDataScriptableObject> experimentVideos = new ();
    public List<InstructionUIContentScriptableObject> instructionUIContent = new ();
    public List<ExpressionDataScriptableObject> expressionData = new ();
    #endregion

    #region [ Unserialised Fields ]
    private Dictionary<string, InstructionUIContentScriptableObject> _instructionUIContentDictionary;
    public Dictionary<string, InstructionUIContentScriptableObject> instructionUIContentDictionary
    {
        get
        {
            if (!ReferenceEquals(_instructionUIContentDictionary, null)) return _instructionUIContentDictionary;
            
            _instructionUIContentDictionary = new Dictionary<string, InstructionUIContentScriptableObject>();
            foreach (InstructionUIContentScriptableObject content in instructionUIContent)
            {
                _instructionUIContentDictionary.Add(content.lookupKey, content);
            }
            
            return _instructionUIContentDictionary;
        }
    }
    
    private Dictionary<string, ExpressionDataScriptableObject> _expressionDataDictionary;
    public Dictionary<string, ExpressionDataScriptableObject> expressionDataDictionary
    {
        get
        {
            if (!ReferenceEquals(_expressionDataDictionary, null)) return _expressionDataDictionary;
            
            _expressionDataDictionary = new Dictionary<string, ExpressionDataScriptableObject>();
            foreach (ExpressionDataScriptableObject content in expressionData)
            {
                _expressionDataDictionary.Add(content.lookupKey, content);
            }
            
            return _expressionDataDictionary;
        }
    }
    #endregion
}