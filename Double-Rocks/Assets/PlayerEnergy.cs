using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public int maxEnergy = 100;
    public int currentEnergy;

    public EnergyBar energyBar;
    void Start()
    {
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);

    }

    void Update()
    {
        //Tester la barre de vie
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    TakeDamage(20);
        //}
    }

    void EnergyUse(int useEnergy)
    {
        //Prendre des dommages
        currentEnergy -= useEnergy;
        energyBar.SetEnergy(currentEnergy);
    }
}
