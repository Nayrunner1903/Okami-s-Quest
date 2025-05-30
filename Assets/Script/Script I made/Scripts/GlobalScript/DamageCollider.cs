using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;

    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider= GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }


    public void EnableDamageCollider()
    {
        //Debug.Log("EnableDamageCollider");
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        //Debug.Log("DisableDamageCollider");
        damageCollider.enabled = false;
    }


    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("OnTriggerEnter triggered with: " + collision.tag);


        if(collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            CharactorManager charactorManager = collision.GetComponent<CharactorManager>();
            BlockingColider shield = collision.transform.GetComponentInChildren<BlockingColider>();

            if(charactorManager != null)
            {
                if(charactorManager.isBlocking && shield != null)
                {
                    float phyDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorbtion) /100;

                    if(playerStats!= null)
                    {
                        playerStats.TakeDamage(Mathf.RoundToInt(phyDamageAfterBlock) , "Blocking");
                        return;
                    }
                }
            }
            else if (charactorManager == null)
            {
                Debug.Log("charactorManager = null");
            }


            if(playerStats != null)
            {
                Debug.Log("deal Damage to player");
                playerStats.TakeDamage(currentWeaponDamage);
            }
            else if(playerStats == null)
            {
                Debug.Log("playerStats = null");
            }
        }
        
        if(collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if(enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
            }
        }




    }




}//class
}//Nay