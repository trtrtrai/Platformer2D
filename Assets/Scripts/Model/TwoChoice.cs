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
        for (int i = 0; i < labels.Count; i++)
        {
            answersField[i].GetComponent<TMP_Text>().text = labels[i];
        }
    }
}
