using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSM : MonoBehaviour
{
    [Header("STATE")]
    public PlayerState currentState;

    [Header("ANIMATIONS")]
    [SerializeField] AnimationClip jumpClip;
    [SerializeField] Animator animator;

    [Header("SPEED")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpHeight = 2f;

    Vector2 dirInput;
    Vector2 jumpDirection;


    Rigidbody2D rb2D;



    public enum PlayerState
    {
        IDLE,
        WALK,
        RUN,
        PUNCH,
        JUMP,
        DEAD,
    }




    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.IDLE;
        rb2D = GetComponent<Rigidbody2D>();
        OnStateEnter();




    }

    // Update is called once per frame
    void Update()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dirInput != Vector2.zero)
        {
            animator.SetFloat("InputX", dirInput.x);
            animator.SetFloat("InputY", dirInput.y);

        }

        OnStateUpdate();
    }

    private void FixedUpdate()
    {
        OnStateFixedUpdate();
    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                animator.SetBool("IDLE", true);
                rb2D.velocity = Vector2.zero;
                break;
            case PlayerState.WALK:
                animator.SetBool("WALK", true);
                break;
            case PlayerState.RUN:
                animator.SetBool("RUN", true);
                break;

            case PlayerState.JUMP:
                animator.SetTrigger("JUMP");
                jumpDirection = new Vector2(animator.GetFloat("InputX"), animator.GetFloat("InputY"));
                StartCoroutine(WaitForRoll());
                break;
            case PlayerState.PUNCH:
                animator.SetTrigger("PUNCH");
                break;

            default:
                break;
        }
    }

    void OnStateUpdate()
    {

        switch (currentState)
        {


            case PlayerState.IDLE:

                // TO RUN OR SPRINT
                if (dirInput.magnitude != 0)
                {
                    TransitionToState(Input.GetKey(KeyCode.LeftShift) ? PlayerState.RUN : PlayerState.WALK);

                }

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    // CALCULE DE LA FORCE EN FONCTION DU SAUT
                    jumpForce = Mathf.Sqrt(-2 * Physics2D.gravity.y * rb2D.gravityScale * jumpHeight);


                    rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    TransitionToState(PlayerState.JUMP);

                }

                //TO PUNCH
                if (Input.GetMouseButtonDown(0))
                {


                    
                    TransitionToState(PlayerState.PUNCH);

                }

                break;


            case PlayerState.WALK:

                // TO IDLE
                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);

                }

                // TO SPRINT
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.RUN);
                }

                // TO ROLL
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMP);
                }

                if (Input.GetMouseButtonDown(0))
                {



                    TransitionToState(PlayerState.PUNCH);

                }


                break;

            case PlayerState.RUN:

                // TO IDLE
                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                    break;
                }

                // TO RUN
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.WALK);
                    break;
                }

                // TO ROLL
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMP);
                }

                if (Input.GetMouseButtonDown(0))
                {



                    TransitionToState(PlayerState.PUNCH);

                }

                break;

            case PlayerState.PUNCH:


                break;

            case PlayerState.DEAD:

                //if(dirInput.magnitude == 0)
                //{
                //    animator.SetBool("IDLE", true);
                //    animator.SetBool("RUN", false);
                //}

                //else
                //{
                //    animator.SetBool("IDLE", false);
                //    animator.SetBool("RUN", true);
                //}


                break;

            default:
                break;
        }

    }

    void OnStateFixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:

                break;
            case PlayerState.WALK:
                rb2D.velocity = dirInput.normalized * speed;
                break;

            case PlayerState.RUN:
                rb2D.velocity = dirInput.normalized * sprintSpeed;

                break;


            case PlayerState.JUMP:
                //rb2D.velocity = jumpDirection.normalized * jumpHeight;
                break;

            case PlayerState.PUNCH:

                break;


            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                animator.SetBool("IDLE", false);
                break;
            case PlayerState.WALK:
                animator.SetBool("WALK", false);
                break;

            case PlayerState.RUN:
                animator.SetBool("RUN", false);
                break;

            case PlayerState.JUMP:
                break;

            default:
                break;
        }
    }

    void TransitionToState(PlayerState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    IEnumerator WaitForRoll()
    {

        yield return new WaitForSeconds(jumpClip.length);
        TransitionToState(PlayerState.IDLE);

    }


}
