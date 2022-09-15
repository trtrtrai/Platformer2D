using Assets.Scripts.Controller;
using Assets.Scripts.ObjectBehaviour;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Others.CustomRandom;

namespace Assets.Scripts.Model
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(BlockBehaviour))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BlockSetup : MonoBehaviour
    {
        [SerializeField]
        Axis objAxis;

        [SerializeField]
        [Range(1, 100)]
        int numberOfChain = 1;

        [SerializeField]
        List<GameObject> blocks;

        [SerializeField]
        new BoxCollider2D collider;

        private bool isHorizontal;

        public Axis ObjAxis => objAxis;

        // Start is called before the first frame update
        void Start()
        {
            if (blocks is null)
            {
                blocks = new List<GameObject>();
                gameObject.GetComponentsInChildren<Transform>().ToList().ForEach((t) => blocks.Add(t.gameObject));
                blocks.Remove(blocks[0]); // Remove parent object
            }

            isHorizontal = isHorizontalInEditMode();
        }

        private void LateUpdate()
        {
            if (isHorizontalInEditMode() != isHorizontal)
            {
                var old = numberOfChain;
                numberOfChain = 1;
                if (isHorizontal) CreateHorizontal();
                else CreateVertical();

                numberOfChain = old;

                isHorizontal = isHorizontalInEditMode();
            }

            if (isHorizontal) CreateHorizontal();
            else CreateVertical();
        }

        private void CreateHorizontal()
        {
            if (blocks.Count == 0)
            {
                //Create first chain
                var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Block"), gameObject.transform);
                obj.transform.localPosition = new Vector3(0, 0, 0);
                blocks.Add(obj);
            }
            else if (blocks.Count > numberOfChain)
            {
                while (true)
                {
                    var obj = blocks[blocks.Count - 1];
                    blocks.Remove(obj);
                    DestroyImmediate(obj);
                    if (blocks.Count == numberOfChain)
                    {
                        collider.offset = new Vector2((numberOfChain - 1) * 0.08f, 0);
                        collider.size = new Vector2(numberOfChain * 0.16f, 0.16f);
                        break;
                    }
                }
            }
            else if (blocks.Count < numberOfChain)
            {
                while (true)
                {
                    var obj = Instantiate(Resources.Load<GameObject>("Prefabs/BLock"), gameObject.transform);

                    float pos;
                    pos = Mathf.Abs(blocks[blocks.Count - 1].transform.localPosition.x);
                    pos += 0.16f;
                    obj.transform.localPosition = new Vector3(pos, 0, 0);
                    blocks.Add(obj);

                    if (blocks.Count == numberOfChain)
                    {
                        collider.offset = new Vector2((numberOfChain - 1) * 0.08f, 0);
                        collider.size = new Vector2(numberOfChain * 0.16f, 0.16f);
                        break;
                    }
                }
            }
        }

        private void CreateVertical()
        {
            if (blocks.Count == 0)
            {
                //Create first chain
                var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Block"), gameObject.transform);
                obj.transform.localPosition = new Vector3(0, 0, 0);
                blocks.Add(obj);
            }
            else if (blocks.Count > numberOfChain)
            {
                while (true)
                {
                    var obj = blocks[blocks.Count - 1];
                    blocks.Remove(obj);
                    DestroyImmediate(obj);
                    if (blocks.Count == numberOfChain)
                    {
                        collider.offset = new Vector2(0, (numberOfChain - 1) * 0.08f);
                        collider.size = new Vector2(0.16f, numberOfChain * 0.16f);
                        break;
                    }
                }
            }
            else if (blocks.Count < numberOfChain)
            {
                while (true)
                {
                    var obj = Instantiate(Resources.Load<GameObject>("Prefabs/BLock"), gameObject.transform);

                    float pos;
                    pos = Mathf.Abs(blocks[blocks.Count - 1].transform.localPosition.y);
                    pos += 0.16f;
                    obj.transform.localPosition = new Vector3(0, pos, 0);
                    blocks.Add(obj);

                    if (blocks.Count == numberOfChain)
                    {
                        collider.offset = new Vector2(0, (numberOfChain - 1) * 0.08f);
                        collider.size = new Vector2(0.16f, numberOfChain * 0.16f);
                        break;
                    }
                }
            }
        }

        public void OnParentDestroy(GameController gameController)
        {
            blocks.ForEach((b) =>
            {
                Destroy(b);

                // block will break some parts when destroy
                var pos = gameObject.transform.localPosition + b.transform.localPosition;
                pos.x += 0.02f;
                var obj = gameController.InvokeResourcesLoad(b, new ResourcesLoadEventHandler("Prefabs/", "Block_part_top", pos));
                obj.GetComponent<Rigidbody2D>().AddForce(RdVector2(-1.5f, -0.5f, 1f, 2f), ForceMode2D.Impulse);
                obj.GetComponent<Rigidbody2D>().angularVelocity = 300;

                pos = gameObject.transform.localPosition + b.transform.localPosition;
                pos.x -= 0.04f;
                obj = gameController.InvokeResourcesLoad(b, new ResourcesLoadEventHandler("Prefabs/", "Block_part_bottom", pos));
                obj.GetComponent<Rigidbody2D>().AddForce(RdVector2(1.5f, 0.5f, 1f, 2f), ForceMode2D.Impulse);
                obj.GetComponent<Rigidbody2D>().angularVelocity = 300;
            });
        }

        public void PlayAnimation()
        {
            blocks.ForEach((b) => b.GetComponent<Animator>().SetBool("isDestroy", true));
        }

        private bool isHorizontalInEditMode() => objAxis.Equals(Axis.Horizontal);
    }
}