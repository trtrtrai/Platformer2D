using Assets.Scripts.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class MainCameraRotation : MonoBehaviour
    {
        [SerializeField]
        GameController gameCtrl;

        Camera main;
        CharacterBehaviour player;

        private void Start()
        {
            var gObj = GameObject.FindGameObjectWithTag("Player");

            player = gObj.GetComponent<CharacterBehaviour>();

            main = gObj.GetComponentInChildren<Camera>();
            main.transform.localRotation = Quaternion.Euler(0, 0, 90f);
        }

        private void FixedUpdate()
        {
            if (gameCtrl.IsEqualsTopDisplay(GameState.GameDisplay) || gameCtrl.IsEqualsTopDisplay(GameState.TwoChoiceQuestionDisplay))
            {

                if (player.Horizon > 0)
                {
                    main.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                }
                else if (player.Horizon < 0)
                {
                    main.transform.localRotation = Quaternion.Euler(0, 0, -90f);
                }
            }
        }
    }
}