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
    private List<CanvasGroup> canvasGroups;

    private void Awake()
    {
        questionManager = gameObject.GetComponentInParent<QuestionManager>();
    }

    public void AfterCheck(int i, bool result, bool loop = true)
    {
        if (result) for (int j = 0; j < i; j++) ListGeneric[j].gameObject.GetComponent<Image>().color = questionManager.LabelTrue;
        else for (int j = 0; j < i; j++) ListGeneric[j].gameObject.GetComponent<Image>().color = questionManager.LabelFalse;
    }

    public int ListCheckedCount()
    {
        return -1;
    }

    public void Render(List<string> labels, Action<int> action)
    {
        var canvasCtrl = gameObject.GetComponentInParent<CanvasController>();

        // create question UI
        buttonCheck = canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/", "CheckResult", new Vector3(), true)).GetComponentInChildren<Button>();
        var obj = canvasCtrl.InstantiateUI(canvasCtrl.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillAnswerSlotContainer", new Vector3(), true));
        var listDropSlot = obj.GetComponentsInChildren<DropSlot>().ToList();

        // create keyword
        canvasGroups = new List<CanvasGroup>();
        for (int i = 0; i < labels.Count; i++)
        {
            var kw = canvasCtrl.InstantiateUI(listDropSlot[i].gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/Fill/", "FillKeyword", new Vector3(), true));
            kw.GetComponentInChildren<TMP_Text>().text = labels[i];

            canvasGroups.Add(kw.GetComponent<CanvasGroup>());
        }

        // Set up time out
        questionManager.Timer.TimeOutEvent += Timer_TimeOutEvent;

        // add listener
        buttonCheck.onClick.AddListener(() => {
            StartCoroutine(WaitForDestroy(3f, obj));
            canvasGroups.ForEach((c) => c.blocksRaycasts = false);

            action(-1);

            buttonCheck.interactable = false;
            buttonCheck.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.75f); // set up to scale with Img on screen
            buttonCheck.onClick.RemoveAllListeners();
        });
    }

    private void Timer_TimeOutEvent()
    {
        buttonCheck.onClick.Invoke();

        questionManager.Timer.TimeOutEvent -= Timer_TimeOutEvent;
    }

    private IEnumerator WaitForDestroy(float t, GameObject obj)
    {
        yield return new WaitForSecondsRealtime(t);
        Destroy(obj);
    }
}
