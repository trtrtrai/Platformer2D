using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Stack<string> GameState;
    public event ResourcesLoadDelegate ResourcesLoad;
    public CanvasController CanvasController;

    // Start is called before the first frame update
    void Start()
    {
        ResourcesLoad += InstantiateObject;
        GameState = new Stack<string>();
        SetGameState(global::GameState.GameDisplay);
    }

    #region GameState
    public void SetGameState(GameState type) //=> GameState.Push(type.ToString());
    {
        GameState.Push(type.ToString());
        Debug.Log(type.ToString());
    }

    public void PopGameState() => GameState.Pop();

    public bool IsEqualsTopDisplay(GameState type) => type.ToString() == GameState.Peek();
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

        if (args.Position != new Vector3()) obj.transform.localPosition = args.Position;

        return obj;
    }

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

}

public enum QuestionType
{
    OneTrue,
    MultipleTrue,
    Connect,
    SlideNumber
}
