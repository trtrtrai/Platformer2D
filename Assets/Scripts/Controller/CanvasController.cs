using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        MissionManager missionMng;

        public GameObject SubCanvasWorldPoint;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitToSeeDontDestroy());
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        private IEnumerator WaitToSeeDontDestroy()
        {
            while (gameController.SceneController.DontDestroy is null) yield return null;

            missionMng.OpenMissionDialog(gameController.SceneController.DontDestroy.GetMission());
        }

        public GameObject InstantiateUI(ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(gameObject, args);

        public GameObject InstantiateUI(GameObject parent, ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(parent, args);

        public void SetState(GameState type) => gameController.SetGameState(type);

        public void PopState() => gameController.PopGameState();
    }
}