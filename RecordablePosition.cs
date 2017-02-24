using UnityEngine;

namespace AnimationRecorder
{
    public class RecordablePosition
    {
        readonly AnimationClip clip;
        readonly string relativePath;

        readonly Transform transform;
        readonly RecordableCurve curveX;
        readonly RecordableCurve curveY;
        readonly RecordableCurve curveZ;

        public RecordablePosition(AnimationClip clip, string path, Transform target)
        {
            this.clip = clip;
            relativePath = path;
            transform = target;

            curveX = new RecordableCurve();
            curveY = new RecordableCurve();
            curveZ = new RecordableCurve();
        }

        public void RecordCurrentValue(float time)
        {
            curveX.Record(time, transform.localPosition.x);
            curveY.Record(time, transform.localPosition.y);
            curveZ.Record(time, transform.localPosition.z);
        }

        public void SetCurve()
        {
            clip.SetCurve(relativePath, typeof(Transform), "localPosition.x", curveX.curve);
            clip.SetCurve(relativePath, typeof(Transform), "localPosition.y", curveY.curve);
            clip.SetCurve(relativePath, typeof(Transform), "localPosition.z", curveZ.curve);
        }
    }
}
