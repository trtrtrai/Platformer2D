using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Collection : MonoBehaviour
    {
        public CollectionFruits Name;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                //Debug.Log(Name + " was collected");
                GetComponent<Animator>().Play("Collected");
                StartCoroutine(WaitAnimation());
            }
        }

        private IEnumerator WaitAnimation()
        {
            yield return new WaitForSeconds(0.15f);
            Destroy(gameObject);
        }
    }
}