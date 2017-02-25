#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AnimationRecorder
{
    public class RecordableQuaternionRotation : IRecordableRotation
    {
        readonly AnimationClip clip;
        readonly string relativePath;

        readonly Transform transform;
        readonly RecordableCurve curveX;
        readonly RecordableCurve curveY;
        readonly RecordableCurve curveZ;
        readonly RecordableCurve curveW;

        public RecordableQuaternionRotation(AnimationClip clip, string path, Transform target)
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

        public void ConnectSegmentedRotations()
        {
            // 必要がない
        }

        public void SetCurve()
        {
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "m_LocalRotation.x" }, curveX.curve);
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "m_LocalRotation.y" }, curveY.curve);
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "m_LocalRotation.z" }, curveZ.curve);
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "m_LocalRotation.w" }, curveW.curve);
        }
    }
}
#endif
