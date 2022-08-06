using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PunchPoint"))
        {
            
            Destroy(transform.parent.gameObject);
        }

       
    }
}
