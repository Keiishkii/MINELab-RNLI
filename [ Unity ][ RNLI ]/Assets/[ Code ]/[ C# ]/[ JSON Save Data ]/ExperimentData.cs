using System;
using System.Collections.Generic;

[Serializable]
public class ExperimentData
{
    public float transitionalWaitDuration;
    public List<VideoData> tutorialVideoData = new ();
    public List<VideoData> experimentVideoData = new ();
}