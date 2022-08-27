using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    GameObject QuestContainer;

    [SerializeField]
    GameObject AnswerContainer;

    private CanvasController parent;
    private Question question;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();
        parent.SetState(GameState.QuestionDisplay);
        question = ChooseAQuestion();
        //Debug.Log(question.Quest + " " + question.CorrectIndex);
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        
    }
}
