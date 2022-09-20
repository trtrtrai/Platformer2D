using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fill : MonoBehaviour, IQuest<GameObject>
{
    [SerializeField]
    private List<GameObject> fillSlots;

    public List<GameObject> ListGeneric { get => fillSlots; set => fillSlots = value; }

    public void AfterCheck(int i, bool result, bool loop = true)
    {
        
    }

    public int ListCheckedCount()
    {
        return -1;
    }

    public void Render(List<string> labels, Action<int> action)
    {
        var canvasCtrl = gameObject.GetComponentInParent<CanvasController>();
        labels.ForEach((l) => {
            var kw = canvasCtrl.InstantiateUI(fillSlots[3], new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillKeyword", new Vector3(), true));
            // random position
            kw.GetComponentInChildren<TMP_Text>().text = l;
        });

        canvasCtrl.InstantiateUI(GetComponentInParent<QuestionManager>().gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/", "CheckResult", new Vector3(), true));
    }
}
