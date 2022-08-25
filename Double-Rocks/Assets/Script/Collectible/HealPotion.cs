using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : MonoBehaviour
{
    public bool isDestroy;
    public SpriteRenderer graphics;
    public float destroyFlashDelay = 0.2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDestroy = true;
            Score.instance.AddPoint(100);
            Destroy(gameObject);
        }

        else
        {
            isDestroy = false;
            StartCoroutine(TimeBeforeDestroy());
        }
    }

    IEnumerator TimeBeforeDestroy()
    {
        if (!isDestroy)
        {

            yield return new WaitForSeconds(5);

            Destroy(gameObject, 3);

            StartCoroutine(DestroyFlash());


        }
    }

    public IEnumerator DestroyFlash()
    {
        while (true)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(destroyFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(destroyFlashDelay);
        }
    }
}
