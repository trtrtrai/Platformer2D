using Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.Events;

public class EndLevelDetect : MonoBehaviour
{
    public UnityEvent<EndLevelState> OnCompletedLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //Debug.Log("Level Completed");
            OnCompletedLevel?.Invoke(EndLevelState.EndPoint);
            OnCompletedLevel = null;
        }
    }
}
