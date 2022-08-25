using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUltimate : MonoBehaviour
{
    public int maxUltimate = 100;
    public int currentUltimate;
    public UltimateBar ultimateBar;
    public int ultimatePoints = 100;
    public bool isFull;

    public static PlayerUltimate instance;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    void Start()
    {
        currentUltimate = maxUltimate;
        ultimateBar.SetMaxUltimate(maxUltimate);

    }

    void Update()
    {
        
    }
    public void RegainUltimate(int amount)
    {

        if ((currentUltimate + amount) > maxUltimate)
        {

            currentUltimate = maxUltimate;

        }

        else
        {
            currentUltimate += amount;
        }

        ultimateBar.SetUltimate(currentUltimate);
    }
    public void UltimateUse(int useUltimate)
    {
        
        if (useUltimate == 100)
        {
            isFull = true;
            //Utiliser Ultimate
            currentUltimate -= useUltimate;
            ultimateBar.SetUltimate(currentUltimate);


        }
        if (currentUltimate <= 0)
        {
            isFull=false;
            currentUltimate = 0;
           
        }
    }
}
