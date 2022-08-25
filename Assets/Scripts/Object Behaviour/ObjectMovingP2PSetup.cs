using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(ObjectMovingP2P))]
public class ObjectMovingP2PSetup : MonoBehaviour
{
    [SerializeField]
    Axis objAxis;

    [SerializeField]
    [Range(1, 100)]
    int numberOfChain = 1;

    [SerializeField]
    Sprite chain;

    [SerializeField]
    GameObject mainObject;

    [SerializeField]
    GameObject firstPoint;

    [SerializeField]
    GameObject lastPoint;

    [SerializeField]
    [Range(0f, 100f)]
    float mainObjPosition;

    [SerializeField]
    private List<GameObject> listChains;

    private float mainObjPos;
    private bool isHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        if (listChains is null)
        {
            listChains = new List<GameObject>();
            gameObject.GetComponentsInChildren<Transform>().ToList().ForEach((t) => listChains.Add(t.gameObject));
            listChains.Remove(listChains[0]); // Remove main object
        }

        mainObjPos = mainObjPosition;
        isHorizontal = isHorizontalInEditMode();
        //gameObject.GetComponentsInChildren<Transform>().ToList().ForEach((t) => Debug.Log(t.gameObject.name));
    }

    private void LateUpdate()
    {
        if (isHorizontalInEditMode() != isHorizontal)
        {
            //Reset
            var old = numberOfChain;
            numberOfChain = 1;
            if (isHorizontal) CreateChainsHorizontal();
            else CreateChainsVertical();

            numberOfChain = old;
            mainObject.transform.localPosition = new Vector3(0, 0);

            isHorizontal = isHorizontalInEditMode();
        }

        if (isHorizontal) CreateChainsHorizontal();
        else CreateChainsVertical();

        if (mainObjPosition != mainObjPos)
        {
            UpdateMainObjPosition();
            mainObjPos = mainObjPosition;
        }
    }

    private bool isHorizontalInEditMode() => objAxis.Equals(Axis.Horizontal);

    private void CreateChainsHorizontal()
    {
        if (listChains.Count == 0)
        {
            //Create first chain
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Chain"), gameObject.transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.GetComponent<SpriteRenderer>().sprite = chain;
            listChains.Add(obj);
            firstPoint = obj;
        }
        else if (listChains.Count > numberOfChain)
        {
            while (true)
            {
                var obj = listChains[listChains.Count - 1];
                listChains.Remove(obj);
                UpdatePoint();
                DestroyImmediate(obj);
                if (listChains.Count == numberOfChain) break;
            }
        }
        else if (listChains.Count < numberOfChain)
        {
            while (true)
            {
                var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Chain"), gameObject.transform);

                float pos;
                pos = Mathf.Abs(listChains[listChains.Count - 1].transform.localPosition.x);
                pos += isOddCountListObj() ? 0.1f : 0;
                pos *= isOddCountListObj() ? 1f : -1f;
                obj.transform.localPosition = new Vector3(pos, 0, 0);

                obj.GetComponent<SpriteRenderer>().sprite = chain;
                listChains.Add(obj);

                if (listChains.Count == numberOfChain)
                {
                    UpdatePoint();
                    break;
                }
            }
        }
    }

    private void CreateChainsVertical()
    {
        if (listChains.Count == 0)
        {
            //Create first chain
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Chain"), gameObject.transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.GetComponent<SpriteRenderer>().sprite = chain;
            listChains.Add(obj);
            firstPoint = obj;
        }
        else if (listChains.Count > numberOfChain)
        {
            while (true)
            {
                var obj = listChains[listChains.Count - 1];
                listChains.Remove(obj);
                UpdatePoint();
                DestroyImmediate(obj);
                if (listChains.Count == numberOfChain) break;
            }
        }
        else if (listChains.Count < numberOfChain)
        {
            while (true)
            {
                var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Chain"), gameObject.transform);

                float pos;
                pos = Mathf.Abs(listChains[listChains.Count - 1].transform.localPosition.y);
                pos += isOddCountListObj() ? 0.1f : 0;
                pos *= isOddCountListObj() ? 1f : -1f;
                obj.transform.localPosition = new Vector3(0, pos, 0);

                obj.GetComponent<SpriteRenderer>().sprite = chain;
                listChains.Add(obj);

                if (listChains.Count == numberOfChain)
                {
                    UpdatePoint();
                    break;
                }
            }
        }
    }

    private void UpdateMainObjPosition()
    {
        var position = mainObject.transform.localPosition;

        if (isHorizontal)
        {
            if (listChains.Count > 2)
            {
                position.x = (listChains.Count - 1) * 0.1f * mainObjPosition / 100;
                position.x -= lastPoint.transform.localPosition.x - (isOddCountListObj() ? 0 : 0.1f);
            }
            else if (listChains.Count == 2) position.x = 0.1f * mainObjPosition / 100;
            else position.x = 0;
        }
        else
        {
            if (listChains.Count > 2)
            {
                position.y = (listChains.Count - 1) * 0.1f * mainObjPosition / 100;
                position.y -= lastPoint.transform.localPosition.y - (isOddCountListObj() ? 0 : 0.1f);
            }
            else if (listChains.Count == 2) position.y = 0.1f * mainObjPosition / 100;
            else position.y = 0;
        }
        
        mainObject.transform.localPosition = position;
    }

    private bool isOddCountListObj()
    {
        if (listChains is null) return false;

        return listChains.Count % 2 == 1 ? true : false;
    }

    private void UpdatePoint()
    {
        if (listChains.Count == 1)
        {
            firstPoint = lastPoint = listChains[0];
        }
        else
        {
            var o1 = listChains[listChains.Count - 1];
            var o2 = listChains[listChains.Count - 2];
            firstPoint = isOddCountListObj() ? o1 : o2;
            lastPoint = isOddCountListObj() ? o2 : o1;
        }   
    }
}
