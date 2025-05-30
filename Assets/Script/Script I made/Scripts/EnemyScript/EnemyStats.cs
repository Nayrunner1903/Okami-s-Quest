using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
public class EnemyStats : CharacterStats
{
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyManager enemyManager;
    public UIEnemyHealthBar enemyHealthBar;
    
    /*public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;*/

    public int soulAwardOnDeath = 50;

    Animator animator;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;

        enemyHealthBar.SetMaxHealth(maxHealth);
    }


    private int SetMaxHealthFromHealthLevel()
    {
        //need to balance this
        maxHealth = healthLevel * 10;
        return maxHealth;
    }


    public void TakeDamage(int damage)
    {
        if(isDead)
            return;


        currentHealth = currentHealth - damage;

        enemyHealthBar.SetHealth(currentHealth);

        Debug.Log("Enemy HP Left " + currentHealth + "by take damage " + damage);

        animator.Play("Damage_01");
        //enemyAnimatorManager.PlayTargetAnimation("Damage_01",true);

        if(currentHealth <= 0)
        {
            HandleDeath();
        }
    }


    public void HandleDeath()
    {
        currentHealth = 0;
        //enemyManager.isInteracting = true;
        animator.Play("Dead_01");
        //enemyAnimatorManager.PlayTargetAnimation("Dead_01",true);
        isDead = true;
        
        //Add Soul (do it already in animation dead)
        //enemyAnimatorManager.AwardSoulOnDeath();


    }//HandleDeath




















}//class
}//Nay
