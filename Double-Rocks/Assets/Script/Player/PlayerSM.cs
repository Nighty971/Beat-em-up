using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSM : MonoBehaviour
{
    [SerializeField] Transform graphics;
    [SerializeField] GameObject prefabDust;
    [SerializeField] GameObject prefabDustLand;
    [SerializeField] GameObject prefabDustDash;
    [SerializeField] GameObject prefabShockWaveL;
    [SerializeField] GameObject prefabShockWaveR;
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject ultimateZone;
    [Header("STATE")]
    public PlayerState currentState;

    [Header("ANIMATIONS")]
    [SerializeField] AnimationClip punchClip;
    [SerializeField] AnimationClip tauntClip;
    [SerializeField] AnimationClip ultimateClip;
    [SerializeField] AnimationCurve _jumpCurve;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer shadow;

    [Header("SPEED")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float jumpTimer;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpDuration = 2f;
    public Collider2D playerCollider;

    Vector2 dirInput;
    Vector2 jumpDirection;


    public PlayerHealth playerHealth;
    public PlayerUltimate playerUltimate;
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
        RESPAWN,
        ULTIMATE,
    }


    public static PlayerSM instance;


    PlayerEnergy playerEnergy;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.IDLE;
        rb2D = GetComponent<Rigidbody2D>();
        OnStateEnter();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerUltimate = GetComponent<PlayerUltimate>();


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
                hitbox.SetActive(true);
                StartCoroutine(Punch());
                break;

            case PlayerState.TAUNT:
                
                animator.SetTrigger("TAUNT");
                StartCoroutine(Taunt());
                
                break;
            case PlayerState.HURT:
                animator.SetTrigger("HURT");
                rb2D.velocity = Vector2.zero;

                break;
            case PlayerState.DEAD:
                animator.SetBool("isDead", true);
                rb2D.velocity = Vector2.zero;
                
                StartCoroutine(DEAD());
                break;

            case PlayerState.RESPAWN:
                animator.SetTrigger("Respawn");
                rb2D.velocity = Vector2.zero;
                gameObject.layer = 8;
                PlayerSM.instance.enabled = true;
                PlayerSM.instance.rb2D.bodyType = RigidbodyType2D.Dynamic;
                PlayerSM.instance.playerCollider.enabled = true; 
                PlayerSM.instance.shadow.enabled = true;
                PlayerHealth.instance.currentHealth = (PlayerHealth.instance.maxHealth);
                PlayerHealth.instance.healthBar.SetHealth(PlayerHealth.instance.currentHealth);
               
                break;
            case PlayerState.ULTIMATE:
                animator.SetTrigger("Ultimate");
                rb2D.velocity = Vector2.zero;
                ultimateZone.SetActive(true);
                GameObject go4 = Instantiate(prefabShockWaveL, shadow.transform.position + prefabShockWaveL.transform.position, prefabShockWaveL.transform.rotation, shadow.transform);
                Destroy(go4, .4f);
                GameObject go5 = Instantiate(prefabShockWaveR, shadow.transform.position + prefabShockWaveR.transform.position, prefabShockWaveR.transform.rotation, shadow.transform);
                Destroy(go5, .4f);
                StartCoroutine(Ultimate());
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
                    GameObject go = Instantiate(prefabDust, shadow.transform.position + prefabDust.transform.position, Quaternion.identity);
                    Destroy(go, .3f);
                    TransitionToState(PlayerState.JUMP);

                }
                //ULTIMATE
                if (Input.GetKeyDown(KeyCode.X))
                {

                    TransitionToState(PlayerState.ULTIMATE);


                }
                if (Input.GetKeyUp(KeyCode.X))
                {
                    TransitionToState(PlayerState.IDLE);
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
                    GameObject go3 = Instantiate(prefabDustDash, shadow.transform.position + prefabDustDash.transform.position, graphics.rotation);
                    Destroy(go3, .3f);

                }

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMP);
                    GameObject go = Instantiate(prefabDust, shadow.transform.position + prefabDust.transform.position, Quaternion.identity);
                    Destroy(go, .3f);
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
                //ULTIMATE
                if (Input.GetKeyDown(KeyCode.X))
                {

                    TransitionToState(PlayerState.ULTIMATE);


                }
                if (Input.GetKeyUp(KeyCode.X))
                {
                    TransitionToState(PlayerState.IDLE);
                }



                break;

            case PlayerState.RUN:

                playerEnergy.EnergyUse(40* Time.deltaTime);

                // TO IDLE
                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                    
                }

                // TO WALK
                if (!Input.GetKey(KeyCode.LeftShift) || playerEnergy.currentEnergy <= 0)
                {
                    TransitionToState(PlayerState.WALK);
                    
                }

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMP);
                    GameObject go = Instantiate(prefabDust, shadow.transform.position + prefabDust.transform.position, Quaternion.identity);
                    Destroy(go, .3f);
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

                //ULTIMATE
                if (Input.GetKeyDown(KeyCode.X))
                {

                    TransitionToState(PlayerState.ULTIMATE);
                    

                }
                if (Input.GetKeyUp(KeyCode.X))
                {
                    TransitionToState(PlayerState.IDLE);
                }
                        

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

            case PlayerState.ULTIMATE:

                //ULTIMATE
                playerUltimate.UltimateUse(100);
                //TransitionToState(PlayerState.IDLE);

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
            case PlayerState.RESPAWN:
                
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
                StartCoroutine(playerEnergy.RegainEnergy());

                break;
            case PlayerState.PUNCH:
                hitbox.SetActive(false);


                break;

            case PlayerState.JUMP:
                graphics.localPosition = Vector3.zero;
                GameObject go2 = Instantiate(prefabDustLand, shadow.transform.position + prefabDustLand.transform.position, Quaternion.identity);
                Destroy(go2, .3f);
                break;

            case PlayerState.ULTIMATE:

                ultimateZone.SetActive(false);
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

    

    public IEnumerator DEAD()
    {
        PlayerSM.instance.enabled = false;
        PlayerSM.instance.rb2D.bodyType = RigidbodyType2D.Kinematic;
        PlayerSM.instance.playerCollider.enabled = false;
        yield return new WaitForSeconds(1.4f);
        PlayerSM.instance.shadow.enabled = false;
        yield return new WaitForSeconds(3);
        GameOverManager.instance.OnPlayerDeath();
    }

    IEnumerator Punch()
    {
        yield return new WaitForSeconds(punchClip.length);
        TransitionToState(PlayerState.IDLE);
    }
    IEnumerator Ultimate()
    {
        yield return new WaitForSeconds(ultimateClip.length);
        TransitionToState(PlayerState.IDLE);
    }
    IEnumerator Taunt()
    {
        yield return new WaitForSeconds(tauntClip.length);
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

    

    
   
}
