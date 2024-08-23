using System;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Instruction UI Content", menuName = "[ MINE Lab ]/[ Experiment ]/Instruction UI Content")]
public class InstructionUIContentScriptableObject : ScriptableObject
{
    #if UNITY_EDITOR
    #region [ Editor ]
    [CustomEditor(typeof(InstructionUIContentScriptableObject))]
    public class InstructionUIContentScriptableObjectEditor : KeiishkiiLib.CustomInspector<InstructionUIContentScriptableObject>
    {
        protected override void OnInspectorRender()
        {
            _targetScript.lookupKey = EditorGUILayout.TextField("Lookup Key:", _targetScript.lookupKey);
            
            EditorStyles.textField.wordWrap = true;
            KeiishkiiLib.InspectorUtility.BooleanField("Display Title:", ref _targetScript.displayTitle);
            if (_targetScript.displayTitle) _targetScript.title = EditorGUILayout.TextArea(_targetScript.title);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.BooleanField("Display Content:", ref _targetScript.displayContent);
            if (_targetScript.displayContent) _targetScript.content = EditorGUILayout.TextArea(_targetScript.content);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.BooleanField("Display Continue Condition:", ref _targetScript.displayFooter);
            if (_targetScript.displayFooter) _targetScript.footer = EditorGUILayout.TextArea(_targetScript.footer);
            
            EditorUtility.SetDirty(_targetScript);
        }
    }
    #endregion
    #endif
    
    #region [ Serialised Fields ]
    public string lookupKey;

    public bool displayTitle;
    [TextArea(1,10000)] public string title;
    public bool displayContent;
    [TextArea(8,10000)] public string content;
    public bool displayFooter;
    [TextArea(1,10000)] public string footer;
    #endregion
}