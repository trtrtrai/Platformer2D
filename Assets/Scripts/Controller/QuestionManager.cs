using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
using static Assets.Scripts.Others.CustomRandom;
using Assets.Scripts.Model;
using System;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controller
{
    [RequireComponent(typeof(Timer))]
    public class QuestionManager : MonoBehaviour
    {
        [SerializeField]
        GameObject QuestContainer;

        private GameObject AnswerContainer;
        private CanvasController parent;
        private Question question;
        private bool isChosen = false;

        public event SendResultToCaller Sender;
        public Timer Timer;
        public Color LabelTrue;
        public Color LabelFalse;
        public float TimeAnswer;
        // Start is called before the first frame update
        void Start()
        {
            parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();
            parent.SetState(GameState.QuestionDisplay);
            question = new Question(ChooseAQuestion());
            Timer = gameObject.GetComponent<Timer>();
            Timer.TimeOutAsyncEvent += TimeOutAsyncHandle;

            LoadQuestionScene();
            //Debug.Log(question.Quest + " " + question.Type);
        }

        private void Update()
        {
            if (TimeAnswer > 0 && !isChosen)
            {
                TimeAnswer -= Time.unscaledDeltaTime;
                Timer.UpdateTimer(TimeAnswer);
            }
        }

        private async Task TimeOutAsyncHandle()
        {
            // Message box notify
            var obj = parent.InstantiateUI(new ResourcesLoadEventHandler("Prefabs/UI/Notification/", "NotificationUI", new Vector3(), true));
            Sender?.Invoke(false);
            await Task.Delay(3000);
            Destroy(obj);
        }

        private QuestionData ChooseAQuestion()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + "/Questions.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                var questions = serializer.Deserialize<List<QuestionData>>(jReader);
                int num = RdPositiveRange(questions.Count);
                //Debug.Log(num);
                return questions[num];
            }
        }

        private void LoadQuestionScene()
        {
            var tmp = QuestContainer.GetComponentInChildren<TMP_Text>();
            tmp.text = question.Quest;
            var answers = question.GetAnswers();

            switch (question.Type)
            {
                case QuestionType.OneTrue:
                    {
                        AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/UI/Question/OneTrue/", "OneTrueAnswer", new Vector3(), QuestContainer.transform));
                        IQuest<Button> script = AnswerContainer.GetComponent<OneTrue>();

                        script.Render(answers, new Action<int>((t) => {
                            var result = question.CheckingResult(new List<int>() { t });

                            if (script.ListCheckedCount() == 0)
                            {
                                isChosen = true;
                                Sender?.Invoke(result);
                            }
                            script.AfterCheck(t, result, !result);
                        }));
                        break;
                    }
                case QuestionType.MultipleTrue:
                    {
                        AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "MultipleTrueAnswer", new Vector3(), true));
                        IQuest<Toggle> script = AnswerContainer.GetComponent<MultipleTrue>();

                        script.Render(answers, new Action<int>((t) =>{
                            var checkList = new List<int>();
                            for (int i = 0; i < script.ListGeneric.Count; i++) if (script.ListGeneric[i].isOn) checkList.Add(i);
                            var result = question.CheckingResult(checkList);

                            isChosen = true;
                            Sender?.Invoke(result);
                        }));

                        break;
                    }
            }
        }

        private void OnDestroy()
        {
            parent.PopState();
            Timer.TimeOutAsyncEvent -= TimeOutAsyncHandle;
            question.UnSubQuestionResult();
        }

        public void UnSubSender(SendResultToCaller action) => Sender -= action;

        public delegate void SendResultToCaller(bool result);
    }
}