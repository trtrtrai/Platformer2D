using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MessageContainer : MonoBehaviour
    {
        private CanvasController parent;
        // Start is called before the first frame update
        void Start()
        {
            parent = gameObject.transform.parent.gameObject.GetComponent<CanvasController>();
            parent.SetState(GameState.NotificationDisplay);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            parent.PopState();
        }
    }
}