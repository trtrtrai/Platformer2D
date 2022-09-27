using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rigid;

    [SerializeField]
    float strength, delay;

    public UnityEvent OnBegin, OnDone;

    public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized; Debug.Log(direction);
        var script = sender.GetComponent<ObjectDetectHit>();
        if (script is null) rigid.AddForce(direction * strength, ForceMode2D.Impulse);
        else rigid.AddForce(direction * (sender.transform.position.y < gameObject.transform.position.y ? script.Strength + 0.5f : script.Strength), ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rigid.velocity = Vector2.zero;
        OnDone?.Invoke();
    }
}
