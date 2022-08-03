using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    [SerializeField] Animator animator;

    
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float HP = 6f;

    public PlayerState currentState;
    private Vector2 dirInput;


    public enum PlayerState
    {
        Run,
        Idle,
        Sprint,
        Jump,
        Attack,
        
    }

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        currentState = PlayerState.Idle;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnStateEnter()
    {

        switch (currentState)
        {
            case PlayerState.Idle:
                rb2D.velocity = Vector2.zero;
                break;

            case PlayerState.Run:

                animator.SetBool("RUN", true);
                break;

            case PlayerState.Sprint:
                animator.SetBool("SPRINT", true);
                break;

            case PlayerState.Jump:
                break;

            case PlayerState.Attack:
                break;


        }
    }


    void OnStateUpdate()
    {


        switch (currentState)
        {
            case PlayerState.Idle:
                //TO RUN
                if (dirInput.magnitude != 0)
                {
                    TransitionToState(Input.GetKey(KeyCode.LeftShift) ? PlayerState.Sprint : PlayerState.Run);
                }

                ///to roll
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.Jump);
                }

                break;


            case PlayerState.Run:

                //to idle
                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.Idle);
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.Sprint);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.Jump);
                }

                break;


            case PlayerState.Sprint:

                //to sprint
                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.Idle);
                }

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.Run);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.Jump);
                }
                break;

            case PlayerState.Attack:
                break;

            case PlayerState.Jump:
                break;

        }
    }

    void OnStateFixedUpdate()
    {
        switch (currentState)
        {

            case PlayerState.Idle:
                break;

            case PlayerState.Run:
                rb2D.velocity = dirInput.normalized * speed;
                break;

            case PlayerState.Sprint:
                rb2D.velocity = dirInput.normalized * sprintSpeed;
                break;

            case PlayerState.Attack:
                break;

            case PlayerState.Jump:
                break;

        }
    }
    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Run:
                animator.SetBool("RUN", false);
                break;

            case PlayerState.Sprint:
                animator.SetBool("SPRINT", false);
                break;

            case PlayerState.Attack:
                animator.SetTrigger("ATTACK");
                break;

            case PlayerState.Jump:
                animator.SetBool("JUMP", false);
                break;

        }
    }

    void TransitionToState(PlayerState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }


}
