using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class SceneController : MonoBehaviour
    {
        public DontDestroyOnLoad DontDestroy;

        public GameObject Loading;

        private List<Button> sceneBtns;

        // Start is called before the first frame update
        void Start()
        {
            DontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoad>();

            sceneBtns = new List<Button>();

            Resources.FindObjectsOfTypeAll(typeof(Button)).ToList().ForEach((b) => {
                //if (b is null) Debug.Log("null"); else Debug.Log(b.name); 
                sceneBtns.Add(b as Button);
            });

            sceneBtns.ForEach((b) => { b.onClick.AddListener(() => DontDestroy.Click.Play()); });
        }

        public void SwapScene(string name) => DontDestroy.SwapScene(name);

        public void SwapSceneWhilePlay()
        {
            Destroy(DontDestroy.MissionObj);
            SwapScene("LevelSelectionScene");
        }

        public void ReloadScene()
        {
            SwapScene(DontDestroy.ThisScene);
        }

        public void SaveBeforeExit() => PlayerData.SaveBeforeExit();

        public void QuitGame() => DontDestroy.QuitGame();
    }
}