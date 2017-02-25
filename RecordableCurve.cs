#if UNITY_EDITOR
using UnityEngine;

namespace AnimationRecorder
{
    public class RecordableCurve
    {
        public readonly AnimationCurve curve;

        public RecordableCurve()
        {
            curve = new AnimationCurve();
        }

        public void Record(float time, float value)
        {
            curve.AddKey(time, value);
        }
    }
}
#endif
