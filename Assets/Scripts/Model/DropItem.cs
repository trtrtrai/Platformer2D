using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Controller;

public class DropItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Fill script;
    private RectTransform rect;
    private CanvasGroup group;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        script = gameObject.GetComponentInParent<QuestionManager>().GetComponentInChildren<Fill>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ReferenceEquals(transform.parent, script.ListGeneric[3].transform)) transform.SetParent(script.ListGeneric[3].transform);
        group.alpha = .75f;
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
        //Debug.Log(Camera.main.ScreenToWorldPoint(rect.anchoredPosition));
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
