using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCoolerSM : MonoBehaviour
{
    public GameObject waterCoolerPrefabs;
    public WaterCoolerState currentState;
    public Animator waterCoolerAnimator;
    public bool isDestroy;

    public enum WaterCoolerState
    {
        IDLE,
        DESTROY,
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = WaterCoolerState.IDLE;
       
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
            case WaterCoolerState.IDLE:
                isDestroy = false;
                break;
            case WaterCoolerState.DESTROY:

                isDestroy=true;
                waterCoolerAnimator.SetTrigger("Destroy");
                
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case WaterCoolerState.IDLE:

                if (isDestroy)
                {
                    TransitionToState(WaterCoolerState.DESTROY);
                }

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {

    }

    public void TransitionToState(WaterCoolerState nextState)
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
            waterCoolerAnimator.SetTrigger("Destroy");

        }

    }
}
