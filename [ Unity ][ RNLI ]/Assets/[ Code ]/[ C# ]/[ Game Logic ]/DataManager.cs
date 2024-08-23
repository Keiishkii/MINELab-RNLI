using System;
using System.IO;
using UnityEngine;

public static class DataManager
{
    #region [ Properties ]
    private static ExperimentData _experimentData; 
    #endregion

    #region [ Activity ]
    public enum Activity
    {
        Tutorial,
        Experiment
    }
    #endregion

    #region [ Events ]
    public enum VideoEvent
    {
        VideoStart,
        VideoEnd
    }
    #endregion
    
    
    
    public static void StartDataCollection(SessionDataScriptableObject sessionDataScriptableObject)
    {
        NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{"Data Collection Started"});
        _experimentData = new ExperimentData
        {
            transitionalWaitDuration = sessionDataScriptableObject.transitionalWaitDuration
        };
    }
    
    public static void EndDataCollection()
    {
        NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{"Data Collection Ended"});
        
        string filename = $"{string.Concat(($"{DateTime.Now:f}").Split(Path.GetInvalidFileNameChars()))}.json";
        string directory = $"{Application.persistentDataPath}/[ Data ]";
        
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        
        string path = $"{directory}/{filename}";
        
        string experimentJSON = JsonUtility.ToJson(_experimentData, true);
        File.WriteAllText(path, experimentJSON);
    }
    
    public static void AddVideoEvent(SceneDataScriptableObject sceneData, VideoEvent videoEvent, Activity activity)
    {
        NetworkManager.Instance.MarkerStreamWriter.WriteMarker(new []{$"Video: {sceneData.videoLabel}, Event: {videoEvent}"});
        
        (activity switch
        {
            Activity.Experiment => _experimentData.experimentVideoData,
            Activity.Tutorial => _experimentData.tutorialVideoData,
            _ => throw new ArgumentOutOfRangeException(nameof(activity), activity, null)
        }).Add(new VideoData()
        {
            VideoLabel = sceneData.videoLabel,
            VideoEvent = videoEvent,
            Time = $"{DateTime.Now:f.fff}"
        });
    }
}
