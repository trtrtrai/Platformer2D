using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test2 : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null){
            //Debug.Log(name);
            var rect = eventData.pointerDrag.GetComponent<RectTransform>();
            var thisRect = GetComponent<RectTransform>();
            // if it have 1 test.cs, don't accept fill
            rect.transform.SetParent(thisRect.transform);
            rect.anchoredPosition = thisRect.anchoredPosition;
        }
    }
}
