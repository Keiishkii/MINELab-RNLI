using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Expression Data", menuName = "[ MINE Lab ]/[ Experiment ]/Expression Data")]
public class ExpressionDataScriptableObject : ScriptableObject
{
    public string lookupKey;
    
    public string marker;
    public Sprite expressionSprite;
}