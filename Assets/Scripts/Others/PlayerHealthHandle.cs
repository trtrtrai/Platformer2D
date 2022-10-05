using Assets.Scripts.Character;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Others
{
    public class PlayerHealthHandle : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> hearts;

        [SerializeField]
        Color lost;

        private Health player;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(WaitToSeePlayerSpawn());
        }

        private IEnumerator WaitToSeePlayerSpawn()
        {

            var obj = GameObject.FindGameObjectWithTag("Player");

            while (obj is null)
            {
                obj = GameObject.FindGameObjectWithTag("Player");
                yield return null;
            }

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            for (int i = hearts.Count; i > 0 + player.Current; i--)
            {
                hearts[i].GetComponent<Image>().color = lost;
            }

            player.OnHit.AddListener((s) =>
            {
                hearts[player.Current].GetComponent<Image>().color = lost;
            });
        }
    }
}