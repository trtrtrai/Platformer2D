using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour, IQuest<GameObject>
{
    [SerializeField]
    private List<GameObject> fillSlots;

    public List<GameObject> ListGeneric { get => fillSlots; set => fillSlots = value; }

    private QuestionManager questionManager;
    private Button buttonCheck;

    private void Awake()
    {
        questionManager = gameObject.GetComponentInParent<QuestionManager>();
    }

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

        buttonCheck = canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/", "CheckResult", new Vector3(), true)).GetComponent<Button>();
        var obj = canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "GrayBGRect", new Vector3(), true)).GetComponentInChildren<DropSlot>().gameObject;
        ListGeneric.Add(obj); //add to check drag and drop, always at index 3

        labels.ForEach((l) => {
            var kw = canvasCtrl.InstantiateUI(obj, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillKeyword", new Vector3(), true));
            // random position
            kw.GetComponentInChildren<TMP_Text>().text = l;
        });

        
    }
}
