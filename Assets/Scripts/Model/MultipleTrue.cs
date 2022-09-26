using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
using Assets.Scripts.ObjectBehaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
    public class MultipleTrue : MonoBehaviour, IQuest<Toggle>
    {
        [SerializeField]
        public GameObject Content;

        [SerializeField]
        Color onColor;

        [SerializeField]
        Color offColor;

        public List<Toggle> ListGeneric { get; set; }

        private Button buttonCheck;
        private QuestionManager questionManager;
        private LightBehaviour lightBehaviour;

        private void Awake()
        {
            questionManager = gameObject.GetComponentInParent<QuestionManager>();
        }

        public void AfterCheck(int i, bool result, bool loop = true) //check each toggle
        {
            
        }

        public int ListCheckedCount()
        {
            return -1;
        }

        private void AddToggle(Toggle t, string content)
        {
            try
            {
                ListGeneric.Add(t);
            }
            catch
            {
                ListGeneric = new List<Toggle>();
                ListGeneric.Add(t);
            }

            var label = t.gameObject.GetComponentInChildren<TMP_Text>();
            label.text = content;

            t.onValueChanged.AddListener((b) =>
            {
                if (b) label.color = onColor;
                else label.color = offColor;
            });

            t.isOn = false;
        }

        public void LockAllToggles() => ListGeneric.ForEach(
            (t) =>
            {
                t.interactable = false;
                t.onValueChanged.RemoveAllListeners();
            });

        public void Render(List<string> labels, Action<int> action)
        {
            // create question UI
            var canvasCtrl = gameObject.GetComponentInParent<CanvasController>();
            canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "ToggleFalseGuide", new Vector3(), true));
            canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "ToggleTrueGuide", new Vector3(), true));
            buttonCheck = canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/", "CheckResult", new Vector3(), true)).GetComponentInChildren<Button>();
            lightBehaviour = canvasCtrl.InstantiateUI(canvasCtrl.SubCanvasWorldPoint, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "Light_Bulb", new Vector3(), true)).GetComponent<LightBehaviour>();

            // create toggle
            for (int i = 0; i < labels.Count; i++)
            {
                var obj = canvasCtrl.InstantiateUI(Content, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "MultipleAnswer", new Vector3(), true));
                var toggle = obj.GetComponent<Toggle>();
                AddToggle(toggle, labels[i]);
            }

            // set up show result
            SetupShowResult(canvasCtrl.SubCanvasWorldPoint);

            // Set up time out
            questionManager.Timer.TimeOutEvent += Timer_TimeOutEvent;

            // add listener
            buttonCheck.onClick.AddListener(() =>
            {
                LockAllToggles();

                action(-1);

                buttonCheck.interactable = false;
                buttonCheck.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.75f); // set up to scale with Img on screen
                // can decor linear color for TextMeshPro...
                buttonCheck.onClick.RemoveAllListeners();
            });
        }

        private void Timer_TimeOutEvent()
        {
            buttonCheck.onClick.Invoke();

            questionManager.Timer.TimeOutEvent -= Timer_TimeOutEvent;
        }

        private void SetupShowResult(GameObject subCanvas)
        {
            var pos = Camera.main.transform.position; //not localPosition
            var camSize = Camera.main.sensorSize;
            pos.x += camSize.x * 0.4f / 10;
            //pos.y = 0.5f;
            pos.z = 0;
            pos.x -= subCanvas.transform.localPosition.x;
            pos.y -= subCanvas.transform.localPosition.y;
            lightBehaviour.gameObject.GetComponent<RectTransform>().localPosition = pos;

            questionManager.Sender += QuestionManager_Sender;
        }

        private void QuestionManager_Sender(bool result)
        {
            if (result)
            {
                lightBehaviour.ActiveState(2);
            }
            else
            {
                lightBehaviour.ActiveState(1);
            }

            StartCoroutine(StopToSeeResult(3));

            questionManager.Sender -= QuestionManager_Sender;
        }

        private IEnumerator StopToSeeResult(float s)
        {
            yield return new WaitForSecondsRealtime(s);

            Destroy(lightBehaviour.gameObject);
        }
    }
}