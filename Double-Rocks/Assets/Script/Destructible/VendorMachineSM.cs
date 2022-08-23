using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorMachineSM : MonoBehaviour
{
    public GameObject vendorMachinePrefabs;
    public GameObject punchShockPrefabs;
    public GameObject punchPoint;
    public VendorMachineState currentState;
    public Animator machineAnimator;
    public bool isDestroy;

    public enum VendorMachineState
    {
        IDLE,
        DESTROY,
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = VendorMachineState.IDLE;
       
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
            case VendorMachineState.IDLE:
                isDestroy = false;
                break;
            case VendorMachineState.DESTROY:

                isDestroy=true;
                machineAnimator.SetTrigger("Destroy");
                
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case VendorMachineState.IDLE:

                if (isDestroy)
                {
                    TransitionToState(VendorMachineState.DESTROY);
                }

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {

    }

    public void TransitionToState(VendorMachineState nextState)
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
            machineAnimator.SetTrigger("Destroy");
            GameObject go = Instantiate(punchShockPrefabs, punchPoint.transform.position + punchShockPrefabs.transform.position, Quaternion.identity);
            Destroy(go, .3f);
        }

    }
}
