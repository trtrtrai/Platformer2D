using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int current;

    [SerializeField]
    private int max;

    [SerializeField]
    private bool isDead;

    private bool penalty;

    public int Current 
    {
        get { return current; }
        private set 
        {
            var h = Mathf.Clamp(value, 0, max);
            if (current > h) { current = h; }
            else if (current < h) { current = h; /*Add heart invoke*/}

            if (current == 0) isDead = true;
        }
    }

    public void GetHit(int damage, GameObject sender)
    {
        if (!penalty)
        {
            Debug.Log(sender.name);
            Current -= damage;
            OnHit?.Invoke(sender);
        }
    }

    private void Awake()
    {
        current = max;
        isDead = false;
        penalty = false;
        OnHit.AddListener((s) => { penalty = true; StartCoroutine(Penalty(1f)); }); // 1 sec to runnnnnnn
    }

    private IEnumerator Penalty(float s)
    {
        yield return new WaitForSeconds(s);
        penalty = false;
    }

    private void OnDestroy()
    {
        OnHit.RemoveAllListeners();
    }

    public UnityEvent<GameObject> OnHit;
}
