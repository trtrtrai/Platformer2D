using System.Collections.Generic;

[System.Serializable]
public class QuestionData
{
    public QuestionType Type { get; set; }
    public string Quest { get; set; }
    public List<string> Answers { get; set; }
    public List<int> CorrectIndex { get; set; }
}
