#if UNITY_EDITOR
using UnityEngine;

namespace AnimationRecorder
{
    public class RecordableTransform
    {
        readonly RecordablePosition position;
        readonly IRecordableRotation rotation;

        public RecordableTransform(AnimationClip clip, Transform root, Transform target, bool useEulerAngles = false)
        {
            var relativePath = BuildRelativePath(root, target);
            position = new RecordablePosition(clip, relativePath, target);

            if (useEulerAngles)
            {
                rotation = new RecordableEulerRotation(clip, relativePath, target);
            }
            else
            {
                rotation = new RecordableQuaternionRotation(clip, relativePath, target);
            }
        }

        public void RecordCurrentValue(float time)
        {
            position.RecordCurrentValue(time);
            rotation.RecordCurrentValue(time);
        }

        public void SetCurveToClip()
        {
            position.SetCurve();
            rotation.SetCurve();
        }

        public void ConnectSegmentedRotations()
        {
            rotation.ConnectSegmentedRotations();
        }

        string BuildRelativePath(Transform root, Transform target)
        {
            var path = "";
            var current = target;
            while (true)
            {
                if (current == null) throw new System.Exception(target.name + "は" + root.name + "の子供ではありません");
                if (current == root) break;

                path = (path == "") ? current.name : current.name + "/" + path;

                current = current.parent;
            }

            return path;
        }
    }
}
#endif
