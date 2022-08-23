using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    GameObject topPart;

    [SerializeField]
    GameObject bottomPart;

    [SerializeField]
    bool isDetect;

    bool isInQues;
    WaitToDisable wait;

    // Start is called before the first frame update
    void Start()
    {
        isInQues = false;
        wait += Wait;
        //wait.Invoke(500);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetect)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Active");
                isInQues=true;
                GetComponent<Animator>().SetBool("isDestroy", true);
                topPart.SetActive(true);
                bottomPart.SetActive(true);
            }
        }

        if (isInQues)
        {
            isInQues = false;
            wait?.Invoke(500);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isDetect = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isDetect = false;
        }
    }


    private async Task Wait(int miliSec)
    {
        await Task.Delay(miliSec);
        //Debug.Log(miliSec);
        gameObject.SetActive(false);
    }

    private delegate Task WaitToDisable(int sec);
}
