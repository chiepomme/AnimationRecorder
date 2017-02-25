using System.IO;
using UnityEngine;

namespace AnimationRecorder
{
    public class AutomaticRecorder : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        Transform[] recordingTransforms;
        [SerializeField]
        int fps = 60;
        [SerializeField]
        bool useEulerAngles = false;
        [SerializeField]
        string filePath = "Assets/hoge.anim";

        Recorder recorder;
        float startedAt;

        void Start()
        {
            recorder = new Recorder(transform, recordingTransforms, fps);
            startedAt = Time.time;
        }

        void Update()
        {
            recorder.RecordCurrentValue(Time.time - startedAt);
        }

        void OnApplicationQuit()
        {
            if (!Application.isPlaying) return;
            recorder.SaveAnimationClip(FindUniqueTakePath(filePath));
        }

        string FindUniqueTakePath(string filePath)
        {
            var i = 1;
            while (true)
            {
                var takePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + "(Take" + i + ").anim");
                if (!File.Exists(takePath)) return takePath;
                i += 1;
            }
        }
#endif
    }
}
