using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event ResourcesLoadDelegate ResourcesLoad;

    // Start is called before the first frame update
    void Start()
    {
        ResourcesLoad += InstantiateObject;
    }

    public GameObject InvokeResourcesLoad(object sender, ResourcesLoadEventHandler args) => ResourcesLoad?.Invoke(sender, args);

    private GameObject InstantiateObject(object sender, ResourcesLoadEventHandler args)
    {
        var obj = Instantiate(Resources.Load<GameObject>(args.Path + args.ObjectName), gameObject.transform);
        obj.transform.localPosition = args.Position;

        return obj;
    }

    public class ResourcesLoadEventHandler : EventArgs
    {
        public readonly string Path;
        public readonly string ObjectName;
        public readonly Vector3 Position;

        public ResourcesLoadEventHandler(string path, string objectName, Vector3 position)
        {
            Path = path;
            ObjectName = objectName;
            Position = position;
        }
    }

    public delegate GameObject ResourcesLoadDelegate(object sender, ResourcesLoadEventHandler args);
}

public enum Axis
{
    Horizontal,
    Vertical,
}
