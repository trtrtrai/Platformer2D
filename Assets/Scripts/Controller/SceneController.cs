using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private DontDestroyOnLoad dontDestroy;
    // Start is called before the first frame update
    void Start()
    {
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoad>();
    }

    public void SwapScene(string name) => dontDestroy.SwapScene(name);

    public void QuitGame() => dontDestroy.QuitGame();
}
