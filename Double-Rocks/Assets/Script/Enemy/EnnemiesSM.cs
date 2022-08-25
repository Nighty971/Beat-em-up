using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSM : MonoBehaviour
{
    [SerializeField] float checkRadius;
    [SerializeField] float speed = 5f;
    [SerializeField] float attackRange /*= 5f*/;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] private float waitingTimeBeforeAttack;
    [SerializeField] private float limitNearTarget;


    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer shadow;
    [SerializeField] AnimationClip deathClip;
    [SerializeField] Collider2D punchCollider;

    [SerializeField]
    private AudioClip _punchAudioClip;
    private AudioSource _audioSource;


    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject ennemiesHitPoint;

    public GameObject punchShockPrefabs;
    public GameObject punchPoint;

    EnnemiesHealth ennemiesHealth;

    public LayerMask whatIsPlayer;
    public EnnemieState currentState;
    private Transform target;

    
    private Vector3 dir;
    private Vector2 movement;

    
    public bool isInChaseRange;
    public bool isInAttackRange;
    private bool Run = false;

    [SerializeField] float attackTimer;
    private float attackDuration;
    
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
        // test son
        _audioSource = GetComponent<AudioSource>();

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
                attackTimer = 0f;
                animator.SetBool("IsRunning", false);
                rb2D.velocity = Vector2.zero;
                break;
            

            case EnnemieState.RUN:
                animator.SetBool("IsRunning", true);
                break;

            

            case EnnemieState.ATTACK:
                _audioSource.clip = _punchAudioClip ;
                attackDuration = 1f;
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("ATTACK");
                hitbox.SetActive(true);
                //test son
                _audioSource.Play();

                rb2D.velocity = Vector2.zero;
                break;

            case EnnemieState.DEAD:
                animator.SetBool("isDead", true);
                rb2D.velocity = Vector2.zero;
                Score.instance.AddPoint(300);
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                
                punchCollider.enabled = false;
                StartCoroutine(DEAD());
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

                /*
                if (dir.magnitude != 0)
                {
                    TransitionToState(EnnemieState.RUN);

                }
                */

                if (Run && !IsTargetNearLimit())
                {
                    TransitionToState(EnnemieState.RUN);
                }


                if (isInAttackRange && IsTargetNearLimit())
                {
                    attackTimer += Time.deltaTime;

                    if (attackTimer >= waitingTimeBeforeAttack)
                    {
                        TransitionToState(EnnemieState.ATTACK);
                    }
                    
                }
                

                break;


            case EnnemieState.RUN:

                /*
                if (isInAttackRange)
                {
                    TransitionToState(EnnemieState.ATTACK);
                }
                */
                /*
                if (!Run)
                {
                    TransitionToState(EnnemieState.IDLE);
                }
                */
                
                if (!isInChaseRange && isInAttackRange)
                {
                    //MoveCharacter(movement);
                    TransitionToState(EnnemieState.IDLE);
                }
                
                
                MoveCharacter(movement);
                if (IsTargetNearLimit())
                {
                    TransitionToState(EnnemieState.IDLE);
                }


                break;
                

            case EnnemieState.ATTACK:

                attackDuration += Time.deltaTime;
                
                if (attackDuration >= attackDelay)
                {
                    TransitionToState(EnnemieState.IDLE);
                }

                if (Run && !IsTargetNearLimit())
                {
                    TransitionToState(EnnemieState.IDLE);
                }
                
                break;


            default:
                break;
        }
    }
    
    private void MoveCharacter(Vector2 dir)
    {
        //rb2D.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
        transform.position = Vector2.MoveTowards(transform.position, target.position,  speed * Time.deltaTime);
    }
    

    bool IsTargetNearLimit()
    {
        return Vector2.Distance(transform.position, target.position) < limitNearTarget;
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

    public IEnumerator DEAD()
    {
        
        yield return new WaitForSeconds(deathClip.length);
        shadow.enabled = false;
        
        Destroy(gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("PunchPoint"))
        {
            GameObject go = Instantiate(punchShockPrefabs, punchPoint.transform.position + punchShockPrefabs.transform.position, Quaternion.identity);
            Destroy(go, .3f);
            animator.SetTrigger("Hurt");
            ennemiesHealth.TakeDamage(10);
        }

        if (collision.CompareTag("UltimateZone"))
        {
            ennemiesHealth.TakeDamage(100);
            TransitionToState(currentState);
        }
    }

}
