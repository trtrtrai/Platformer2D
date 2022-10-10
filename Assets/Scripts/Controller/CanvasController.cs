using Assets.Scripts.Model;
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

        private List<TMP_Text> collected;

        public GameObject SubCanvasWorldPoint;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitToSeeDontDestroy());
        }
        
        private IEnumerator WaitToSeeDontDestroy()
        {
            while (gameController.SceneController.DontDestroy is null) yield return null;

            var missionDatas = gameController.SceneController.DontDestroy.GetMission();
            missionMng.OpenMissionDialog(missionDatas);
            CheckMission(missionDatas);
        }

        private void CheckMission(MissionData[] datas)
        {
            foreach (var data in datas)
            {
                if (data.Type == MissionType.FullCollection)
                {
                    collected = new List<TMP_Text>();
                    var imgs = collectionTableDisplay.GetComponentsInChildren<Image>();
                    /*var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PointCounter>();
                    player.PointListener += Player_PointListener;*/

                    for (int i = 0; i < imgs.Length; i++)
                    {
                        collected.Add(imgs[i].GetComponentInChildren<TMP_Text>());
                        if (i < data.NumberOfCollection)
                        {
                            Debug.Log("Set image " + data.Names[i]);
                            Debug.Log("Set amount " + data.Amount[i]);
                            imgs[i].sprite = info.GetSprite(data.Names[i]);
                            collected[i].text = data.Amount[i] + " left";
                        }
                        else imgs[i].gameObject.SetActive(false);
                    }

                    return;
                }
            }

            collectionTableDisplay.SetActive(false);
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