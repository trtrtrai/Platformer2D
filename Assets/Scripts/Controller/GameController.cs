using Assets.Scripts.Character;
using Assets.Scripts.Model;
using Assets.Scripts.Others;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class GameController : MonoBehaviour
    {
        private Stack<string> gameState;
        public event ResourcesLoadDelegate ResourcesLoad;
        public CanvasController CanvasController;
        public SceneController SceneController;
        public GameObject StartPoint;
        public GameObject EndPoint;
        public List<GameObject> RestPoints;
        public float FullTime;

        [SerializeField]
        Timer gameTime;

        [SerializeField]
        List<GameObject> specialControllers;

        private float time;

        // Start is called before the first frame update
        void Start()
        {
            ResourcesLoad += InstantiateObject;
            gameState = new Stack<string>();
            SetGameState(GameState.GameDisplay);
            time = FullTime; //load from data
            gameTime.TimeOutEvent += GameTime_TimeOutEvent;

            SceneController.Loading.SetActive(true);
            StartCoroutine(WaitToSeeDontDestroy());
            //SpawnPlayer();
        }

        private void GameTime_TimeOutEvent()
        {
            LevelCompletedHandle(EndLevelState.TimeOut);
        }

        private IEnumerator WaitToSeeDontDestroy()
        {
            while (SceneController.DontDestroy is null) yield return null;

            if (SceneController.Loading.activeInHierarchy)
            {
                SceneController.Loading.SetActive(false);
                Destroy(GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject);
            }
            var child = StartPoint.transform.GetChild(0).localPosition;
            var player = InvokeResourcesLoad(gameObject, new ResourcesLoadEventHandler("Prefabs/Players/", SceneController.DontDestroy.Name.ToString(), StartPoint.transform.localPosition + child, false));
            player.GetComponent<CharacterBehaviour>().PlayAppearAnim();
            specialControllers.ForEach((s) => s.SetActive(true));
        }

        private void Update()
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                gameTime.UpdateTimer(time);
            }
        }

        #region GameState
        public void SetGameState(GameState type) //=> GameState.Push(type.ToString());
        {
            gameState.Push(type.ToString());
            //Debug.Log(type.ToString());
        }

        public void PopGameState() => gameState.Pop();

        public bool IsEqualsTopDisplay(GameState type) => type.ToString() == gameState.Peek();
        #endregion

        public GameObject InvokeResourcesLoad(object sender, ResourcesLoadEventHandler args) => ResourcesLoad?.Invoke(sender, args);

        private GameObject InstantiateObject(object sender, ResourcesLoadEventHandler args)
        {
            GameObject obj;
            if (args.IsChildren)
            {
                obj = Instantiate(Resources.Load<GameObject>(args.Path + args.ObjectName), (sender as GameObject).transform);
            }
            else obj = Instantiate(Resources.Load<GameObject>(args.Path + args.ObjectName));

            if (args.Position != Vector3.zero) obj.transform.localPosition = args.Position;

            return obj;
        }

        public void LevelCompletedHandle(string state) => LevelCompletedHandle((EndLevelState)Enum.Parse(typeof(EndLevelState), state)); //for Inspector Unity

        public void LevelCompletedHandle(EndLevelState state)
        {
            //Debug.Log(state);
            GamePause();
            
            switch (state)
            {
                case EndLevelState.EndPoint:
                    {
                        SoundPackage.Controller.PlayAudio("EndPoint");
                        // calculation result
                        var missions = SceneController.DontDestroy.GetMission();
                        var stars = new List<bool>();
                        for (int i = 0; i < 3; i++)
                        {
                            stars.Add(CheckMissonCompleted(missions[i]));
                        }
                        CanvasController.ShowResultLevel(state, stars);
                        PlayerData.UpdateLevel(stars, CanvasController.GetComponentInChildren<PointHandle>().Points);
                        break;
                    }
                case EndLevelState.Dead:
                    {
                        SoundPackage.Controller.PlayAudio("Failed");
                        CanvasController.ShowResultLevel(state, new List<bool>() { false, false, false});
                        break;
                    }
                case EndLevelState.TimeOut:
                    {
                        SoundPackage.Controller.PlayAudio("Failed");
                        CanvasController.ShowResultLevel(state, new List<bool>() { false, false, false });
                        break;
                    }
                case EndLevelState.Exit:
                    {
                        SoundPackage.Controller.PlayAudio("Failed");
                        CanvasController.ShowResultLevel(state, new List<bool>() { false, false, false });
                        break;
                    }
            }
        }

        private bool CheckMissonCompleted(MissionData data)
        {
            switch (data.Type)
            {
                case MissionType.LevelCompleted:
                    {
                        return true;
                    }
                case MissionType.Point:
                    {
                        if (CanvasController.GetComponentInChildren<PointHandle>().Points >= data.PointsChallenge)
                        {
                            return true;
                        }
                        else return false;
                    }
                case MissionType.CompletionTime:
                    {
                        if ((FullTime - time) <= data.SecondsChallenge)
                        {
                            return true;
                        }
                        else return false;
                    }
                case MissionType.FullCollection:
                    {
                        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PointCounter>();

                        int c = 0;
                        for (int i = 0; i < data.NumberOfCollection; i++)
                        {
                            if (player.GetAmount(data.Names[i]) >= data.Amount[i]) c++;
                            else break;
                        }

                        if (c == data.NumberOfCollection) return true;
                        else return false;
                    }
                case MissionType.PerfectCompleted:
                    {
                        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

                        if (player.Current == player.Max) return true;
                        else return false;
                    }
                default: return false;
            }
        }

        public void GamePause() => Time.timeScale = 0;

        public void GameContinue() => Time.timeScale = 1;

        public delegate GameObject ResourcesLoadDelegate(object sender, ResourcesLoadEventHandler args);
    }

    public class ResourcesLoadEventHandler : EventArgs
    {
        public readonly string Path;
        public readonly string ObjectName;
        public readonly Vector3 Position;
        public readonly bool IsChildren;

        public ResourcesLoadEventHandler(string path, string objectName, Vector3 position, bool isChildren = false)
        {
            Path = path;
            ObjectName = objectName;
            Position = position;
            IsChildren = isChildren;
        }
    }

    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public enum GameState
    {
        GameDisplay,
        QuestionDisplay,
        TwoChoiceQuestionDisplay,
        NotificationDisplay,
        EndLevelDisplay,
    }

    public enum QuestionType
    {
        OneTrue,
        MultipleTrue,
        Fill,
        TwoChoice,
    }

    public enum MissionType
    {
        LevelCompleted,
        CompletionTime,
        Point,
        FullCollection,
        PerfectCompleted
    }

    public enum CharacterName
    {
        NinjaFrog,
        PinkMan,
        MaskDude,
        VirtualGuy
    }

    public enum EndLevelState
    {
        EndPoint,
        Dead,
        TimeOut,
        Exit,
    }

    public enum CollectionFruits
    {
        Apple,
        Bananas,
        Cherries,
        Kiwi,
        Melon,
        Orange,
        Pineapple,
        Strawberry,
    }
}