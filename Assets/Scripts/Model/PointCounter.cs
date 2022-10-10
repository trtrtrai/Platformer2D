using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class PointCounter : MonoBehaviour
    {
        public event PointCounterDelegate PointListener;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Collection"))
            {
                //Debug.Log(500);
                //var sender = .Name;
                PointListener?.Invoke(collision.gameObject.GetComponent<Collection>());
            }
        }

        public delegate void PointCounterDelegate(Collection sender);
    }
}