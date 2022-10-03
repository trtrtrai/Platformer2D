using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> missions;

        // Start is called before the first frame update
        void Start()
        {
            var script = missions[0].GetComponent<MissionData>();
            script.Type = MissionType.LevelCompleted;
            LoadText(script, missions[0].gameObject.GetComponentInChildren<TMP_Text>());

            script = missions[1].GetComponent<MissionData>();
            script.Type = MissionType.CompletionTime;
            script.SecondsChallenge = 187;
            LoadText(script, missions[1].gameObject.GetComponentInChildren<TMP_Text>());

            script = missions[2].GetComponent<MissionData>();
            script.Type = MissionType.FullCollection;
            //script.PointsChallenge = 3000;
            LoadText(script, missions[2].gameObject.GetComponentInChildren<TMP_Text>());

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LoadText(MissionData data, TMP_Text text)
        {
            Console.OutputEncoding = Encoding.UTF8;
            switch (data.Type)
            {
                case MissionType.LevelCompleted:
                    {
                        text.text = "Hoàn thành màn chơi.";
                        break;
                    }
                case MissionType.CompletionTime:
                    {
                        var minute = data.SecondsChallenge / 60;
                        var sec = data.SecondsChallenge % 60;
                        if (minute == 0) text.text = "Hoàn thành màn chơi trong " + sec + " giây.";
                        else if (sec == 0) text.text = "Hoàn thành màn chơi trong " + minute + " phút.";
                        else text.text = "Hoàn thành màn chơi trong " + minute + " phút " + sec + " giây.";
                        break;
                    }
                case MissionType.Point:
                    {
                        text.text = "Đạt được " + data.PointsChallenge + " điểm khi tổng kết.";
                        break;
                    }
                case MissionType.FullCollection:
                    {
                        text.text = "Thu thập 15 chuối 20 táo.";
                        break;
                    }
            }
        }
    }
}