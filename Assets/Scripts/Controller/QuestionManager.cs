using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    GameObject QuestContainer;

    private GameObject AnswerContainer;
    private CanvasController parent;
    private Question question;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();
        parent.SetState(GameState.QuestionDisplay);
        question = ChooseAQuestion();
        LoadQuestionScene();
        //Debug.Log(question.Quest + " " + question.Type);
    }

    private Question ChooseAQuestion()
    {
        JsonSerializer serializer = new JsonSerializer();
        using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + "/Questions.txt"))
        using (JsonReader jReader = new JsonTextReader(sReader))
        {
            var questions = serializer.Deserialize<List<Question>>(jReader);
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
                AnswerContainer = parent.InstantiateUI(QuestContainer, new ResourcesLoadEventHandler("Prefabs/", "OneTrueAnswer", new Vector3(), QuestContainer.transform));
                var tmp = QuestContainer.GetComponentInChildren<TMP_Text>();
                tmp.text = question.Quest;
                List<Button> buttons = AnswerContainer.GetComponentsInChildren<Button>().ToList();
                for (int i=0;i<buttons.Count;i++)
                {
                    buttons[i].GetComponentInChildren<TMP_Text>().text = question.Answers[i];
                }
                break;
        }
    }

    private void OnDestroy()
    {
        
    }
}
