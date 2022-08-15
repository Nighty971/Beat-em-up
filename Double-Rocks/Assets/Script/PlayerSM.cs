using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSM : MonoBehaviour
{
    [SerializeField] Transform graphics;
    [Header("STATE")]
    public PlayerState currentState;

    [Header("ANIMATIONS")]
    [SerializeField] AnimationClip punchClip;
    [SerializeField] AnimationClip tauntClip;
    [SerializeField] AnimationCurve _jumpCurve;
    [SerializeField] Animator animator;

    [Header("SPEED")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float jumpTimer;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpDuration = 2f;
    
    Vector2 dirInput;
    Vector2 jumpDirection;


    public PlayerHealth playerHealth;
    Rigidbody2D rb2D;



    public enum PlayerState
    {
        IDLE,
        WALK,
        HURT,
        RUN,
        PUNCH,
        JUMP,
        TAUNT,
        DEAD,
        ULTIMATE,
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

        GetMoveDirection();
       
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
                
                //StartCoroutine(WaitForJump());
                break;
            case PlayerState.PUNCH:
                rb2D.velocity = Vector2.zero;
                animator.SetTrigger("PUNCH");
                StartCoroutine(Punch());
                break;

            case PlayerState.TAUNT:
                animator.SetTrigger("TAUNT");
                
                break;
            case PlayerState.HURT:
                animator.SetTrigger("HURT");
                rb2D.velocity = Vector2.zero;

                break;
            case PlayerState.DEAD:
                animator.SetBool("isDead", true);
                rb2D.velocity = Vector2.zero;
                gameObject.layer = 8;

                break;

            case PlayerState.ULTIMATE:
                animator.SetBool("isReviving", true);
                rb2D.velocity = Vector2.zero;
                gameObject.layer = 8;

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

                    
                    TransitionToState(PlayerState.JUMP);

                }

                


                //TO PUNCH
                if (Input.GetButtonDown("Fire1"))
                {

                    TransitionToState(PlayerState.PUNCH);

                }

                if (Input.GetButtonUp("Fire1"))
                {

                    TransitionToState(PlayerState.IDLE);

                }
                //TO TAUNT
                if (Input.GetKeyDown(KeyCode.C))
                {
                    TransitionToState(PlayerState.TAUNT);
                }

                if (Input.GetKeyUp(KeyCode.C))
                {
                    TransitionToState(PlayerState.IDLE);
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

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMP);
                }

                // TO PUNCH
                if (Input.GetButtonDown("Fire1"))
                {

                    TransitionToState(PlayerState.PUNCH);

                }

                if (Input.GetButtonUp("Fire1"))
                {

                    TransitionToState(PlayerState.IDLE);

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

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMP);
                }

                // TO PUNCH
                if (Input.GetButtonDown("Fire1"))
                {

                    TransitionToState(PlayerState.PUNCH);

                }

                if (Input.GetButtonUp("Fire1"))
                {

                    TransitionToState(PlayerState.IDLE);

                }

                

                break;

            case PlayerState.PUNCH:


                break;

            case PlayerState.JUMP:
                if (jumpTimer < jumpDuration)
                {
                    jumpTimer += Time.deltaTime;

                    float y = _jumpCurve.Evaluate(jumpTimer / jumpDuration);

                    graphics.localPosition = new Vector3(graphics.localPosition.x, y * jumpHeight, graphics.localPosition.z);

                }
                else
                {
                    jumpTimer = 0f;
                    TransitionToState(PlayerState.IDLE);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    animator.SetTrigger("AirAttack");
                }

               



                break;

            case PlayerState.DEAD:



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
            case PlayerState.DEAD:
                
                rb2D.velocity = Vector2.zero;

                break;
            case PlayerState.ULTIMATE:
                
                rb2D.velocity = Vector2.zero;

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
                graphics.localPosition = Vector3.zero;
                break;

            case PlayerState.DEAD:
                animator.SetBool("isDead", false);
                StartCoroutine(ULTIMATE());
                break;
            case PlayerState.ULTIMATE:
                animator.SetBool("isReviving", false);
                break;

            default:
                break;
        }
    }

    public void TransitionToState(PlayerState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    public IEnumerator ULTIMATE()
    {

        yield return new WaitForSeconds(2);
        TransitionToState(PlayerState.ULTIMATE);

    }

    IEnumerator Punch()
    {
        yield return new WaitForSeconds(punchClip.length);
        TransitionToState(PlayerState.IDLE);
    }

    
    private void GetMoveDirection()
    {

        if (dirInput.x < 0)
        // ON ORIENTE LES GRAPHICS EN FONCTION DE LA VALEUR DE LA DERNIERE DIRECTION
        graphics.transform.eulerAngles = new Vector3(0, 180, 0);

        if (dirInput.x > 0)
            // ON ORIENTE LES GRAPHICS EN FONCTION DE LA VALEUR DE LA DERNIERE DIRECTION
            graphics.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            animator.SetTrigger("HURT");
        }
    }
}
