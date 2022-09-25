using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using static Assets.Scripts.Others.CustomRandom;
using Assets.Scripts.Model;
using System;
using Assets.Scripts.Interfaces;
using System.Collections;

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
        private Vector3 playerPosition;

        public event SendResultToCaller Sender;
        public Timer Timer;
        public Color LabelTrue;
        public Color LabelFalse;
        public float TimeAnswer;
        // Start is called before the first frame update
        void Start()
        {
            parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();            
            question = new Question(ChooseAQuestion());

            if (question.Type == QuestionType.TwoChoice)
            {
                parent.SetState(GameState.TwoChoiceQuestionDisplay);                
                var player = GameObject.FindGameObjectWithTag("Player");
                playerPosition = player.transform.localPosition;
                //Debug.Log(playerPosition);
                player.transform.localPosition = new Vector3(0, .2f);
                Sender += QuestionManager_Sender;
                gameObject.GetComponentInChildren<Image>().gameObject.SetActive(false); //Get children to hide because TwoChoice don't need it
            }
            else
            {
                Time.timeScale = 0;
                parent.SetState(GameState.QuestionDisplay);
            }

            Timer = gameObject.GetComponent<Timer>();
            Timer.TimeOutAsyncEvent += TimeOutAsyncHandle;

            LoadQuestionScene();
            //Debug.Log(question.Quest + " " + question.Type);
        }

        private void QuestionManager_Sender(bool result)
        {
            StartCoroutine(WaitToReturnPlayer(3f));
        }

        private IEnumerator WaitToReturnPlayer(float s)
        {
            yield return new WaitForSecondsRealtime(s);

            Destroy(AnswerContainer);
            GameObject.FindGameObjectWithTag("Player").transform.localPosition = playerPosition;
            Destroy(gameObject);
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
                return questions[6];
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
                case QuestionType.Fill:
                    {
                        AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillAnswer", new Vector3(), true));
                        IQuest<GameObject> script = AnswerContainer.GetComponent<Fill>();

                        script.Render(answers, new Action<int>((t) => {
                            var checkList = new List<int>();
                            for (int i = 0; i < script.ListGeneric.Count; i++)
                            {
                                var item = script.ListGeneric[i].GetComponentInChildren<DropItem>();
                                if (!(item is null)) checkList.Add(item.Index);
                            }
                            var result = question.CheckingResult(checkList);

                            isChosen = true;
                            Sender?.Invoke(result);

                            script.AfterCheck(3, result, false);
                        }));

                        break;
                    }
                case QuestionType.TwoChoice:
                    {
                        AnswerContainer = parent.InstantiateUI(gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/TwoChoice/", "TwoChoiceQuestion", Vector3.zero));
                        TwoChoice script = AnswerContainer.GetComponent<TwoChoice>();

                        script.QuestField.GetComponent<TMP_Text>().text = question.Quest;

                        script.Render(answers, new Action<int>((t) =>
                        {
                            var result = question.CheckingResult(new List<int>() { t});

                            isChosen = true;
                            Sender?.Invoke(result);

                            script.AfterCheck(t, result, false);  
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