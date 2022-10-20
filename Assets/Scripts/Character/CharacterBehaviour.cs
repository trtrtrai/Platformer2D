using Assets.Scripts.Controller;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Character
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(PointCounter))]
    public class CharacterBehaviour : MonoBehaviour
    {
        private GameController gameCtrl;

        [SerializeField]
        [Range(0, 100)]
        float speed;

        [SerializeField]
        [Range(0, 100)]
        float jumpHigh;

        [SerializeField] Rigidbody2D rigid;
        [SerializeField] float maxSpeed;
        [SerializeField] int jumpCount = 2;
        [SerializeField] bool isGrounded;
        [SerializeField] LayerMask ground;
        [SerializeField] new CircleCollider2D collider2D;
        [SerializeField] Animator animator;

        private float jumpCD;
        private bool moveContinue;
        private bool isMove;

        // Start is called before the first frame update
        void Start()
        {
            gameCtrl = GameObject.Find("GameController").GetComponent<GameController>();
            moveContinue = false;
            isMove = false;
        }

        private void Update()
        {
            if (gameCtrl.IsEqualsTopDisplay(GameState.GameDisplay) || gameCtrl.IsEqualsTopDisplay(GameState.TwoChoiceQuestionDisplay))
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded || jumpCount < 1)
                    {
                        //Debug.Log("Jump");
                        SoundPackage.Controller.PlayAudio("Jump");
                        rigid.AddForce(transform.up * jumpHigh * Time.fixedDeltaTime, ForceMode2D.Impulse);
                        rigid.velocity = new Vector2(rigid.velocity.x, Vector2.ClampMagnitude(rigid.velocity, 5f).y);
                        jumpCount++;
                    }
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (gameCtrl.IsEqualsTopDisplay(GameState.GameDisplay) || gameCtrl.IsEqualsTopDisplay(GameState.TwoChoiceQuestionDisplay))
            {
                DetectGround();

                animator.SetFloat("Yveloc", Mathf.Clamp(rigid.velocity.y, -1f, 4f));
                var horizon = Input.GetAxis("Horizontal");
                //Left-Right
                rigid.AddForce(new Vector2(horizon * speed * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
                rigid.velocity = new Vector2(Vector2.ClampMagnitude(rigid.velocity, maxSpeed).x, rigid.velocity.y);
                if (horizon != 0)
                {
                    animator.SetBool("isRun", true);
                    isMove = true;
                }
                else
                {
                    animator.SetBool("isRun", false);
                    isMove = false;
                }

                if (moveContinue != isMove)
                {
                    if (!moveContinue) SoundPackage.Controller.PlayAudio("Walk");
                    else SoundPackage.Controller.StopAudio("Walk");
                    moveContinue = isMove;
                }

                //Flip
                if (horizon > 0)
                {
                    var scale = gameObject.transform.localScale;
                    scale.x = 1;

                    gameObject.transform.localScale = scale;
                }
                else if (horizon < 0)
                {
                    var scale = gameObject.transform.localScale;
                    scale.x = -1;

                    gameObject.transform.localScale = scale;
                }
            }
        }

        void DetectGround()
        {
            if (Physics2D.CircleCast(collider2D.bounds.center, collider2D.bounds.extents.y, Vector2.down, 0.05f, ground).collider != null)
            {
                //Debug.Log("Detect ground");
                isGrounded = true;
                jumpCount = 0;
                jumpCD = Time.time + 0.2f;
            }
            else if (Time.time < jumpCD) isGrounded = true;
            else isGrounded = false;

            animator.SetBool("isJump", jumpCount == 0 && !isGrounded ? true : false);
        }

        private IEnumerator WaitToDestroy()
        {
            yield return new WaitForSeconds(0.5f);
            gameCtrl.InvokeResourcesLoad(gameObject, new ResourcesLoadEventHandler("Prefabs/Players/", "MainCameraAfterDead", gameObject.transform.position, false));
            Destroy(gameObject);
            gameCtrl.LevelCompletedHandle(EndLevelState.Dead);
        }

        public void PlayAppearAnim() => animator.Play("CharacterAppearing");

        public void IsDead(bool result)
        {
            if (result)
            {
                animator.Rebind();
                animator.Play("CharacterDisappearing");
                StartCoroutine(WaitToDestroy());
            }
        } 
    }
}