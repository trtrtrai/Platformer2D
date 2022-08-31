using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

[RequireComponent(typeof(Timer))]
public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    GameObject QuestContainer;

    private GameObject AnswerContainer;
    private CanvasController parent;
    private Question question;
    private List<Button> buttons;
    private List<int> exceptedBtns;
    private Timer timer;

    public SendResultToCaller Sender;
    public float TimeAnswer;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();
        parent.SetState(GameState.QuestionDisplay);
        question = new Question(ChooseAQuestion());
        timer = gameObject.GetComponent<Timer>();

        LoadQuestionScene();
        //Debug.Log(question.Quest + " " + question.Type);
    }

    private void Update()
    {
        TimeAnswer -= Time.deltaTime;
        timer.UpdateTimer(TimeAnswer);
    }

    private QuestionData ChooseAQuestion()
    {
        JsonSerializer serializer = new JsonSerializer();
        using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + "/Questions.txt"))
        using (JsonReader jReader = new JsonTextReader(sReader))
        {
            var questions = serializer.Deserialize<List<QuestionData>>(jReader);
            int num = Random.Range(0, questions.Count - 1);
            //Debug.Log(num);
            return questions[num];
        }       
    }

    private void LoadQuestionScene()
    {
        switch (question.Type)
        {
            case QuestionType.OneTrue:
                var tmp = QuestContainer.GetComponentInChildren<TMP_Text>();
                tmp.text = question.Quest;

                AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/", "OneTrueAnswer", new Vector3(), QuestContainer.transform));
                buttons = AnswerContainer.GetComponentsInChildren<Button>().ToList();
                exceptedBtns = new List<int>();

                var answers = question.GetAnswers();
                for (int i=0;i<buttons.Count;i++)
                {
                    var t = i;
                    buttons[t].GetComponentInChildren<TMP_Text>().text = answers[t];
                    buttons[t].onClick.AddListener(() => 
                    {
                        var result = question.CheckingResult(t);

                        if (exceptedBtns.Count == 0)
                        {
                            Sender?.Invoke(result);

                            if (result) buttons[t].gameObject.GetComponent<Image>().color = new Color(0, 1, 0);
                            else
                            {
                                buttons[t].gameObject.GetComponent<Image>().color = new Color(1, 59 / 255, 59 / 255);
                                
                            }
                        }
                        else if (result) buttons[t].gameObject.GetComponent<Image>().color = new Color(0, 1, 0);
                        else FindRightButton(t);

                        buttons[t].onClick.RemoveAllListeners();
                    });
                }
                break;
        }
    }

    private void FindRightButton(int except)
    {
        exceptedBtns.Add(except);

        for (int i = 0; i < buttons.Count; i++)
        {
            var isInvoke = true;
            for (int j = 0; j < exceptedBtns.Count; j++)
            {
                if (i == exceptedBtns[j])
                {
                    isInvoke = false;
                    break;
                }
            }

            if (isInvoke) buttons[i].onClick.Invoke();
        }
    }

    private void OnDestroy()
    {
        
    }

    public delegate void SendResultToCaller(bool result);
}
