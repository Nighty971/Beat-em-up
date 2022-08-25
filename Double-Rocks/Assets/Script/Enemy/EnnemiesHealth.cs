using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesHealth : MonoBehaviour
{
    //[SerializeField] DataScriptable healthData;

    [SerializeField]public float healthAmount = 10;
    public bool isdead;

    private ItemDrop getItem;
    public static EnnemiesHealth instance;

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
        isdead = false;
        getItem = GetComponent<ItemDrop>();
    }

    // Update is called once per frame
    void Update()
    {
        

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
        if (healthAmount <= 0 && !isdead)
        {
            isdead = true;

            GetComponent<EnnemiesSM>().TransitionToState(EnnemiesSM.EnnemieState.DEAD);


            //test
            if (getItem != null)
            {
                getItem.DropItem();

            }

            //fin

        }

    }

}
