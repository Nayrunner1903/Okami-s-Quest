using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay{
public class PlayerStats : CharacterStats
{
    
    

    /*public int staminaLevel = 10;
    public int maxStamina;
    public int currentStamina;
    public int staminaHeal = 1;*/

    PlayerManager playerManager;

    public float rollStaminaUsage;

    public HealthBar healthBar;
    public FocusPointBar focusPointBar;
    public StaminaBar staminaBar;
    public SoulCountBar soulCountBar;

    PlayerAnimatorManager animatorHandler;

    public float staminaRegenAmount = 5f;
    public float staminaRegenTimer = 0f;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        soulCountBar  = FindObjectOfType<SoulCountBar>();
        focusPointBar = FindObjectOfType<FocusPointBar>();
        animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        rollStaminaUsage = (maxStamina*30)/100;

        maxFocusPoint = SetMaxFocusPointFromFocusPointLevel();
        currentFocusPoint = maxFocusPoint;
        focusPointBar.SetMaxFocusPoint(maxFocusPoint);
        focusPointBar.SetCurrentFocusPoint(currentFocusPoint);

        if(soulCountBar != null)
        {
            soulCountBar.SetSoulCountText(soulCount);
        }
    }


    public int SetMaxHealthFromHealthLevel()
    {
        //need to balance this
        maxHealth = healthLevel * 10;
        currentHealth = maxHealth;
        return maxHealth;
    }


    public float SetMaxStaminaFromStaminaLevel()
    {
        //need to balance this
        maxStamina = staminaLevel * 10;
        currentStamina = maxStamina;
        return maxStamina;
    }
    public int SetMaxFocusPointFromFocusPointLevel()
    {
        //need to balance this
        maxFocusPoint = focusPointLevel * 10;
        currentFocusPoint = maxFocusPoint;
        return maxFocusPoint;
    }



    public void TakeDamage(int damage , string damageAnimation = "Damage_01")
    {
        if(playerManager.isInvulerable)
        return;

        Debug.Log("player Take damage" + damage);
        
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animatorHandler.PlayTargetAnimation(damageAnimation,true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Dead_01", true);
            //HandleDEATH
        }
    }


    public void TakeStamina(float damage)
    {
        currentStamina = currentStamina - damage;
        if(currentStamina < 0 )
        {
            currentStamina = 0;
        }
        staminaBar.SetCurrentStamina(currentStamina);

        //StartStaminaRegen();
    }


    public void RegenerateStamina()
    {
        if(playerManager.isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else
        {
            staminaRegenTimer += Time.deltaTime;

            if(currentStamina < maxStamina && staminaRegenTimer > 3f)
            {
                currentStamina += staminaRegenAmount * Time.deltaTime  ;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
            
        }
        
    }

        /*private void StartStaminaRegen() {
            if (staminaRegenCoroutine != null) {
                StopCoroutine(staminaRegenCoroutine);
            }
            staminaRegenCoroutine = StartCoroutine(RegenerateStamina());
        }

        private IEnumerator RegenerateStamina() {
            yield return new WaitForSeconds(1.5f);

            while (currentStamina < maxStamina) {
                currentStamina += staminaHeal;
                currentStamina = Mathf.Min(currentStamina, maxStamina); 
                staminaBar.SetCurrentStamina(currentStamina);
                yield return new WaitForSeconds(1.5f); 
            }

            staminaRegenCoroutine = null; 
        }*/

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;
            if( currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoint)
        {
            currentFocusPoint = currentFocusPoint - focusPoint;

            if(currentFocusPoint < 0)
            {
                currentFocusPoint = 0;
            }
            
            focusPointBar.SetCurrentFocusPoint(currentFocusPoint);
        }

        public void AddSouls(int souls)
        {
            soulCount = soulCount + souls;
        }


}//class
}//Nay