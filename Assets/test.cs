using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Fill script;
    private RectTransform rect;
    private CanvasGroup group;

    private void Awake()
    {  
        rect = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
        script = GetComponentInParent<Fill>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ReferenceEquals(transform.parent, script.ListGeneric[3].transform)) transform.SetParent(script.ListGeneric[3].transform);
        group.alpha = .6f;
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        group.alpha = 1f;
        group.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
