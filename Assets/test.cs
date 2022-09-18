using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    [SerializeField]
    Camera mainCmr;
    // Start is called before the first frame update
    void Start()
    {
        var pos = mainCmr.transform.position; //not localPosition
        var camSize = Camera.main.sensorSize;
        var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Image"), gameObject.transform);
        pos.x += camSize.x * 0.4f / 10;
        //pos.y = 0.5f;
        pos.z = 0;
        pos.x -= gameObject.transform.localPosition.x;
        pos.y -= gameObject.transform.localPosition.y;
        obj.GetComponent<RectTransform>().localPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
