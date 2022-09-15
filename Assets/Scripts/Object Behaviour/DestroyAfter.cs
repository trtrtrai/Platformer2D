using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ObjectBehaviour
{
    public class DestroyAfter : MonoBehaviour
    {
        [SerializeField] private float timeWaitToDestroy;

        private float timer;
        // Start is called before the first frame update
        void Start()
        {
            timer = timeWaitToDestroy;
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}