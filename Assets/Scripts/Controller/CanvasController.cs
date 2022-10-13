using Assets.Scripts.Model;
using Assets.Scripts.ObjectBehaviour;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        MissionManager missionMng;

        [SerializeField]
        Button paused;

        [SerializeField]
        GameObject resultTable;

        [SerializeField]
        GameObject collectionTableDisplay;

        [SerializeField]
        CollectionInfo info;

        public GameObject SubCanvasWorldPoint;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitToSeeDontDestroy());
        }
        
        private IEnumerator WaitToSeeDontDestroy()
        {
            while (gameController.SceneController.DontDestroy is null) yield return null;

            if (gameController.SceneController.Loading.activeInHierarchy)
            {
                gameController.SceneController.Loading.SetActive(false);
                Destroy(GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject);
            }
            var missionDatas = gameController.SceneController.DontDestroy.GetMission();
            missionMng.OpenMissionDialog(missionDatas);
            collectionTableDisplay.GetComponent<CollectionHandle>().CheckMission(missionDatas, info);
        }

        public GameObject InstantiateUI(ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(gameObject, args);

        public GameObject InstantiateUI(GameObject parent, ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(parent, args);

        public void ShowResultLevel(EndLevelState state)
        {

            SetState(GameState.EndLevelDisplay);

            var label = resultTable.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
            switch (state)
            {
                case EndLevelState.EndPoint:
                    label.text = "đến đích";
                    break;
                case EndLevelState.Dead:
                    label.text = "thất bại";
                    break;
                case EndLevelState.Exit:
                    label.text = "thất bại";
                    break;
            }

            resultTable.SetActive(true);
        }

        public void SetState(GameState type)
        {
            if (type != GameState.GameDisplay) paused.interactable = false;
            gameController.SetGameState(type);
        }

        public void PopState() 
        {
            gameController.PopGameState(); 
            if (gameController.IsEqualsTopDisplay(GameState.GameDisplay)) paused.interactable = true;
        }
    }
}