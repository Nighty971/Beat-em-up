using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieAI : MonoBehaviour
{
    [SerializeField]float speed = 5f;
    [SerializeField] float checkRadius;
    [SerializeField] float attackRadius;

    public int damageOnCollision = 20;
    private bool shouldRotate;

    public LayerMask whatIsPlayer;

    private Transform target;
    Rigidbody2D rb2D;
    [SerializeField]Animator animator;
    private Vector2 movement;
    private Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsRunning", isInChaseRange);

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;

        if (shouldRotate)
        {
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);
        }


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

    private void MoveCharacter(Vector2 dir)
    {
        rb2D.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageOnCollision);
        }

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PunchPoint"))
        {
            animator.SetTrigger("HURT");
        }
    }

}
