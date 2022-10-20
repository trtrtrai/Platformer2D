using Assets.Scripts.Controller;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Model
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public string ThisScene;
        public GameObject MissionObj;
        public CharacterName Name;
        public AudioSource MainBG;

        // Start is called before the first frame update
        void Start()
        {
            //Don't destroy on load script
            var objs = GameObject.FindGameObjectsWithTag("DontDestroy"); //only right when have 1 obj don't destroy

            if (objs.Length > 1)
            {
                Destroy(gameObject);
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

        private void OnApplicationQuit()
        {
            PlayerData.SaveBeforeExit();
        }
    }
}