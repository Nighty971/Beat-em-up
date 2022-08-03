using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSM : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed = 5f;
    

    public EnnemieState currentState;


    public enum EnnemieState
    {
        Run,
        Idle,
        Jump,
        Attack,

    }

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        currentState = EnnemieState.Idle;
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
            case EnnemieState.Idle:
                rb2D.velocity = Vector2.zero;
                break;

            case EnnemieState.Run:

                animator.SetBool("RUN", true);
                break;


            case EnnemieState.Jump:
                
                animator.SetBool("JUMP",true);
                break;

            case EnnemieState.Attack:
                animator.SetTrigger("ATTACK");
                break;


        }
    }


    void OnStateUpdate()
    {


        switch (currentState)
        {
            case EnnemieState.Idle:

                //A MODIFIER
                
                //if ()
                //{
                //    TransitionToState(EnnemieState.Run);
                //}

                
                //if ()
                //{
                //    TransitionToState(EnnemieState.Jump);
                //}

                break;


            case EnnemieState.Run:

                //A MODIFIER
                //to idle
                //if ()
                //{
                //    TransitionToState(EnnemieState.Idle);
                //}


                //if ()
                //{
                //    TransitionToState(EnnemieState.Jump);
                //}

                break;


            case EnnemieState.Jump:
                break;

            case EnnemieState.Attack:
                break;
        }
    }

    void OnStateFixedUpdate()
    {
        switch (currentState)
        {

            case EnnemieState.Idle:
                break;

            case EnnemieState.Run:
                //A MODIFIER
                //rb2D.velocity = dirInput.normalized * speed;
                break;

            case EnnemieState.Jump:
                break;

            case EnnemieState.Attack:
                break;

        }
    }
    void OnStateExit()
    {
        switch (currentState)
        {
            case EnnemieState.Idle:
                break;

            case EnnemieState.Run:
                animator.SetBool("RUN", false);
                break;

            case EnnemieState.Jump:
                animator.SetBool("JUMP", false);
                break;

            case EnnemieState.Attack:
                animator.SetTrigger("ATTACK");
                break;

        }
    }

    void TransitionToState(EnnemieState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }


}
