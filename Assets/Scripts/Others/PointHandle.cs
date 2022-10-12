using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Others
{
    public class PointHandle : MonoBehaviour
    {
        [SerializeField]
        CollectionInfo info;

        [SerializeField]
        private TMP_Text pointField;

        private PointCounter player;
        public int Points { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitToSeePlayerSpawn());
            Points = 0;
            UpdateUI();
        }

        private IEnumerator WaitToSeePlayerSpawn()
        {

            var obj = GameObject.FindGameObjectWithTag("Player");

            while (obj is null)
            {
                obj = GameObject.FindGameObjectWithTag("Player");
                yield return null;
            }

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PointCounter>();

            player.PointListener += Player_PointListener;
        }

        private void Player_PointListener(Collection sender)
        {
            int add = info.GetPoints(sender.Name); // anyway
            //Debug.Log("Collected " + sender.Name + " is " + add + " point.");

            Points += add;
            UpdateUI();
        }

        private void UpdateUI()
        {
            pointField.text = Points.ToString();
        }

        private void OnDestroy()
        {
            player.PointListener -= Player_PointListener;
        }
    }
}