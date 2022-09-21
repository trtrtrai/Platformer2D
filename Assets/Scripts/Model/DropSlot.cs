using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //Debug.Log(name);
            var rect = eventData.pointerDrag.GetComponent<RectTransform>();
            var thisRect = GetComponent<RectTransform>();
            // if it have 1 test.cs, don't accept fill
            rect.transform.SetParent(thisRect.transform);
            rect.anchoredPosition = thisRect.anchoredPosition;
        }
    }
}
