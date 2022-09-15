using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject InstantiateUI(ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(gameObject, args);

        public GameObject InstantiateUI(GameObject parent, ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(parent, args);

        public void SetState(GameState type) => gameController.SetGameState(type);

        public void PopState() => gameController.PopGameState();
    }
}