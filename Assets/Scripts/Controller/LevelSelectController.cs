using Assets.Scripts.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    private SceneController sceneController;

    private List<string> listNames;

    // Start is called before the first frame update
    void Start()
    {
        listNames = new List<string>();
        var levelAmount = DirCount(new DirectoryInfo(Application.dataPath + "/Scenes/Level/"));
        //Debug.Log(levelAmount);

        for (int i = 0; i < levelAmount; i++)
        {
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelSelect/" + "GameLevel"), content.transform);
            obj.name = listNames[i];
            var btn = obj.GetComponent<Button>();
           btn.onClick.AddListener(() =>{
                btn.onClick.RemoveAllListeners();
                PlayLevel(obj);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int DirCount(DirectoryInfo d)
    {
        int i = 0;
        // Add file sizes.
        var fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Equals(".unity"))
            {
                var name = fi.Name.Substring(0, fi.Name.LastIndexOf(".unity"));             
                listNames.Add(name);
                i++;
            }
        }
        return i;
    }

    public void PlayLevel(GameObject obj) => sceneController.SwapScene(obj.name);
}
