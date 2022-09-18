using Assets.Scripts.Controller;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
    public class OneTrue : MonoBehaviour, IQuest<Button>
    {
        [SerializeField]
        private List<Button> buttons;

        public List<Button> ListGeneric { get => buttons; set => buttons = value; }

        private List<int> exceptedBtns;
        private QuestionManager questionManager;

        private void Awake()
        {
            exceptedBtns = new List<int>();
            questionManager = gameObject.GetComponentInParent<QuestionManager>();
        }

        public void AfterCheck(int i, bool result, bool loop = true)
        {
            if (result) ListGeneric[i].gameObject.GetComponent<Image>().color = questionManager.LabelTrue;
            else
            {
                if (ListCheckedCount() == 0) ListGeneric[i].gameObject.GetComponent<Image>().color = questionManager.LabelFalse;
                if (loop) FindRightButton(i);
            }
        }

        private void FindRightButton(int except)
        {
            exceptedBtns.Add(except);

            for (int i = 0; i < ListGeneric.Count; i++)
            {
                var isInvoke = true;
                for (int j = 0; j < exceptedBtns.Count; j++)
                {
                    if (i == exceptedBtns[j])
                    {
                        isInvoke = false;
                        break;
                    }
                }

                if (isInvoke) ListGeneric[i].onClick.Invoke();
            }
        }

        public int ListCheckedCount() => exceptedBtns.Count;

        public void Render(List<string> labels, Action<int> action)
        {
            for (int i = 0; i < ListGeneric.Count; i++)
            {
                var t = i;
                ListGeneric[t].GetComponentInChildren<TMP_Text>().text = labels[t];
                ListGeneric[t].onClick.AddListener(() =>
                {
                    action(t);

                    buttons[t].onClick.RemoveAllListeners();
                });
            }

            // Set up time out
            questionManager.Timer.TimeOutEvent += Timer_TimeOutEvent;
        }

        private void Timer_TimeOutEvent()
        {
            ListGeneric.ForEach((b) => b.interactable = false);

            questionManager.Timer.TimeOutEvent -= Timer_TimeOutEvent;
        }
    }
}