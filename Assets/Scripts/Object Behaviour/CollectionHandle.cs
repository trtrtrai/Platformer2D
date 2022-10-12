using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ObjectBehaviour
{
    public class CollectionHandle : MonoBehaviour
    {

        private List<TMP_Text> collected;
        private MissionData data;
        private List<int> amount;

        private void CollectionHandle_PointListener(Collection sender)
        {
            if (data.Names.Contains(sender.Name))
            {
                int index = data.Names.IndexOf(sender.Name);
                if (amount[index] == 0) return;
                amount[index]--;
                if (amount[index] == 0) collected[index].text = "Done!";
                else collected[index].text = $"{amount[index]} left";
            }
        }

        public void CheckMission(MissionData[] datas, CollectionInfo info) //for level only have one FullCollection enum and NumberOfCollection <= 3
        {
            foreach (var data in datas)
            {
                //Debug.Log(data.Type);
                if (data.Type == MissionType.FullCollection)
                {
                    this.data = data;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PointCounter>().PointListener += CollectionHandle_PointListener;

                    collected = new List<TMP_Text>();
                    amount = new List<int>();
                    var imgs = gameObject.GetComponentsInChildren<Image>();

                    for (int i = 0; i < imgs.Length; i++)
                    {
                        collected.Add(imgs[i].GetComponentInChildren<TMP_Text>());
                        if (i < data.NumberOfCollection)
                        {
                            //Debug.Log("Set image " + data.Names[i]);
                            //Debug.Log("Set amount " + data.Amount[i]);
                            imgs[i].sprite = info.GetSprite(data.Names[i]);
                            amount.Add(data.Amount[i]);
                            collected[i].text = amount[i] + " left";
                        }
                        else imgs[i].gameObject.SetActive(false);
                    }

                    return;
                }
            }

            gameObject.SetActive(false);
        }
    }
}