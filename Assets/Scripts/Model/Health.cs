using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Model
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int current;

        [SerializeField]
        private int max;

        [SerializeField]
        private bool isDead;

        private bool penalty;

        public int Current
        {
            get { return current; }
            private set
            {
                var h = Mathf.Clamp(value, 0, max);
                if (current > h) { current = h; }
                else if (current < h) { current = h; /*Add heart invoke*/}

                if (current == 0) isDead = true;
            }
        }

        public int Max => max;

        public void GetHit(int damage, GameObject sender)
        {
            if (sender.GetComponent<RestPointDetect>() != null) {Current -= damage; return; }

            if (!penalty)
            {
                //Debug.Log(sender.name);
                Current -= damage;
                OnHit?.Invoke(sender);
                OnDead?.Invoke(isDead);
            }
        }

        private void Awake()
        {
            current = max;
            isDead = false;
            penalty = false;
            OnHit.AddListener((s) => { SoundPackage.Controller.PlayAudio("Hit"); penalty = true; StartCoroutine(Penalty(1f)); }); // 1 sec to runnnnnnn
            OnDead.AddListener((s) => { if (s) SoundPackage.Controller.PlayAudio("Died"); });
        }

        private IEnumerator Penalty(float s)
        {
            yield return new WaitForSeconds(s);
            penalty = false;
        }

        private void OnDestroy()
        {
            OnHit.RemoveAllListeners();
        }

        public UnityEvent<GameObject> OnHit;
        public UnityEvent<bool> OnDead;
    }
}