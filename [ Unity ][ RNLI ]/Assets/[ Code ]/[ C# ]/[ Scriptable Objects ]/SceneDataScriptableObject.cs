using System;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
[CreateAssetMenu(fileName = "Scene", menuName = "[ MINE Lab ]/[ Experiment ]/Scene")]
public class SceneDataScriptableObject : ScriptableObject
{
    public string videoLabel;
    public VideoClip clip;
}