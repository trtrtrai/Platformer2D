using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public string ThisScene;
    public GameObject MissionObj;
    public CharacterName Name;

    // Start is called before the first frame update
    void Start()
    {
        //Don't destroy on load script
        var objs = GameObject.FindGameObjectsWithTag("DontDestroy"); //only right when have 1 obj don't destroy

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);      
    }

    public void SwapScene(string name)
    {
        var child = gameObject.GetComponentInChildren<RectTransform>();
        if (child != null) MissionObj = child.gameObject;

        ThisScene = name;
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public MissionData[] GetMission()
    {
        return MissionObj.transform.GetChild(0).GetComponentsInChildren<MissionData>();
    }
}
