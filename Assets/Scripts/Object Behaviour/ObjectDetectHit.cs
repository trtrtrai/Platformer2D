using Assets.Scripts.Model;
using UnityEngine;

public class ObjectDetectHit : MonoBehaviour
{
    public float Strength;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<Health>().GetHit(1, gameObject);
        }
    }
}
