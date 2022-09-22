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

            if (!(gameObject.GetComponentInChildren<DropItem>() is null)) //it have 1 DropItem.cs, don't accept fill
            {
                rect.anchoredPosition = rect.parent.GetComponent<RectTransform>().anchoredPosition;
                return;
            }

            var thisRect = GetComponent<RectTransform>();

            rect.transform.SetParent(thisRect.transform);
            rect.anchoredPosition = thisRect.anchoredPosition;
        }
    }
}
