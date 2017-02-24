using UnityEngine;

namespace AnimationRecorder
{
    public class RecordableTransform
    {
        readonly RecordablePosition position;
        readonly RecordableRotation rotation;

        public RecordableTransform(AnimationClip clip, Transform root, Transform target)
        {
            var relativePath = BuildRelativePath(root, target);
            position = new RecordablePosition(clip, relativePath, target);
            rotation = new RecordableRotation(clip, relativePath, target);
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
