using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSM : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float checkRadius;
    [SerializeField] float attackRadius;
    [SerializeField] float speed = 5f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float attackDelay = 1f;

    public LayerMask whatIsPlayer;
    public EnnemieState currentState;
    private Transform target;
    private Vector3 dir;
    private Vector2 movement;

    public bool shouldRotate;
    public bool isInChaseRange;
    public bool isInAttackRange;
    private bool Run = false;

    private float attackTime;
    
    public enum EnnemieState
    {
        IsRunning,
        Idle,
        Attack,

    }

    Rigidbody2D rb2D;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        currentState = EnnemieState.Idle;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        
        Run = isInChaseRange && !isInAttackRange;
        Debug.Log(Run);
        animator.SetBool("IsRunning" , Run);

        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;

        if (shouldRotate)
        {
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);
        }


        OnStateUpdate();
    }

    private void FixedUpdate()
    {
        if (isInChaseRange && isInAttackRange)
        {
            MoveCharacter(movement);

        }

        if (isInAttackRange)
        {
            rb2D.velocity = Vector2.zero;
        }
    }
    
    
    void OnStateEnter()
    {

        switch (currentState)
        {
            case EnnemieState.Idle:
                //animator.SetBool("IsRunning", false);
                rb2D.velocity = Vector2.zero;
                break;
            

            case EnnemieState.IsRunning:
                animator.SetBool("IsRunning", true);
                break;

            

            case EnnemieState.Attack:
                Debug.Log("Attack");
                attackTime = 0f;
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("ATTACK");
                rb2D.velocity = Vector2.zero;
                break;


        }
    }


    void OnStateUpdate()
    {


        switch (currentState)
        {
            case EnnemieState.Idle:


                if (dir.magnitude != 0)
                {
                    TransitionToState(EnnemieState.IsRunning);

                }

                if (Run)
                {
                    TransitionToState(EnnemieState.IsRunning);
                }


                if (isInAttackRange)
                {
                    TransitionToState(EnnemieState.Attack);
                }
                

                break;


            case EnnemieState.IsRunning:

                MoveCharacter(movement);
                
                if (isInAttackRange)
                {
                    TransitionToState(EnnemieState.Attack);
                }


                if (! Run)
                {
                    TransitionToState(EnnemieState.Idle);
                }
                
                

                break;
                

            case EnnemieState.Attack:

                attackTime += Time.deltaTime;
                
                if (attackTime >= attackDelay)
                {
                    TransitionToState(EnnemieState.Idle);
                }


                break;
        }
    }

    /*void OnStateFixedUpdate()
    {
        switch (currentState)
        {

            case EnnemieState.Idle:
                break;

            case EnnemieState.IsRunning:
                
                break;

            case EnnemieState.Attack:
                break;

        }
    }
    */
    
    void OnStateExit()
    {
        switch (currentState)
        {
            case EnnemieState.Idle:
                break;

            case EnnemieState.IsRunning:
                //animator.SetBool("IsRunning", false);
                break;


            case EnnemieState.Attack:
                break;

        }
    }

    void TransitionToState(EnnemieState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb2D.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }


}
