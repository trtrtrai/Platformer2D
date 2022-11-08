using Assets.Scripts.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    CharacterBehaviour characterBehaviour;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name.Equals("Left")) characterBehaviour.horizon = -1f;
        if (gameObject.name.Equals("Right")) characterBehaviour.horizon = 1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        characterBehaviour.horizon = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
