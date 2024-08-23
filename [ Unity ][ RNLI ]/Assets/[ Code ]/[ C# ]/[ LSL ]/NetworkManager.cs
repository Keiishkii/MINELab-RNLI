using System;
using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;
using Object = UnityEngine.Object;

public class NetworkManager : MonoBehaviour
{
    #region [ Instance ]
    private static NetworkManager _instance;
    public static NetworkManager Instance => _instance ? _instance : _instance = FindObjectOfType<NetworkManager>();
    #endregion

    #region [ Streams ]
    public MarkerStreamWriter<string> MarkerStreamWriter;
    public MarkerStreamWriter<float> AVDataStreamWriter;
    private FixedUpdateStreamWriter<float> AVDataStreamWriterContinuous;
    #endregion

    private void Awake()
    {
        MarkerStreamWriter = new MarkerStreamWriter<string>("Marker", "string", new []{"Marker"}, channel_format_t.cf_string);
        AVDataStreamWriter = new MarkerStreamWriter<float>("AV Rating Changes", "float", new []{"X", "Y"}, channel_format_t.cf_float32);
        AVDataStreamWriterContinuous = new FixedUpdateStreamWriter<float>("Av Rating Continuous", "float", new []{"X", "Y"}, channel_format_t.cf_float32,
            () =>
            {
                Vector2 avRating = InputManager.Instance.avScaleInputActionReference.action.ReadValue<Vector2>();
                return new[] { avRating.x, avRating.y };
            });
        
        AVDataStreamWriterContinuous.StartStream();
    }
}