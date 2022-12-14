using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ObjectBehaviour
{
    [RequireComponent(typeof(ObjectMovingP2PSetup))]
    public class ObjectMovingP2P : MonoBehaviour
    {
        [SerializeField]
        bool startAtRight;

        [SerializeField]
        [Range(0f, 3f)]
        float speed;

        GameObject firstPoint;
        GameObject lastPoint;
        GameObject mainObj;

        private void Awake()
        {
            gameObject.GetComponent<ObjectMovingP2PSetup>().enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            var listChains = new List<GameObject>();
            gameObject.GetComponentsInChildren<Transform>().ToList().ForEach((t) => listChains.Add(t.gameObject));

            mainObj = listChains[1];
            if (listChains.Count % 2 == 1)
            {
                firstPoint = listChains[listChains.Count - 1];
                lastPoint = listChains[listChains.Count - 2];
            }
            else
            {
                firstPoint = listChains[listChains.Count - 2];
                lastPoint = listChains[listChains.Count - 1];
            }

            if (startAtRight) mainObj.transform.localScale = new Vector3(-1, 1, 1);
            else mainObj.transform.localScale = new Vector3(1, 1, 1);
            //Debug.Log(firstPoint.transform.localPosition);
            //Debug.Log(lastPoint.transform.localPosition);
        }

        // Update is called once per frame
        private void Update()
        {
            if (speed != 0f)
            {
                mainObj.GetComponent<Animator>().SetBool("turnOn", true);
            }
            else mainObj.GetComponent<Animator>().SetBool("turnOn", false);
        }

        void FixedUpdate()
        {
            var fpAxis = firstPoint.transform.localPosition;
            var lpAxis = lastPoint.transform.localPosition;
            var mainPos = mainObj.transform.localPosition;
            //if (gameObject.name.Equals("Saw")) Debug.Log(fpAxis == lpAxis);
            if (fpAxis == lpAxis) startAtRight = true;
            else if (fpAxis.y == 0f)
            {
                if (startAtRight && mainPos.x < lpAxis.x)
                {
                    var num = mainPos.x + speed * Time.fixedDeltaTime;
                    mainPos.x = Mathf.Clamp(num, fpAxis.x, lpAxis.x);
                }
                else if (!startAtRight && mainPos.x > fpAxis.x)
                {
                    var num = mainPos.x - speed * Time.fixedDeltaTime;
                    mainPos.x = Mathf.Clamp(num, fpAxis.x, lpAxis.x);
                }
                else if (mainPos.x == lpAxis.x || mainPos.x == fpAxis.x)
                {
                    startAtRight = !startAtRight;

                    if (startAtRight) mainObj.transform.localScale = new Vector3(-1, 1, 1);
                    else mainObj.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                if (startAtRight && mainPos.y < lpAxis.y)
                {
                    var num = mainPos.y + speed * Time.fixedDeltaTime;
                    mainPos.y = Mathf.Clamp(num, fpAxis.y, lpAxis.y);
                }
                else if (!startAtRight && mainPos.y > fpAxis.y)
                {
                    var num = mainPos.y - speed * Time.fixedDeltaTime;
                    mainPos.y = Mathf.Clamp(num, fpAxis.y, lpAxis.y);
                }
                else if (mainPos.y == lpAxis.y || mainPos.y == fpAxis.y)
                {
                    startAtRight = !startAtRight;

                    if (startAtRight) mainObj.transform.localScale = new Vector3(1, 1, 1);
                    else mainObj.transform.localScale = new Vector3(1, -1, 1);
                }
            }

            mainObj.transform.localPosition = mainPos;
        }
    }
}