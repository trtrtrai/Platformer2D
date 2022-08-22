using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ObjectMovingP2PSetup))]
public class ObjectMovingP2P : MonoBehaviour
{
    [SerializeField]
    bool startAtRight;

    [SerializeField]
    [Range(0f, 100f)]
    float speed;

    GameObject firstPoint;
    GameObject lastPoint;

    private void Awake()
    {
        gameObject.GetComponent<ObjectMovingP2PSetup>().enabled = false;    
    }

    // Start is called before the first frame update
    void Start()
    {
        var listChains = new List<GameObject>();
        gameObject.GetComponentsInChildren<Transform>().ToList().ForEach((t) => listChains.Add(t.gameObject));
        
        if(listChains.Count % 2 == 1)
        {
            firstPoint = listChains[listChains.Count - 1];
            lastPoint = listChains[listChains.Count - 2];
        }
        else
        {
            firstPoint = listChains[listChains.Count - 2];
            lastPoint = listChains[listChains.Count - 1];
        }
        //Debug.Log(firstPoint.transform.localPosition);
        //Debug.Log(lastPoint.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
