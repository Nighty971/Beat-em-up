using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesHealth : MonoBehaviour
{
    [SerializeField] DataScriptable healthData;

    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = healthData.startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
