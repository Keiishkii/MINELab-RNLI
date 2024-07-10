using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
[CreateAssetMenu(fileName = "Session", menuName = "[ MINE Lab ]/[ Experiment ]/Session")]
public class SessionDataScriptableObject : ScriptableObject
{
    public List<SceneDataScriptableObject> trials = new ();
}