using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovingP2P : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    int numberOfChain;

    [SerializeField]
    Sprite chain;

    [SerializeField]
    GameObject mainObject;

    [SerializeField]
    GameObject firstPoint;

    [SerializeField]
    GameObject lastPoint;

    [SerializeField]
    bool startAtRight;

    [SerializeField]
    [Range(0f, 100f)]
    float initialPlace;

    // Start is called before the first frame update
    void Start()
    {
        if (numberOfChain <= 2) gameObject.GetComponent<ObjectMovingP2P>().enabled = false;

        for (int i = 0; i < numberOfChain; i++)
        {
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Chain"), gameObject.transform);
            obj.transform.localPosition = new Vector3(i * 0.1f, 0, 0);
            obj.GetComponent<SpriteRenderer>().sprite = chain;

            if (i == 0) firstPoint = obj;
            if (i == numberOfChain - 1) lastPoint = obj;
        }

        var position = mainObject.transform.localPosition;
        position.x = (numberOfChain - 1) * 0.1f * initialPlace / 100;
        mainObject.transform.localPosition = position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
