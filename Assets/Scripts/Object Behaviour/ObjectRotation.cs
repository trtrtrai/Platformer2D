using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 180f)]
    float rad;

    [SerializeField]
    [Range(0f, 100f)]
    float speed;

    [SerializeField]
    bool startAtRight;

    float circular;
    
    // Start is called before the first frame update
    void Start()
    {
        circular = 0f;

        if (startAtRight)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, rad);
        }
        else
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -rad);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startAtRight)
        {
            var angle = speed * Time.deltaTime;
            circular += angle;
            gameObject.transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.back);
            if (circular >= 2 * rad)
            {
                startAtRight = false;
                circular = 0f;
            }
        }
        else
        {
            var angle = speed * Time.deltaTime;
            circular += angle;
            gameObject.transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.forward);
            if (circular >= 2 * rad)
            {
                startAtRight = true;
                circular = 0f;
            }
        }
    }
}
