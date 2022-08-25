using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateFill : MonoBehaviour
{
    public int ultimatePoints;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Rendre de la vie
            PlayerUltimate.instance.RegainUltimate(ultimatePoints);
            Destroy(gameObject);
        }
    }
}
