using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestPointDetect : MonoBehaviour
{
    public UnityEvent OnRest;

    private bool firstTime = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!firstTime && collision.gameObject.tag.Equals("Player"))
        {
            SoundPackage.Controller.PlayAudio("RestPoint");
            collision.gameObject.GetComponent<Health>().GetHit(-2, gameObject);
            OnRest?.Invoke();
            firstTime = true;
        }
    }

    public void PlayAnimation()
    {
        GetComponent<Animator>().SetBool("playerTouch", true);
    }
}
