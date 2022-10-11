using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
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

    [SerializeField]
    Color noStar;

    private List<LevelData> levels;
    private string currentName;
    private Image currentPlayerBg;
    private Transform child;

    // Start is called before the first frame update
    void Start()
    {
        var levels = PlayerData.GetLevelDatas();

        for (int i = 0; i < levels.Count; i++)
        {
            var t = i;
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelSelect/" + levels[t].Name), content.transform);
            obj.name = levels[t].Name;

            if (levels[t].isUnlock) SetupGameLevel(obj, levels[t]);
            else LockLevel(obj);
        }

        //set default
        currentPlayerBg = initialPlayer.GetComponent<Image>();
        initialPlayer.GetComponentInChildren<Button>().onClick.Invoke();
    }

    public void LockLevel(GameObject obj)
    {
        var btn = obj.GetComponent<Button>();
        btn.interactable = false;

        obj.transform.GetChild(1).GetComponentsInChildren<Image>().ToList().ForEach((i) => i.color = noStar);
        obj.transform.GetChild(2).GetComponent<TMP_Text>().text = "HighPts: none";
    }

    public void SetupGameLevel(GameObject obj, LevelData data)
    {
        if (data.isComplete)
        {
            var stars = obj.transform.GetChild(1).GetComponentsInChildren<Image>();
            for (int i = 0; i < 3; i++)
            {
                if (!data.IsStar[i]) stars[i].color = noStar;
            }

            obj.transform.GetChild(2).GetComponent<TMP_Text>().text = $"HighPts: {data.HighPoints}";
        }
        else
        {
            obj.transform.GetChild(1).GetComponentsInChildren<Image>().ToList().ForEach((i) => i.color = noStar);
            obj.transform.GetChild(2).GetComponent<TMP_Text>().text = "HighPts: none";
        }

        var btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            child = obj.GetComponentInChildren<RectTransform>().transform;
            currentName = data.Name;
            missionManager.GetComponent<MissionManager>().OpenMissionDialog(obj.GetComponentsInChildren<MissionData>());
            missionManager.SetActive(true);
        });
    }

    public void PlayLevel()
    {
        child.transform.SetParent(sceneController.DontDestroy.transform);
        sceneController.DontDestroy.Name = (CharacterName)Enum.Parse(typeof(CharacterName), currentPlayerBg.transform.GetChild(0).name);
        //Debug.Log(currentName);
        sceneController.SwapScene(currentName);
    }

    public void ChangeBackground(Image bg)
    {
        currentPlayerBg.sprite = normalBg;

        currentPlayerBg = bg;

        currentPlayerBg.sprite = selectedBg;
    }
}
