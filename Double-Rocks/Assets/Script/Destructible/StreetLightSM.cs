using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightSM : MonoBehaviour
{
    public GameObject streetLightPrefabs;
    public GameObject punchShockPrefabs;
    public GameObject punchPoint;
    public StreetLightState currentState;
    public Animator streetLightAnimator;
    public bool isDestroy;
    public bool destroyCollider;

    public enum StreetLightState
    {
        IDLE,
        DESTROY,
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = StreetLightState.IDLE;
       Collider2D col = GetComponent<Collider2D>();
        OnStateEnter();
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
            case StreetLightState.IDLE:
                isDestroy = false;
                break;
            case StreetLightState.DESTROY:

                isDestroy=true;
                streetLightAnimator.SetTrigger("Destroy");
                
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case StreetLightState.IDLE:

                if (isDestroy)
                {
                    TransitionToState(StreetLightState.DESTROY);
                }

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {

    }

    public void TransitionToState(StreetLightState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.transform.CompareTag("PunchPoint"))
        {
            isDestroy = true;
            streetLightAnimator.SetTrigger("Destroy");
            GameObject go = Instantiate(punchShockPrefabs, punchPoint.transform.position + punchShockPrefabs.transform.position, Quaternion.identity);
            Destroy(go, .3f);

            if (destroyCollider)
                GetComponent<Collider2D>().enabled = false;

        }

    }
}
