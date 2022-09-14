using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
using static CustomRandom;
using static UnityEngine.UI.Toggle;

[RequireComponent(typeof(Timer))]
public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    GameObject QuestContainer;

    private GameObject AnswerContainer;
    private CanvasController parent;
    private Question question;
    private Timer timer;
    private bool isChosen = false;

    public event SendResultToCaller Sender;
    public float TimeAnswer;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();
        parent.SetState(GameState.QuestionDisplay);
        question = new Question(ChooseAQuestion());
        timer = gameObject.GetComponent<Timer>();
        timer.TimeOutAsyncEvent += TimeOutAsyncHandle;

        LoadQuestionScene();
        //Debug.Log(question.Quest + " " + question.Type);
    }

    private void Update()
    {
        if (TimeAnswer > 0 && !isChosen)
        {
            TimeAnswer -= Time.unscaledDeltaTime;
            timer.UpdateTimer(TimeAnswer);
        }
    }

    private async Task TimeOutAsyncHandle()
    {
        // Disable buttons (OneTrue only???)
        //buttons.ForEach((b) => b.enabled = false);
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
                AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/UI/Question/OneTrue/", "OneTrueAnswer", new Vector3(), QuestContainer.transform));
                var script = AnswerContainer.GetComponent<OneTrue>();
                var buttons = script.Buttons;
             
                for (int i=0;i<buttons.Count;i++)
                {
                    var t = i;
                    buttons[t].GetComponentInChildren<TMP_Text>().text = answers[t];
                    buttons[t].onClick.AddListener(() => 
                    {
                        var result = question.CheckingResult(new List<int>() { t});

                        if (script.ButtonClicked() == 0)
                        {
                            isChosen = true;
                            Sender?.Invoke(result);                      
                        }
                        script.AfterCheck(t, result, !result);

                        buttons[t].onClick.RemoveAllListeners();
                    });
                }
                break;
            case QuestionType.MultipleTrue:
                AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "MultipleTrueAnswer", new Vector3(), true));
                parent.InstantiateUI(gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "ToggleFalseGuide", new Vector3(), true));
                parent.InstantiateUI(gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "ToggleTrueGuide", new Vector3(), true));
                var content = AnswerContainer.GetComponent<MultipleTrue>().Content;

                answers.ForEach((a) => {
                    var obj = parent.InstantiateUI(content, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "MultipleAnswer", new Vector3(), true));

                    var label = obj.GetComponentInChildren<TMP_Text>();
                    label.text = a;

                    obj.GetComponent<Toggle>().onValueChanged.AddListener((t) => {
                        if (t) label.color = new Color(0, 129/255, 1);
                        else label.color = new Color(0, 0, 0);
                    });
                    obj.GetComponent<Toggle>().isOn = false;
                });

                var btnCheck = parent.InstantiateUI(gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "CheckMultipleTrue", new Vector3(), true)).GetComponentInChildren<Button>();
                btnCheck.onClick.AddListener(() =>{
                    content.GetComponentsInChildren<Toggle>().ToList().ForEach((t) => { 
                        t.interactable = false; 
                        t.onValueChanged.RemoveAllListeners();
                    });

                    //...

                    btnCheck.interactable = false;
                    btnCheck.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.75f);
                    // can decor linear color for TextMeshPro...
                    btnCheck.onClick.RemoveAllListeners();
                });
                break;
        }
    }

    private void OnDestroy()
    {
        parent.PopState();
        timer.TimeOutAsyncEvent -= TimeOutAsyncHandle;
        question.UnSubQuestionResult();
    }

    public void UnSubSender(SendResultToCaller action) => Sender -= action;

    public delegate void SendResultToCaller(bool result);
}
