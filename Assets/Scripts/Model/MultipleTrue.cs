using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
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

        private void Awake()
        {
            questionManager = gameObject.GetComponentInParent<QuestionManager>();
        }

        public void AfterCheck(int i, bool result, bool loop = true) //check each toggle
        {
            throw new System.NotImplementedException();
        }

        public int ListCheckedCount()
        {
            throw new System.NotImplementedException();
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
            buttonCheck = canvasCtrl.InstantiateUI(questionManager.gameObject, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "CheckMultipleTrue", new Vector3(), true)).GetComponentInChildren<Button>();

            // create toggle
            labels.ForEach((a) =>
            {
                var obj = canvasCtrl.InstantiateUI(Content, new ResourcesLoadEventHandler("Prefabs/UI/Question/MultipleTrue/", "MultipleAnswer", new Vector3(), true));
                var toggle = obj.GetComponent<Toggle>();
                AddToggle(toggle, a);
            });

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
    }
}