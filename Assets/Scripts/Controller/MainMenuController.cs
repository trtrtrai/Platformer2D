using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    SceneController sceneController;

    [SerializeField]
    GameObject createUserName;

    [SerializeField]
    List<GameObject> users;

    [SerializeField]
    TMP_Text inputFieldName;

    private int currentChoice;

    public void DisplayPlayerData()
    {
        for (int i = 1; i <= users.Count; i++)
        {
            if (PlayerData.HaveUser(i))
            {
                var data = PlayerData.GetPlayerDataJson(i);//display
                var txts = users[i-1].GetComponentsInChildren<TMP_Text>();

                txts[1].text = data.Name;
                txts[2].text = $"{data.CurrentStar}/{data.MaxStar}";
                txts[3].text = $"{data.CurrentLevelCompleted}/{data.MaxLevel}";
            }
        }
    }

    public void LoadPlayerData(int no)
    {
        if (!PlayerData.HaveUser(no))
        {
            currentChoice = no;
            createUserName.SetActive(true);
        }
        else
        {
            PlayerData.InitiatePlayer(no);
            sceneController.SwapScene("LevelSelectionScene");
            sceneController.Loading.SetActive(true);
        }
    }

    public void CreateNewUser()
    {
        PlayerData.CreateNewUser(currentChoice, inputFieldName.text);
    }
}
