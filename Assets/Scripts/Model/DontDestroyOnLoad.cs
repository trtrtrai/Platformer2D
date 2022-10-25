using Assets.Scripts.Controller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public string ThisScene;
        public GameObject MissionObj;
        public CharacterName Name;
        public AudioSource MainBG;
        public AudioSource Click;
        public Configure Config;
        public List<ResolutionProp> Resolutions;
        public int IndexRes;

        // Start is called before the first frame update
        void Start()
        {
            //Don't destroy on load script
            var objs = GameObject.FindGameObjectsWithTag("DontDestroy"); //only right when have 1 obj don't destroy

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }

            if (Config is null)
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + $"/config.txt"))
                using (JsonReader jReader = new JsonTextReader(sReader))
                {
                    Config = serializer.Deserialize<Configure>(jReader);
                }

                OnVolumnChange(Config.Volumn);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void SwapScene(string name)
        {
            var child = gameObject.GetComponentInChildren<RectTransform>();
            if (child != null) MissionObj = child.gameObject;

            if (!ThisScene.Equals("HomeScene") && !ThisScene.Equals("LevelSelectionScene") && (name.Equals("HomeScene") || name.Equals("LevelSelectionScene"))) MainBG.Play();
            ThisScene = name;
            SceneManager.LoadScene(name);
        }

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

            var serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + $"/Config.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, Config);
            }
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