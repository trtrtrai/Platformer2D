using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class SceneController : MonoBehaviour
    {
        public DontDestroyOnLoad DontDestroy;
        // Start is called before the first frame update
        void Start()
        {
            DontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoad>();
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

        public void LoadPlayerData()
        {
            PlayerData.InitiatePlayer(1);
        }

        public void QuitGame() => DontDestroy.QuitGame();
    }
}