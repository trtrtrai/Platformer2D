using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        List<AudioSource> audioSources;

        [SerializeField]
        List<AudioClip> audioClips;

        private List<string> audioNames;

        // Start is called before the first frame update
        void Start()
        {
            SoundPackage.Controller = gameObject.GetComponent<SoundController>();
            StartCoroutine(WaitToGetDontDestroy());

            audioNames = new List<string>();
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioNames.Add(audioSources[i].name);
            }
        }

        private IEnumerator WaitToGetDontDestroy()
        {
            while (gameController.SceneController.DontDestroy is null) yield return null;

            gameController.SceneController.DontDestroy.MainBG.Stop();
        }

        public void PlayAudio(string name)
        {
            audioSources[audioNames.IndexOf(name)].Play();
        }

        public void StopAudio(string name)
        {
            audioSources[audioNames.IndexOf(name)].Stop();
        }
    }

    public static class SoundPackage
    {
        public static SoundController Controller;
    }
}