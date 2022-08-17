using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;

    
    public Image fill;
    public void SetMaxEnergy(int ernergy)
    {
        slider.maxValue = ernergy;
        slider.value = ernergy;
        
    }

    public void SetEnergy(int energy)
    {
        slider.value = energy;
        
    }
}
