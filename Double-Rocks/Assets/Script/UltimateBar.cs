using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBar : MonoBehaviour
{
    public Slider slider;

    
    public Image fill;
    public void SetMaxUltimate(int ultimate)
    {
        slider.maxValue = ultimate;
        slider.value = ultimate;
        
    }

    public void SetUltimate(float ultimate)
    {
        slider.value = ultimate;
        
    }
}
