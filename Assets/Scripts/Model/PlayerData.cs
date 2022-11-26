using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
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

#if UNITY_STANDALONE
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

        public static PlayerDataJson GetPlayerDataJson(int n, bool isLoad = true)
        {
            if (isLoad)
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sReader = new StreamReader(Application.streamingAssetsPath + $"/PlayerData/{n}/PlayerInfo.txt"))
                using (JsonReader jReader = new JsonTextReader(sReader))
                {
                    return serializer.Deserialize<PlayerDataJson>(jReader);
                }
            }
            else return player;
        }
#endif

#if UNITY_ANDROID
        private static readonly string pData = "{\"Name\":\"$$$$$$$$$$$$$$$$$$$$\",\"CurrentStar\":0,\"MaxStar\":30,\"CurrentLevel\":0,\"MaxLevel\":10}";

        private static readonly string levelsData = "[{\"Name\":\"SampleScene\",\"isUnlock\":true,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-1\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-2\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-3\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-4\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-5\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-6\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-7\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-8\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},{\"Name\":\"Scene_1-9\",\"isUnlock\":false,\"isComplete\":false,\"isStar\":[false,false,false],\"HighPoints\":0},]";
        
        public static void SaveBeforeExit()
        {
            if (current == 0) return;

            using (var stream = new FileStream(path: Application.persistentDataPath + $"/LevelCompletedInfo{current}.txt", mode: FileMode.Open, access: FileAccess.Write, share: FileShare.ReadWrite)) // write in new file at persistent
            {
                Encoding encoding = Encoding.UTF8;

                using (var sWriter = new StreamWriter(stream, encoding))
                using (JsonWriter jReader = new JsonTextWriter(sWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jReader, levels);
                }
            }

            using (var stream = new FileStream(path: Application.persistentDataPath + $"/PlayerInfo{current}.txt", mode: FileMode.Open, access: FileAccess.Write, share: FileShare.ReadWrite)) // write in new file at persistent
            {
                Encoding encoding = Encoding.UTF8;

                using (var sWriter = new StreamWriter(stream, encoding))
                using (JsonWriter jReader = new JsonTextWriter(sWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jReader, player);
                }
            }
        }

        private static void GetLevels(int n)
        {
            using (var sReader = new StreamReader(Application.persistentDataPath + $"/LevelCompletedInfo{n}.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                levels = serializer.Deserialize<List<LevelData>>(jReader);
            }

            //Debug.Log(levels[0].Name);
        }

        private static void GetPlayer(int n)
        {
            using (var sReader = new StreamReader(Application.persistentDataPath + $"/PlayerInfo{n}.txt"))
            using (JsonReader jReader = new JsonTextReader(sReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                player = serializer.Deserialize<PlayerDataJson>(jReader);
            }

            //Debug.Log(player.Name);
        }

        public static PlayerDataJson GetPlayerDataJson(int n, bool isLoad = true)
        {
            if (!isLoad) return player;

            if (File.Exists(Application.persistentDataPath + $"/PlayerInfo{n}.txt")) // exists -> read -> convert
            {
                using (var sReader = new StreamReader(Application.persistentDataPath + $"/PlayerInfo{n}.txt"))
                using (JsonReader jReader = new JsonTextReader(sReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize<PlayerDataJson>(jReader);
                }
            }
            else // create new
            {
                #region PlayerInfo
                using (var stream = new FileStream(path: Application.persistentDataPath + $"/PlayerInfo{n}.txt", mode: FileMode.CreateNew, access: FileAccess.Write, share: FileShare.ReadWrite)) // write in new file at persistent
                {
                    Encoding encoding = Encoding.UTF8;

                    using (var sWriter = new StreamWriter(stream, encoding))
                    using (JsonWriter jReader = new JsonTextWriter(sWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jReader, JsonConvert.DeserializeObject<PlayerDataJson>(pData));
                    }
                }
                #endregion

                #region LevelCompletedInfo
                using (var stream = new FileStream(path: Application.persistentDataPath + $"/LevelCompletedInfo{n}.txt", mode: FileMode.CreateNew, access: FileAccess.Write, share: FileShare.ReadWrite)) // write in new file at persistent
                {
                    Encoding encoding = Encoding.UTF8;

                    using (var sWriter = new StreamWriter(stream, encoding))
                    using (JsonWriter jReader = new JsonTextWriter(sWriter))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jReader, JsonConvert.DeserializeObject<List<LevelData>>(levelsData));
                    }
                }
                #endregion

                return JsonConvert.DeserializeObject<PlayerDataJson>(pData);
            }
        }
#endif

        public static List<LevelData> GetLevelDatas()
        {
            return new List<LevelData>(levels);
        }

        public static void CreateNewUser(int n, string name)
        {
            //Debug.Log("Create new user " + n);

            InitiatePlayer(n);
            player.Name = name;
            SaveBeforeExit();
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