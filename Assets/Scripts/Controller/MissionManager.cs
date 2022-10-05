using Assets.Scripts.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField]
        List<TMP_Text> missionsText;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void OpenMissionDialog(MissionData[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                LoadText(datas[i], missionsText[i]);
            }
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