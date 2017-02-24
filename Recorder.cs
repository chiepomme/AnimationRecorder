using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnimationRecorder
{
    public class Recorder
    {
        readonly AnimationClip clip;
        readonly RecordableTransform[] recordableTransforms;

        public bool IsRecording { get; private set; }

        public Recorder(Transform rootTransform, Transform[] recordingTransforms, int fps = 60)
        {
            clip = new AnimationClip { frameRate = fps, };
            AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings() { loopTime = false, });

            recordableTransforms = recordingTransforms.Select(recordingTransform => new RecordableTransform(clip, rootTransform, recordingTransform)).ToArray();
        }

        public void RecordCurrentValue(float time)
        {
            foreach (var recordableTransform in recordableTransforms)
            {
                recordableTransform.RecordCurrentValue(time);
            }
        }

        public void SaveAnimationClip(string path)
        {
            if (clip == null) throw new Exception("まだ何も録画されていません");

            foreach (var recordableTransform in recordableTransforms)
            {
                recordableTransform.SetCurveToClip();
            }

            clip.EnsureQuaternionContinuity();
            AssetDatabase.CreateAsset(clip, path);
        }
    }
}
