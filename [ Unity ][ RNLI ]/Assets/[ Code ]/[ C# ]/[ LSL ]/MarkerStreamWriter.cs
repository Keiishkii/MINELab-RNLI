using System.Linq;
using LSL;
using UnityEngine;

public class MarkerStreamWriter<T> : IStreamWriter<T>
{
    public MarkerStreamWriter(string streamName, string streamType, string[] channelData, channel_format_t channelDataType) : base(streamName, streamType)
    {
        StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, channelData.Length, 0, channelDataType);
        XMLElement channels = streamInfo.desc().append_child("channels");
        
        foreach (string channel in channelData) channels.append_child("channel").append_child_value("label", $"{channel}");

        _outlet = new StreamOutlet(streamInfo);
        _currentSample = new T[1];
    }
    
    public void WriteMarker(in T[] marker)
    {
        PushOutput(_outlet, marker);
        
        #if UNITY_EDITOR
        string channels = marker.Aggregate("", (current, val) => current + $"{val}, ");
        channels = channels.Substring(0, channels.Length - 2);
        Debug.Log($"<color=#36a4ba>Marker</color>: [ {channels} ]");
        #endif
    }
}