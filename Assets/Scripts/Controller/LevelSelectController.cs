using Assets.Scripts.Controller;
using Assets.Scripts.Model;
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

    [SerializeField]
    GameObject missionManager;

    [SerializeField]
    GameObject initialPlayer;

    [SerializeField]
    Sprite normalBg;

    [SerializeField]
    Sprite selectedBg;

    private List<string> listNames;
    private int currentName;
    private Image currentPlayerBg;

    // Start is called before the first frame update
    void Start()
    {
        listNames = new List<string>();
        var levelAmount = DirCount(new DirectoryInfo(Application.dataPath + "/Scenes/Level/"));
        //Debug.Log(levelAmount);

        for (int i = 0; i < levelAmount; i++)
        {
            var t = i;
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelSelect/" + listNames[t]), content.transform);
            obj.name = listNames[t];
            var btn = obj.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                currentName = t;
                missionManager.GetComponent<MissionManager>().OpenMissionDialog(obj.GetComponentsInChildren<MissionData>());
                //set play button
                missionManager.SetActive(true);
            });
        }

        currentPlayerBg = initialPlayer.GetComponent<Image>();
        initialPlayer.GetComponentInChildren<Button>().onClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int DirCount(DirectoryInfo d)
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

    public void PlayLevel() => sceneController.SwapScene(listNames[currentName]);

    public void ChangeBackground(Image bg)
    {
        currentPlayerBg.sprite = normalBg;

        currentPlayerBg = bg;

        currentPlayerBg.sprite = selectedBg;
    }
}
