using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LSL;
using UnityEngine;
using Object = UnityEngine.Object;

public class FixedUpdateStreamWriter<T> : IStreamWriter<T>
{
    #region [ Properties ]
    private readonly NetworkManager _networkManager;
    private readonly IEnumerator _updateCoroutine;
    private readonly Func<T[]> _function;
    private bool _streamActive;
    #endregion
    
    public FixedUpdateStreamWriter(string streamName, string streamType, string[] channelData, channel_format_t channelDataType, Func<T[]> function) : base(streamName, streamType)
    {
        StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, channelData.Length, 50, channelDataType);
        XMLElement channels = streamInfo.desc().append_child("channels");
        
        foreach (string channel in channelData) channels.append_child("channel").append_child_value("label", $"{channel}");

        _outlet = new StreamOutlet(streamInfo);
        _currentSample = new T[1];

        _updateCoroutine = UpdateCoroutine();
        _networkManager = Object.FindObjectOfType<NetworkManager>();
        _function = function;
    }

    private IEnumerator UpdateCoroutine()
    {
        while (_streamActive)
        {
            _currentSample = _function.Invoke();
            PushOutput(_outlet, _currentSample);
            
            yield return null;
        }
    }

    public void StartStream()
    {
        if (_streamActive) return;
        _streamActive = true;

        _networkManager.StartCoroutine(_updateCoroutine);
    }
    
    public void EndStream()
    {
        if (!_streamActive) return;
        _streamActive = false;
        
        _networkManager.StopCoroutine(_updateCoroutine);
    }
}