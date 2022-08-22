using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public int maxEnergy = 100;
    public float currentEnergy;
    public bool isRunning;
    public EnergyBar energyBar;
    public float regainDelay = 1.5f;
    public float energyPoints = 30f;

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

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;

        }
    }
    public void RegainPlayer(float amount)
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
    public void EnergyUse(float useEnergy)
    {
        if (useEnergy > 0)
        {
            isRunning = true;
            //Perdre endurance
            currentEnergy -= useEnergy;
            energyBar.SetEnergy(currentEnergy);


        }
        if (currentEnergy <= 0)
        {
            isRunning = false;
            currentEnergy = 0;
            RegainEnergy();
        }
    }

    public IEnumerator RegainEnergy()
    {
        isRunning = false;

        yield return new WaitForSeconds(regainDelay);

        while (!isRunning)
        {

            RegainPlayer(energyPoints * Time.deltaTime);
            yield return new WaitForEndOfFrame();

        }
    }
}
