using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
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
        public Button ButtonCheck;

        private QuestionManager questionManager;

        private void Start()
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

        public void AddToggle(Toggle t, string content)
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
    }
}