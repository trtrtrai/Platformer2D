using System.Threading.Tasks;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    int timeToDestroy;

    [SerializeField]
    GameController controller;

    private bool isDetect;
    private bool isInQues;
    private WaitToDisable wait;

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
                isInQues = true;
                GetComponent<Animator>().SetBool("isDestroy", true);
                /*topPart.SetActive(true);
                bottomPart.SetActive(true);*/
            }
        }

        if (isInQues)
        {
            isInQues = false;
            wait?.Invoke(timeToDestroy);
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
        Debug.Log(miliSec);
        gameObject.SetActive(false);

        var pos = gameObject.transform.localPosition;
        pos.x += 0.02f;
        var obj = controller.InvokeResourcesLoad(gameObject, new GameController.ResourcesLoadEventHandler("Prefabs/", "Block_part_top", pos));
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1.5f), ForceMode2D.Impulse);

        pos = gameObject.transform.localPosition;
        pos.x -= 0.04f;
        obj = controller.InvokeResourcesLoad(gameObject, new GameController.ResourcesLoadEventHandler("Prefabs/", "Block_part_bottom", pos));
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 1.5f), ForceMode2D.Impulse);
    }

    private delegate Task WaitToDisable(int sec);
}
