using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        buttonCheck = canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/", "CheckResult", new Vector3(), true)).GetComponentInChildren<Button>();
        var obj = canvasCtrl.InstantiateUI(canvasCtrl.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillAnswerSlotContainer", new Vector3(), true));
        //ListGeneric.Add(obj); //add to check drag and drop, always at index 3
        var listDropSlot = obj.GetComponentsInChildren<DropSlot>().ToList();

        for (int i = 0; i < labels.Count; i++)
        {
            var kw = canvasCtrl.InstantiateUI(listDropSlot[i].gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillKeyword", new Vector3(), true));
            kw.GetComponentInChildren<TMP_Text>().text = labels[i];
        }

        buttonCheck.onClick.AddListener(() => {
            StartCoroutine(WaitForDestroy(3f, obj));

            action(-1);


        });
    }

    private IEnumerator WaitForDestroy(float t, GameObject obj)
    {
        yield return new WaitForSecondsRealtime(t);
        Destroy(obj);
    }
}
