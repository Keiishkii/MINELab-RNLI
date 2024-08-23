using LSL;

public abstract class IStreamWriter<T>
{
    #region [ Properties ]
    protected readonly string _streamName;
    protected readonly string _streamType;

    protected StreamOutlet _outlet;
    protected T[] _currentSample;
    #endregion

    protected IStreamWriter(string streamName, string streamType)
    {
        _streamName = streamName;
        _streamType = streamType;
    }

    protected static void PushOutput(in StreamOutlet outlet, in T[] sample)
    {
        switch (sample)
        {
            case not null when sample is char[] castedSample:
            {
                outlet.push_sample(castedSample);
            } break;
            case not null when sample is double[] castedSample:
            {
                outlet.push_sample(castedSample);
            } break;
            case not null when sample is float[] castedSample:
            {
                outlet.push_sample(castedSample);
            } break;
            case not null when sample is int[] castedSample:
            {
                outlet.push_sample(castedSample);
            } break;
            case not null when sample is short[] castedSample:
            {
                outlet.push_sample(castedSample);
            } break;
            case not null when sample is string[] castedSample:
            {
                outlet.push_sample(castedSample);
            } break;
        }
    }
}