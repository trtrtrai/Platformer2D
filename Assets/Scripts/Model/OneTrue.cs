using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTrue : MonoBehaviour
{
    public List<Button> Buttons;

    private List<int> exceptedBtns;

    private void Start()
    {
        exceptedBtns = new List<int>();
    }

    public void AfterCheck(int i, bool result, bool loop = true)
    {
        if (result) Buttons[i].gameObject.GetComponent<Image>().color = new Color(0, 1, 0);
        else
        {
            if (ButtonClicked() == 0) Buttons[i].gameObject.GetComponent<Image>().color = new Color(1, 59 / 255, 59 / 255);
            if (loop) FindRightButton(i);
        }
    }

    private void FindRightButton(int except)
    {
        exceptedBtns.Add(except);

        for (int i = 0; i < Buttons.Count; i++)
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

            if (isInvoke) Buttons[i].onClick.Invoke();
        }
    }

    public int ButtonClicked() => exceptedBtns.Count;
}
