using UnityEngine;
using System.Collections.Generic;


public class Question
{
    private QuestionData rawData;

    public QuestionType Type => rawData.Type;
    public string Quest => rawData.Quest;
    public event CorrectChoice QuestionResult;

    public Question(QuestionData data)
    {
        rawData = new QuestionData();
        rawData.Type = data.Type;
        rawData.Quest = data.Quest;
        rawData.Answers = new List<string>(data.Answers);
        rawData.CorrectIndex = data.CorrectIndex;

        QuestionResult += isTrue;
        /*Swap(0, 1);
        Debug.Log(rawData.Answers[0] + " old:" + data.Answers[0]);
        Debug.Log(rawData.Answers[1] + " old:" + data.Answers[1]);*/
    }

    private bool isTrue(int i)
    {
        //Debug.Log($"{i} - {i == rawData.CorrectIndex}");
        return i == rawData.CorrectIndex;
    }

    public List<string> GetAnswers()
    {
        MixAnswers();
        return rawData.Answers;
    }

    private void MixAnswers()
    {
        switch (Type)
        {
            case QuestionType.OneTrue:
                int i = Random.Range(1, 6); // loop count 1 to 5
                for (; i > 0; i--)
                {
                    int index1 = Random.Range(0, rawData.Answers.Count); // Index will be swap 0 to Count-1
                    int index2 = Random.Range(0, rawData.Answers.Count); // Index will be swap
                    if (index1 == rawData.CorrectIndex) rawData.CorrectIndex = index2; // Change CorrectIndex
                    else if (index2 == rawData.CorrectIndex) rawData.CorrectIndex = index1; // Change CorrectIndex
                    //Debug.Log($"Swap {index1} and  {index2} - {rawData.CorrectIndex}");
                    Swap(index1, index2);
                }
                break;
        }
    }

    private void Swap(int i1, int i2)
    {
        string c = rawData.Answers[i1];
        rawData.Answers[i1] = rawData.Answers[i2];
        rawData.Answers[i2] = c;
    }

    public void UnSubQuestionResult() => QuestionResult -= isTrue;

    public bool CheckingResult(int choice) => QuestionResult.Invoke(choice);

    public delegate bool CorrectChoice(int choice);
}
