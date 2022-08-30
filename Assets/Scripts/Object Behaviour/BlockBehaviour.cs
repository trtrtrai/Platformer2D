using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    int timeToDestroy;

    [SerializeField]
    GameController gameController;

    [SerializeField]
    GameObject eSymbol;

    private bool isDetect;
    private bool isInQues;
    private WaitToDisable wait;
    private GameObject QuestionObj;

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
                //Debug.Log("Active");
                QuestionObj = gameController.CanvasController.InstantiateUI(new ResourcesLoadEventHandler("Prefabs/", "QuestionUI", new Vector3(), true));
                //QuestionObj.GetComponent<QuestionManager>().
            }
        }

        if (isInQues)
        {
            GetComponent<Animator>().SetBool("isDestroy", true);
            isInQues = false;
            wait?.Invoke(timeToDestroy);
        }
    }

    public void Answer(bool result)
    {
        isInQues = result;
        if (result) Destroy(GameObject.Find("QuestionUI(Clone)").gameObject);
        gameController.PopGameState();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PositionESymbol(collision.gameObject.transform.localPosition);
            eSymbol.SetActive(true);
            isDetect = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            eSymbol.SetActive(false);
            isDetect = false;
        }
    }

    private void PositionESymbol(Vector3 player)
    {
        var obj = gameObject.transform.localPosition;
        if (player.x < obj.x) eSymbol.transform.localPosition = new Vector3(0.1f, 0.1f);
        else if (player.x > obj.x) eSymbol.transform.localPosition = new Vector3(-0.1f, 0.1f);
        //...
    }

    private async Task Wait(int miliSec)
    {
        await Task.Delay(miliSec);
        //Debug.Log(miliSec);
        gameObject.SetActive(false);

        var pos = gameObject.transform.localPosition;
        pos.x += 0.02f;
        var obj = gameController.InvokeResourcesLoad(gameObject, new ResourcesLoadEventHandler("Prefabs/", "Block_part_top", pos));
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1.5f), ForceMode2D.Impulse);

        pos = gameObject.transform.localPosition;
        pos.x -= 0.04f;
        obj = gameController.InvokeResourcesLoad(gameObject, new ResourcesLoadEventHandler("Prefabs/", "Block_part_bottom", pos));
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 1.5f), ForceMode2D.Impulse);
    }

    private delegate Task WaitToDisable(int sec);
}
