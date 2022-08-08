using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesHealth : MonoBehaviour
{
    //[SerializeField] DataScriptable healthData;

    [SerializeField]public float healthAmount = 10;
    public static bool isdead;

    
    // Start is called before the first frame update
    void Start()
    {
        isdead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            isdead = true;

            GetComponent<EnnemiesSM>().TransitionToState(EnnemiesSM.EnnemieState.isDead);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(5);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }

    }

    public void TakeDamage(float playerDamage)

    { 
        healthAmount -= playerDamage;
    }

}
