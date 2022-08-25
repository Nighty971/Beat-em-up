using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invicibilityFlashDelay = 0.2f;
    public float invicibilityTimeAfterHit = 1.5f;

    public float destroyDelay = 1.5f;
    
    public bool isInvincible = false;
    public SpriteRenderer graphics;
    public HealthBar healthBar;

    public static PlayerHealth instance;

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
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void Update()
    {
        //Tester la barre de vie
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(20);
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Instantiate(gameObject);
        //    GetComponent<PlayerSM>().TransitionToState(PlayerSM.PlayerState.ULTIMATE);
        //}
    }



    public void HealPlayer(int amount)
    {

        if ((currentHealth + amount) > maxHealth)
        {

            currentHealth = maxHealth;

        }

        else
        {
            currentHealth += amount;
        }

        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int ennemyDamage)
    {
        if (!isInvincible && currentHealth > 0)
        {
            //Prendre des dommages
            currentHealth -= ennemyDamage;
            
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(DelayBeforeDestroy()); 
                return;
            }
            isInvincible = true;
            StartCoroutine(InvicibilityFlash());
            StartCoroutine(HandleInvicibilityDelay());
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

    public IEnumerator DelayBeforeDestroy()
    {
        GetComponent<PlayerSM>().TransitionToState(PlayerSM.PlayerState.DEAD);
        yield return new WaitForSeconds(destroyDelay);
        

    }
}
