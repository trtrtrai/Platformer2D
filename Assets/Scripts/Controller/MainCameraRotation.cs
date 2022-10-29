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

        private void Start()
        {
            main = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
            main.transform.localRotation = Quaternion.Euler(0, 0, 90f);
        }

        private void FixedUpdate()
        {
            if (gameCtrl.IsEqualsTopDisplay(GameState.GameDisplay) || gameCtrl.IsEqualsTopDisplay(GameState.TwoChoiceQuestionDisplay))
            {
                var horizon = Input.GetAxis("Horizontal");

                if (horizon > 0)
                {
                    main.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                }
                else if (horizon < 0)
                {
                    main.transform.localRotation = Quaternion.Euler(0, 0, -90f);
                }
            }
        }
    }
}