using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public int maxEnergy = 100;
    public int currentEnergy;
    public bool isRunning;
    public EnergyBar energyBar;
    public float regainDelay = 1.5f;
    public int energyPoints;

    public static PlayerEnergy instance;
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
        currentEnergy = maxEnergy;
        energyBar.SetMaxEnergy(maxEnergy);

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;

            if (Input.GetKey(KeyCode.LeftShift) && isRunning)
            {
                
                EnergyUse(1);

            }
           

            
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            StartCoroutine(RegainEnergy());
        }
    }
    public void RegainPlayer(int amount)
    {

        if ((currentEnergy + amount) > maxEnergy)
        {

            currentEnergy = maxEnergy;

        }

        else
        {
            currentEnergy += amount;
        }

        energyBar.SetEnergy(currentEnergy);
    }
    void EnergyUse(int useEnergy)
    {
        //Prendre des dommages
        currentEnergy -= useEnergy;
        energyBar.SetEnergy(currentEnergy);
    }

    public IEnumerator RegainEnergy()
    {
        while (!isRunning)
        {
            
            yield return new WaitForSeconds(regainDelay);
            PlayerEnergy.instance.RegainPlayer(energyPoints);

        }
    }
}
