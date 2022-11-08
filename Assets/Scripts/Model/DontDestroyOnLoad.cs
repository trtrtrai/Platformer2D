using Assets.Scripts.Controller;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public string ThisScene;
        public int PlayerIndex;
        public GameObject MissionObj;
        public CharacterName Name;
        public AudioSource MainBG;
        public AudioSource Click;
        public Configure Config;
        public List<ResolutionProp> Resolutions;
        public int IndexRes;
        public GameObject EventSystem;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_STANDALONE
            //Don't destroy on load script
            var objs = GameObject.FindGameObjectsWithTag("DontDestroy"); //only right when have 1 obj don't destroy

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }
#endif

            if (Config is null)
            {
#if UNITY_STANDALONE
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + $"/config.txt"))
                using (JsonReader jReader = new JsonTextReader(sReader))
                {
                    Config = serializer.Deserialize<Configure>(jReader);
                }
#endif
#if UNITY_ANDROID
                if (!File.Exists(Application.persistentDataPath + "/config.txt"))
                {
                    using (var stream = new FileStream(path: Application.persistentDataPath + "/config.txt", mode: FileMode.CreateNew, access: FileAccess.Write, share: FileShare.ReadWrite)) // write in new file at persistent
                    {
                        Encoding encoding = Encoding.UTF8;

                        using (var sWriter = new StreamWriter(stream, encoding))
                        using (JsonWriter jReader = new JsonTextWriter(sWriter))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(jReader, JsonConvert.DeserializeObject<Configure>("{\"Volumn\":1}"));
                        }
                    }
                }

                using (var sReader = new StreamReader(Application.persistentDataPath + "/config.txt"))
                using (JsonReader jReader = new JsonTextReader(sReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Config = serializer.Deserialize<Configure>(jReader);
                }
#endif

                OnVolumnChange(Config.Volumn);
            }
#if UNITY_STANDALONE
            DontDestroyOnLoad(gameObject);
#endif
        }

        public void SwapScene(string name)
        {
            var child = gameObject.GetComponentInChildren<RectTransform>();
            if (child != null) MissionObj = child.gameObject;

#if UNITY_STANDALONE
            if (!ThisScene.Equals("HomeScene") && !ThisScene.Equals("LevelSelectionScene") && (name.Equals("HomeScene") || name.Equals("LevelSelectionScene"))) MainBG.Play();
#endif
#if UNITY_ANDROID
            if (name.Equals("HomeScene"))
            {
                SceneManager.LoadScene(name);
                return;
            }

            if (!ThisScene.Equals("HomeScene") && !ThisScene.Equals("LevelSelectionScene") && (name.Equals("HomeScene") || name.Equals("LevelSelectionScene"))) MainBG.Play();
            StartCoroutine(LoadSceneAsync(name));
#endif
        }
#if UNITY_ANDROID
        private IEnumerator LoadSceneAsync(string name)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

            while (!asyncLoad.isDone) yield return null;

            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(name));
            SceneManager.MoveGameObjectToScene(EventSystem, SceneManager.GetSceneByName(name));
            var old = ThisScene;
            ThisScene = name;
            SceneManager.UnloadSceneAsync(old);
        }
#endif
        public void QuitGame()
        {
            Application.Quit();
        }

        public MissionData[] GetMission()
        {
            return MissionObj.transform.GetChild(1).GetComponentsInChildren<MissionData>();
        }

        public void OnVolumnChange(float v)
        {
            Config.Volumn = v;
            AudioListener.volume = v;
        }

        public void ChangeResolution()
        {
            if (IndexRes == 0)
            {
                Screen.fullScreen = true;
            }
            else Screen.SetResolution(Resolutions[IndexRes].Width, Resolutions[IndexRes].Height, false);
        }

        public void NextResolution(TMP_Text txt)
        {
            IndexRes++;
            if (IndexRes == Resolutions.Count) IndexRes = 0;

            DisplayResolution(txt);
        }

        public void PrevResolution(TMP_Text txt)
        {
            IndexRes--;
            if (IndexRes == -1) IndexRes = Resolutions.Count - 1;

            DisplayResolution(txt);
        }

        private void DisplayResolution(TMP_Text txt)
        {
            if (IndexRes == 0) txt.text = "Full Screen";
            else txt.text = $"{Resolutions[IndexRes].Width} x {Resolutions[IndexRes].Height}";
        }

        private void OnApplicationQuit()
        {
            PlayerData.SaveBeforeExit();

#if UNITY_STANDALONE
            var serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + $"/Config.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, Config);
            }
#endif

#if UNITY_ANDROID
            using (var stream = new FileStream(path: Application.persistentDataPath + "/config.txt", mode: FileMode.Open, access: FileAccess.Write, share: FileShare.ReadWrite)) // write in new file at persistent
            {
                Encoding encoding = Encoding.UTF8;

                using (var sWriter = new StreamWriter(stream, encoding))
                using (JsonWriter jReader = new JsonTextWriter(sWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jReader, Config);
                }
            }
#endif
        }
    }

    public class Configure
    {
        public float Volumn;
    }

    [Serializable]
    public class ResolutionProp
    {
        public int Width, Height;
    }
}