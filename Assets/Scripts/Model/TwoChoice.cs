using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwoChoice : MonoBehaviour, IQuest<GameObject>
{
    [SerializeField]
    private List<GameObject> choiceFields;

    [SerializeField]
    List<GameObject> answersField;

    private Action<int> action;

    public GameObject QuestField;
    public List<GameObject> ListGeneric { get => choiceFields; set => choiceFields = value; }

    public void AfterCheck(int i, bool result, bool loop = true)
    {
        
    }

    public int ListCheckedCount()
    {
        return -1;
    }

    public void Render(List<string> labels, Action<int> action)
    {
        this.action = action;

        for (int i = 0; i < labels.Count; i++)
        {
            int t = i;
            answersField[t].GetComponent<TMP_Text>().text = labels[t];

            choiceFields[t].GetComponent<TwoChoiceAnswerBehaviour>().TriggerPublisher += TwoChoice_TriggerPublisher;
        }
    }

    private void TwoChoice_TriggerPublisher(string name)
    {
        if (name.Equals(choiceFields[0].name)) action(0);
        else if (name.Equals(choiceFields[1].name)) action(1);

        for (int i = 0; i < choiceFields.Count; i++)
        {
            choiceFields[i].GetComponent<TwoChoiceAnswerBehaviour>().TriggerPublisher -= TwoChoice_TriggerPublisher;
        }
    }
}
