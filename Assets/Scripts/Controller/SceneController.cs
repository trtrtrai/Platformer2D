using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
            Loading.SetActive(true);
            StartCoroutine(WaitToSeeDontDestroy());
        }

        private IEnumerator WaitToSeeDontDestroy()
        {
            while (GameObject.FindGameObjectWithTag("DontDestroy") is null) yield return null;

            DontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoad>();

#if UNITY_STANDALONE
            if (!DontDestroy.ThisScene.Equals("HomeScene"))
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/GameScene/EventSystem"));
            }
#endif

            sceneBtns = new List<Button>();

            Resources.FindObjectsOfTypeAll(typeof(Button)).ToList().ForEach((b) => {
                //if (b is null) Debug.Log("null"); else Debug.Log(b.name); 
                sceneBtns.Add(b as Button);
            });

            sceneBtns.ForEach((b) => { if (!b.tag.Equals("ButtonPlayer")) b.onClick.AddListener(() => DontDestroy.Click.Play()); });

            if (DontDestroy.ThisScene.Equals("HomeScene") || DontDestroy.ThisScene.Equals("LevelSelectionScene"))
            {
                Loading.SetActive(false);
            }
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

        public void OnVolumnChange(float v) => DontDestroy.OnVolumnChange(v);

        public void UpdateVolumnSlider(Slider slider)
        {
            slider.value = DontDestroy.Config.Volumn;
        }

        public void ChangeResolution() => DontDestroy.ChangeResolution();

        public void NextResolution(TMP_Text txt) => DontDestroy.NextResolution(txt);

        public void PrevResolution(TMP_Text txt) => DontDestroy.PrevResolution(txt);

        public void SaveConfig() => DontDestroy.SaveConfig();

        public void SaveBeforeExit() => PlayerData.SaveBeforeExit();

        public void QuitGame() => DontDestroy.QuitGame();
    }
}