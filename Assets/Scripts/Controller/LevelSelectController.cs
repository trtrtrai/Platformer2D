using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    GameObject playerInfo;

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

    [SerializeField]
    GameObject btnPrevious;

    [SerializeField]
    GameObject btnNext;

    private List<LevelData> levels;
    private List<GameObject> contents;
    private string currentName;
    private Image currentPlayerBg;
    private Transform child;
    private int currentContent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitDontDestroy());

        levels = PlayerData.GetLevelDatas();

        contents = new List<GameObject>();
        contents.Add(Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelSelect/Content"), gameObject.transform));

        var j = 0;
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == (j + 1) * 8)
            {
                j++;
                contents.Add(Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelSelect/Content"), gameObject.transform));
            }

            var t = i;
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelSelect/" + levels[t].Name), contents[j].transform);
            obj.name = levels[t].Name;

            if (levels[t].isUnlock) SetupGameLevel(obj, levels[t]);
            else LockLevel(obj);
        }

        //set default
        currentPlayerBg = initialPlayer.GetComponent<Image>();
        initialPlayer.GetComponentInChildren<Button>().onClick.Invoke();
        InitialContent();
    }

    private IEnumerator WaitDontDestroy()
    {
        if (sceneController.DontDestroy is null) yield return null;

        var playerData = PlayerData.GetPlayerDataJson(sceneController.DontDestroy.PlayerIndex, false);

        //Load player info
        var txts = playerInfo.GetComponentsInChildren<TMP_Text>();
        txts[0].text = playerData.Name; //txt name
        txts[1].text = $"{playerData.CurrentStar}/{playerData.MaxStar}"; // txt star
        int pts = 0;
        levels.ForEach((l) => pts += l.HighPoints);
        txts[2].text = "Tổng điểm: " + pts; // txt sumpts
    }

    private void InitialContent()
    {
        for (int i = 1; i < contents.Count; i++)
        {
            contents[i].SetActive(false);
        }
        currentContent = 0;

        btnPrevious.SetActive(false);
        if (contents.Count == 1) btnNext.SetActive(false);
    }

    public void PreviousPage()
    {
        if (currentContent == contents.Count - 1) btnNext.SetActive(true);
        contents[currentContent].SetActive(false);
        currentContent--;
        contents[currentContent].SetActive(true);

        if (currentContent == 0) btnPrevious.SetActive(false);
    }

    public void NextPage()
    {
        if (currentContent == 0) btnPrevious.SetActive(true);
        contents[currentContent].SetActive(false);
        currentContent++;
        contents[currentContent].SetActive(true);

        if (currentContent == contents.Count - 1) btnNext.SetActive(false);
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
            missionManager.transform.parent.gameObject.SetActive(true);
        });
    }

    public void PlayLevel()
    {
        child.transform.SetParent(sceneController.DontDestroy.transform);
        sceneController.DontDestroy.Name = (CharacterName)Enum.Parse(typeof(CharacterName), currentPlayerBg.transform.GetChild(0).name);
        //Debug.Log(currentName);
        sceneController.SwapScene(currentName);
    }

    public void ChangeBackground(Image bg) //Choose character
    {
        currentPlayerBg.sprite = normalBg;

        currentPlayerBg = bg;

        currentPlayerBg.sprite = selectedBg;
    }
}
