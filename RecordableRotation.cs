using UnityEngine;

namespace AnimationRecorder
{
    public class RecordableRotation
    {
        readonly Transform transform;
        readonly RecordableCurve curveX;
        readonly RecordableCurve curveY;
        readonly RecordableCurve curveZ;
        readonly RecordableCurve curveW;

        readonly AnimationClip clip;
        readonly string relativePath;

        public RecordableRotation(AnimationClip clip, string path, Transform target)
        {
            this.clip = clip;
            relativePath = path;
            transform = target;

            curveX = new RecordableCurve();
            curveY = new RecordableCurve();
            curveZ = new RecordableCurve();
            curveW = new RecordableCurve();
        }

        public void RecordCurrentValue(float time)
        {
            curveX.Record(time, transform.localRotation.x);
            curveY.Record(time, transform.localRotation.y);
            curveZ.Record(time, transform.localRotation.z);
            curveW.Record(time, transform.localRotation.w);
        }

        public void SetCurve()
        {
            clip.SetCurve(relativePath, typeof(Transform), "localRotation.x", curveX.curve);
            clip.SetCurve(relativePath, typeof(Transform), "localRotation.y", curveY.curve);
            clip.SetCurve(relativePath, typeof(Transform), "localRotation.z", curveZ.curve);
            clip.SetCurve(relativePath, typeof(Transform), "localRotation.w", curveW.curve);
        }
    }
}
