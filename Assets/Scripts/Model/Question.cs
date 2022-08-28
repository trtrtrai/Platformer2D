using System.Collections.Generic;

[System.Serializable]
public class Question
{
    public QuestionType Type { get; set; }
    public string Quest { get; set; }
    public List<string> Answers { get; set; }
    public int CorrectIndex { get; set; }
}
