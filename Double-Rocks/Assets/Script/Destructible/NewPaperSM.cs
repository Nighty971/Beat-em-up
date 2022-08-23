using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPaperSM : MonoBehaviour
{
    public GameObject vendorMachinePrefabs;
    public GameObject punchShockPrefabs;
    public GameObject punchPoint;
    public NewPaperState currentState;
    public Animator newPaperAnimator;
    public bool isDestroy;

    public enum NewPaperState
    {
        IDLE,
        DESTROY,
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = NewPaperState.IDLE;
       
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
            case NewPaperState.IDLE:
                isDestroy = false;
                break;
            case NewPaperState.DESTROY:

                isDestroy=true;
                newPaperAnimator.SetTrigger("Destroy");
                
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case NewPaperState.IDLE:

                if (isDestroy)
                {
                    TransitionToState(NewPaperState.DESTROY);
                }

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {

    }

    public void TransitionToState(NewPaperState nextState)
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
            newPaperAnimator.SetTrigger("Destroy");
            GameObject go = Instantiate(punchShockPrefabs, punchPoint.transform.position + punchShockPrefabs.transform.position, Quaternion.identity);
            Destroy(go, .3f);
        }

    }
}
