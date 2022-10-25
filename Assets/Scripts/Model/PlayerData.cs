using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Model
{
    public static class PlayerData
    {
        private static int current;
        private static PlayerDataJson player;
        private static List<LevelData> levels;

        public static void InitiatePlayer(int n)
        {
            current = n;
            GetPlayer(current);
            GetLevels(current);
        }

        private static void UpdatePlayer()
        {
            player.MaxLevel = levels.Count;
            player.MaxStar = player.MaxLevel * 3;

            player.CurrentLevelCompleted = 0;
            player.CurrentStar = 0;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].isUnlock)
                {
                    if (levels[i].isComplete)
                    {
                        player.CurrentLevelCompleted++;
                        player.CurrentStar += levels[i].IsStar.Count((s) => s);
                    }
                }
                else break; // or not?
            }
        }

        public static void UpdateLevel(List<bool> s, int p)
        {
            var thisLevel = levels.First((l) => l.Name == SceneManager.GetActiveScene().name);
            thisLevel.isComplete = true;
            
            if (s.Count((b) => b) >= thisLevel.IsStar.Count((s) => s)) 
                for (int i = 0; i < thisLevel.IsStar.Count; i++)
                {
                    thisLevel.IsStar[i] = s[i];
                }

            if (thisLevel.HighPoints < p) thisLevel.HighPoints = p;

            var index = levels.IndexOf(thisLevel);
            if (index == levels.Count - 1)
            {
                UpdatePlayer();
                return;
            }
            else levels[index + 1].isUnlock = true;

            UpdatePlayer();
        }

        public static void SaveBeforeExit()
        {
            if (current == 0) return;

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + $"/PlayerData/{current}/LevelCompletedInfo.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, levels);
            }

            serializer = new JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + $"/PlayerData/{current}/PlayerInfo.txt"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, player);
            }
        }

        private static void GetLevels(int n)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + $"/PlayerData/{n}/LevelCompletedInfo.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                levels = serializer.Deserialize<List<LevelData>>(jReader);
            }
            //Debug.Log(levels[0].Name);
        }

        private static void GetPlayer(int n)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + $"/PlayerData/{n}/PlayerInfo.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                player = serializer.Deserialize<PlayerDataJson>(jReader);
            }

            //Debug.Log(player.Name);
        }

        public static PlayerDataJson GetPlayerDataJson(int n)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + $"/PlayerData/{n}/PlayerInfo.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                return serializer.Deserialize<PlayerDataJson>(jReader);
            }
        }

        public static List<LevelData> GetLevelDatas()
        {
            return new List<LevelData>(levels);
        }

        public static void CreateNewUser(int n, string name)
        {
            //Debug.Log("Create new user " + n);

            InitiatePlayer(n);
            player.Name = name;
        }

        public static bool HaveUser(int n) 
        {
            var playerData = GetPlayerDataJson(n);

            if (playerData.Name.Equals("$$$$$$$$$$$$$$$$$$$$")) return false;
            else return true;
        }
    }

    public class PlayerDataJson
    {
        public string Name { get; set; }
        public int CurrentStar { get; set; }
        public int MaxStar { get; set; }
        public int CurrentLevelCompleted { get; set; }
        public int MaxLevel { get; set; }
    }

    public class LevelData
    {
        public string Name { get; set; }
        public bool isUnlock { get; set; }
        public bool isComplete { get; set; }
        public List<bool> IsStar { get; set; }
        public int HighPoints { get; set; }
    }
}