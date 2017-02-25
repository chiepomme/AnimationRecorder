#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AnimationRecorder
{
    // http://mebiustos.hatenablog.com/entry/2015/09/16/230000
    // https://github.com/MattRix/UnityDecompiled/blob/master/UnityEditor/UnityEditor/RotationCurveInterpolation.cs
    // http://answers.unity3d.com/questions/26447/how-to-change-the-animationcurve-interpolation-qua.html
    public class RecordableEulerRotation : IRecordableRotation
    {
        readonly Transform transform;
        readonly RecordableCurve curveX;
        readonly RecordableCurve curveY;
        readonly RecordableCurve curveZ;

        readonly AnimationClip clip;
        readonly string relativePath;

        public RecordableEulerRotation(AnimationClip clip, string path, Transform target)
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
            curveX.Record(time, transform.localEulerAngles.x);
            curveY.Record(time, transform.localEulerAngles.y);
            curveZ.Record(time, transform.localEulerAngles.z);
        }

        public void ConnectSegmentedRotations()
        {
            var prevEuler = Vector3.zero;
            var offset = Vector3.zero;
            for (var i = 0; i < curveX.curve.length; i++)
            {
                var x = curveX.curve[i];
                var y = curveY.curve[i];
                var z = curveZ.curve[i];

                var euler = new Vector3(x.value, y.value, z.value);
                if (i == 0)
                {
                    prevEuler = euler;
                    continue;
                }

                const int bottom = 0 + 60;
                const int top = 360 - 60;

                if (prevEuler.x < bottom && euler.x > top)
                {
                    offset.x -= 360;
                }
                else if (prevEuler.x > top && euler.x < bottom)
                {
                    offset.x += 360;
                }

                if (prevEuler.y < bottom && euler.y > top)
                {
                    offset.y -= 360;
                }
                else if (prevEuler.y > top && euler.y < bottom)
                {
                    offset.y += 360;
                }

                if (prevEuler.z < bottom && euler.z > top)
                {
                    offset.z -= 360;
                }
                else if (prevEuler.z > top && euler.z < bottom)
                {
                    offset.z += 360;
                }

                var newEuler = euler + offset;
                x.value = newEuler.x;
                y.value = newEuler.y;
                z.value = newEuler.z;

                curveX.curve.MoveKey(i, x);
                curveY.curve.MoveKey(i, y);
                curveZ.curve.MoveKey(i, z);

                prevEuler = euler;
            }

            for (var i = 0; i < curveX.curve.length; i++)
            {
                curveX.curve.SmoothTangents(i, 1);
                curveY.curve.SmoothTangents(i, 1);
                curveZ.curve.SmoothTangents(i, 1);
            }
        }

        public void SetCurve()
        {
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "localEulerAngles.x" }, curveX.curve);
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "localEulerAngles.y" }, curveY.curve);
            AnimationUtility.SetEditorCurve(clip, new EditorCurveBinding() { path = relativePath, type = typeof(Transform), propertyName = "localEulerAngles.z" }, curveZ.curve);
        }
    }
}
#endif
