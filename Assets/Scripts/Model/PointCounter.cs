using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class PointCounter : MonoBehaviour
    {
        public event PointCounterDelegate PointListener;

        private Dictionary<CollectionFruits, int> listCollected;
        private List<GameObject> olds = new List<GameObject>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Collection") && !olds.Contains(collision.gameObject))
            {
                //Debug.Log(50);
                olds.Add(collision.gameObject);
                var script = collision.gameObject.GetComponent<Collection>();
                AddCollection(script.Name);
                PointListener?.Invoke(script);
            }
        }

        public int GetAmount(CollectionFruits f) { if (listCollected is null) return 0; else return listCollected.ContainsKey(f) ? listCollected[f] : 0; }

        private void AddCollection(CollectionFruits f)
        {
            try
            {
                if (listCollected.ContainsKey(f)) listCollected[f]++;
                else listCollected.Add(f, 1);
            }
            catch
            {
                listCollected = new Dictionary<CollectionFruits, int>();
                listCollected.Add(f, 1);
            }
        }

        public delegate void PointCounterDelegate(Collection sender);
    }
}