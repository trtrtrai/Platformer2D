using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void InstantiateUI(GameController.ResourcesLoadEventHandler args) => gameController.InvokeResourcesLoad(gameObject, args);

    public void SetState(GameState type) => gameController.SetGameState(type);
}
