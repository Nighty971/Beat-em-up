using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSM : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float checkRadius;
    [SerializeField] float speed = 5f;
    [SerializeField] float attackRange /*= 5f*/;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] GameObject hitbox;
    public GameObject punchShockPrefabs;
    public GameObject punchPoint;
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject ennemiesHitPoint;


    EnnemiesHealth ennemiesHealth;

    public LayerMask whatIsPlayer;
    public EnnemieState currentState;
    private Transform target;

    
    private Vector3 dir;
    private Vector2 movement;

    
    public bool isInChaseRange;
    public bool isInAttackRange;
    private bool Run = false;


    private float attackTime;
    
    public enum EnnemieState
    {
        RUN,
        IDLE,
        ATTACK,
        DEAD,
        HURT,
    }

    
    Rigidbody2D rb2D;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        ennemiesHealth = GetComponent<EnnemiesHealth>();
        currentState = EnnemieState.IDLE;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        OnStateEnter();

    }

    
    
    // Update is called once per frame
    void Update()
    {


        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        
        Run = isInChaseRange && !isInAttackRange;
        
        animator.SetBool("IsRunning" , Run);

        
        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;


        if (dir.x > 0)
        {
            graphics.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            graphics.transform.eulerAngles = new Vector3(0, 180, 0);
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
            case EnnemieState.IDLE:
                //animator.SetBool("IsRunning", false);
                rb2D.velocity = Vector2.zero;
                break;
            

            case EnnemieState.RUN:
                animator.SetBool("IsRunning", true);
                break;

            

            case EnnemieState.ATTACK:
                attackTime = 1f;
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("ATTACK");
                hitbox.SetActive(true);
                rb2D.velocity = Vector2.zero;
                break;

            case EnnemieState.DEAD:
                
                animator.SetBool("isDead", true);
                rb2D.velocity = Vector2.zero;
                Destroy(gameObject, 1.5f);
                break;
            case EnnemieState.HURT:
                animator.SetTrigger("Hurt");
                rb2D.velocity = Vector2.zero;
                break;

            default:
                break;
        }
    }


    void OnStateUpdate()
    {


        switch (currentState)
        {
            case EnnemieState.IDLE:


                if (dir.magnitude != 0)
                {
                    TransitionToState(EnnemieState.RUN);

                }

                if (Run)
                {
                    TransitionToState(EnnemieState.RUN);
                }


                if (isInAttackRange)
                {
                    TransitionToState(EnnemieState.ATTACK);
                }
                

                break;


            case EnnemieState.RUN:

                MoveCharacter(movement);
                
                if (isInAttackRange)
                {
                    TransitionToState(EnnemieState.ATTACK);
                }


                if (! Run)
                {
                    TransitionToState(EnnemieState.IDLE);
                }
                
                

                break;
                

            case EnnemieState.ATTACK:

                attackTime += Time.deltaTime;
                
                if (attackTime >= attackDelay)
                {
                    TransitionToState(EnnemieState.IDLE);
                }
                break;


            default:
                break;
        }
    }

    
    void OnStateExit()
    {
        switch (currentState)
        {
            case EnnemieState.IDLE:
                break;

            case EnnemieState.RUN:
                //animator.SetBool("IsRunning", false);
                break;


            case EnnemieState.ATTACK:
                break;

            case EnnemieState.DEAD:

                rb2D.velocity = Vector2.zero;

                break;
        }
    }

    public void TransitionToState(EnnemieState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb2D.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

            Debug.Log("Punch " + collision.name);

        if (collision.CompareTag("PunchPoint"))
        {
            GameObject go = Instantiate(punchShockPrefabs, punchPoint.transform.position + punchShockPrefabs.transform.position, Quaternion.identity);
            Destroy(go, .3f);
            animator.SetTrigger("Hurt");
            ennemiesHealth.TakeDamage(20f);
        }
    }

}
