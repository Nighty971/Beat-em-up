using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesHealth : MonoBehaviour
{
    [SerializeField] DataScriptable healthData;

    private int health = 10;
    public static bool dead;
    // Start is called before the first frame update
    void Start()
    {
       // health = healthData.startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage()

    {
        //if(health >=0)
    }

}
