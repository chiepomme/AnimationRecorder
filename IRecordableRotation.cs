#if UNITY_EDITOR
namespace AnimationRecorder
{
    public interface IRecordableRotation
    {
        void ConnectSegmentedRotations();
        void RecordCurrentValue(float time);
        void SetCurve();
    }
}
#endif
