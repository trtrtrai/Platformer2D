using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.ObjectBehaviour
{
    [RequireComponent(typeof(BlockSetup))]
    public class BlockBehaviour : MonoBehaviour
    {
        [SerializeField]
        int timeToDestroy;

        [SerializeField]
        GameController gameController;

        [SerializeField]
        GameObject eSymbol;

        [SerializeField]
        [Range(30f, 180f)]
        float timeForAnswer;

        private bool isDetect;
        private event WaitToDisable wait;
        private GameObject QuestionObj;
        private BlockSetup setup;
        private Health player;

        private void Awake()
        {
            setup = gameObject.GetComponent<BlockSetup>();
            setup.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            wait += Wait;
            //wait.Invoke(500);

#if UNITY_ANDROID
            touchPack = gameController.CanvasController.TouchScreenPackage.transform.GetChild(3).gameObject;     
#endif
        }

#if UNITY_STANDALONE
        private void Update()
        {
            if (isDetect)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Action();
                }
            }
         }
#endif
#if UNITY_ANDROID
        private GameObject touchPack;
#endif

        private void Action()
        {
            //Debug.Log("Active");
            QuestionObj = gameController.CanvasController.InstantiateUI(new ResourcesLoadEventHandler("Prefabs/UI/Question/", "QuestionUI", new Vector3(), true));
            var questMng = QuestionObj.GetComponent<QuestionManager>();
            questMng.Sender += Answer;
            questMng.TimeAnswer = timeForAnswer;
            enabled = false;
        }

        private void Answer(bool result)
        {
            QuestionObj.GetComponent<QuestionManager>().UnSubSender(Answer);
            StartCoroutine(StopToSeeResult(3, result));
        }

        private IEnumerator StopToSeeResult(float s, bool result)
        {
            yield return new WaitForSecondsRealtime(s);

            Destroy(QuestionObj);
            QuestionObj = null;
            gameController.GameContinue();

            if (result)
            {
                SoundPackage.Controller.PlayAudio("Explosion");
                setup.PlayAnimation();
                wait?.Invoke(timeToDestroy);
            }
            else
            {
                //health + knock
                player.GetHit(1, gameObject);
                //Debug.Log("Wrong answer");
            }

            enabled = true;
        }

#if UNITY_ANDROID
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Player")) touchPack.GetComponent<Button>().onClick.AddListener(() => { if (QuestionObj is null) Action(); });
        }
#endif

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                player = collision.gameObject.GetComponent<Health>();
                PositionESymbol(collision.gameObject.transform.localPosition);
                eSymbol.SetActive(true);
                isDetect = true;

#if UNITY_ANDROID
                touchPack.SetActive(true);
#endif
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                eSymbol.SetActive(false);
                isDetect = false;

#if UNITY_ANDROID
                touchPack.GetComponent<Button>().onClick.RemoveAllListeners();
                touchPack.SetActive(false);
#endif
            }
        }

        private void PositionESymbol(Vector3 player)
        {
            var box = gameObject.GetComponent<BoxCollider2D>();
            var obj = gameObject.transform.localPosition + new Vector3(box.offset.x, box.offset.y);
            switch (setup.ObjAxis)
            {
                case Axis.Horizontal:
                    if (player.x < obj.x) eSymbol.transform.localPosition = new Vector3(0.1f + box.size.x - 0.16f, 0.1f);
                    else if (player.x > obj.x) eSymbol.transform.localPosition = new Vector3(-0.1f, 0.1f);
                    break;
                case Axis.Vertical:
                    if (player.x < obj.x) eSymbol.transform.localPosition = new Vector3(0.1f, 0.1f + box.size.y - 0.16f);
                    else if (player.x > obj.x) eSymbol.transform.localPosition = new Vector3(-0.1f, 0.1f + box.size.y - 0.16f);
                    break;
            }

        }

        private async Task Wait(int miliSec)
        {
            await Task.Delay(miliSec);
            //Debug.Log(miliSec);

            setup.OnParentDestroy(gameController);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            wait -= Wait;
        }

        private delegate Task WaitToDisable(int sec);
    }
}