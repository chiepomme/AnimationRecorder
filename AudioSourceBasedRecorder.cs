using System.Collections;
using System.IO;
using UnityEngine;

namespace AnimationRecorder
{
    public class AudioSourceBasedRecorder : MonoBehaviour
    {
        [SerializeField]
        float playingWaitSec = 3;
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        Transform[] recordingTransforms;
        [SerializeField]
        int fps = 60;
        [SerializeField]
        string filePath = "Assets/hoge.anim";

        Recorder recorder;
        bool needsSave;

        void Awake()
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.Stop();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(playingWaitSec);
            recorder = new Recorder(transform, recordingTransforms, fps);
            audioSource.Play();
            needsSave = true;

            yield return new WaitWhile(() => audioSource.isPlaying);
            if (needsSave) Save();
        }

        void Save()
        {
            recorder.SaveAnimationClip(FindUniqueTakePath(filePath));
            needsSave = false;
        }

        void Update()
        {
            if (!audioSource.isPlaying) return;
            recorder.RecordCurrentValue(audioSource.time);
        }

        void OnApplicationQuit()
        {
            if (!Application.isPlaying) return;
            if (needsSave) Save();
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
    }
}
