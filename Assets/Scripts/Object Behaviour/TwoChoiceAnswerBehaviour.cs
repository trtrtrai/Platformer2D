using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TwoChoiceAnswerBehaviour : MonoBehaviour
{
    [SerializeField]
    Light2D light;

    [SerializeField]
    ParticleSystem particleSystem;

    private bool turn = false;
    //private float speed = 2f;

    public event TwoChoiceTrigger TriggerPublisher;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("Player"))
        {
            TriggerPublisher?.Invoke(name);
        }
    }

    private void Update()
    {
        if (!turn)
        {
            light.intensity -= Time.deltaTime;
        }
        else light.intensity += Time.deltaTime;

        if (light.intensity > 1.3f) turn = false;
        if (light.intensity < 1f) turn = true;
    }

    public void ShowResultField(Color color)
    {
        light.color = color;

        var main = particleSystem.main;
        main.startColor = color;
    }

    public delegate void TwoChoiceTrigger(string name);
}
