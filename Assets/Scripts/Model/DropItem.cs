using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Controller;

public class DropItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    Vector2 anchorMinAnswer;

    [SerializeField]
    Vector2 anchorMaxAnswer;

    private Fill script;
    private GameObject currentParent;
    private RectTransform rect;
    private CanvasGroup group;
    private int index;

    public int Index => index;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
        currentParent = transform.parent.gameObject;
        script = gameObject.GetComponentInParent<CanvasController>().GetComponentInChildren<Fill>();
        index = int.Parse(currentParent.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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

        if (!ReferenceEquals(currentParent, transform.parent.gameObject)) currentParent = transform.parent.gameObject;
        CheckFillAnswer();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void DefaultRectSetting()
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
    }

    private void RectSetting()
    {
        rect.anchorMin = anchorMinAnswer;
        rect.anchorMax = anchorMaxAnswer;
    }

    private void CheckFillAnswer()
    {
        if (script.ListGeneric.Contains(currentParent)) RectSetting();
        else DefaultRectSetting();

        rect.offsetMin = Vector3.zero;
        rect.offsetMax = Vector3.zero;
    }
}
