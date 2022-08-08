using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invicibilityFlashDelay = 0.2f;
    public float invicibilityTimeAfterHit = 1.5f;
    
    public bool isInvincible = false;
    public SpriteRenderer graphics;
    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void Update()
    {
        //Tester la barre de vie
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    TakeDamage(20);
        //}
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible && currentHealth > 0)
        {
            //Prendre des dommages
            currentHealth -= damage;
            if(currentHealth < 0)
                currentHealth = 0;
            healthBar.SetHealth(currentHealth);
            isInvincible = true;
            StartCoroutine(InvicibilityFlash());
            StartCoroutine(HandleInvicibilityDelay());
        }

        if(currentHealth <= 0)
        {
            
            GetComponent<PlayerSM>().TransitionToState(PlayerSM.PlayerState.DEAD);
            
        }
        
    }

    public IEnumerator InvicibilityFlash()
    {
        while (isInvincible)
        {
            graphics.color = new Color(1f,1f,1f,0f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
            graphics.color = new Color(1f,1f,1f,1f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
        }
    }
    
    public IEnumerator HandleInvicibilityDelay()
    {
        yield return new WaitForSeconds(invicibilityTimeAfterHit);
        isInvincible=false;
    }
}
