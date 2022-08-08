using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float limitNearTarget = 1;
    [SerializeField] float waitingTimeBeforeAttack = 1f;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private GameObject hitbox;


    private bool playerDetected = false;
    private Transform moveTarget;

    public EnemyState currentState;
    private float attackTimer;

    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Dead,
    }
    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(EnemyState.Idle);
        moveTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                attackTimer = 0f;
                break;
            case EnemyState.Walk:
                animator.SetBool("isWalking", true);
                break;
            case EnemyState.Attack:
                attackTimer = 0f;
                hitbox.SetActive(true);
                animator.SetBool("isAttacking", true);
                break;
            case EnemyState.Dead:
                
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                if (playerDetected && !IsTargetNearLimit())
                {
                    TransitionToState(EnemyState.Walk);
                }

                if (IsTargetNearLimit())
                {
                    TransitionToState(EnemyState.Attack);
                }
                {
                    TransitionToState(EnemyState.Attack);
                }

                break;
            case EnemyState.Walk:

                transform.position = Vector2.MoveTowards(transform.position, moveTarget.position, Time.deltaTime);

                if (IsTargetNearLimit())
                {
                    TransitionToState(EnemyState.Idle);
                }

                if (!playerDetected)
                {
                    TransitionToState(EnemyState.Idle);
                }

                break;
            case EnemyState.Attack:

                attackTimer += Time.deltaTime;
                if (attackTimer >= attackDuration)
                {
                    TransitionToState(EnemyState.Idle);
                }
                


                break;
            case EnemyState.Dead:
                
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                
                break;
            case EnemyState.Walk:
                animator.SetBool("IsRunning", false);
                break;
            case EnemyState.Attack:
                hitbox.SetActive(false);
                animator.SetBool("IsAttacking", false);
                break;
            case EnemyState.Dead:
                
                break;
        }
    }

    void TransitionToState ( EnemyState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    void PlayerDetected()
    {
        playerDetected = true;
    }

    void PlayerLost()
    {
        playerDetected = false;
    }

    

    bool IsTargetNearLimit()
    {
        return Vector2.Distance(transform.position, moveTarget.position) < limitNearTarget;
    }
}




