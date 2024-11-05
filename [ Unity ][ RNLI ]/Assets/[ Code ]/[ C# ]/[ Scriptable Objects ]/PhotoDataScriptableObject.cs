using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Photo", menuName = "[ MINE Lab ]/[ Experiment ]/Photo")]
public class PhotoDataScriptableObject : ScriptableObject
{
    public string photoLabel;
    public Texture2D photo;
}