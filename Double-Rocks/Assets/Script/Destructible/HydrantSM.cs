using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrantSM : MonoBehaviour
{
    public GameObject hydrantPrefabs;
    public GameObject punchShockPrefabs;
    public GameObject punchPoint;
    public HydrantState currentState;
    public Animator hydrantAnimator;
    public bool isDestroy;

    public enum HydrantState
    {
        IDLE,
        DESTROY,
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = HydrantState.IDLE;
       
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
            case HydrantState.IDLE:
                isDestroy = false;
                break;
            case HydrantState.DESTROY:

                isDestroy=true;
                hydrantAnimator.SetTrigger("Destroy");
                
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case HydrantState.IDLE:

                if (isDestroy)
                {
                    TransitionToState(HydrantState.DESTROY);
                }

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {

    }

    public void TransitionToState(HydrantState nextState)
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
            hydrantAnimator.SetTrigger("Destroy");
            GameObject go = Instantiate(punchShockPrefabs, punchPoint.transform.position + punchShockPrefabs.transform.position, Quaternion.identity);
            Destroy(go, .3f);

        }

    }
}
