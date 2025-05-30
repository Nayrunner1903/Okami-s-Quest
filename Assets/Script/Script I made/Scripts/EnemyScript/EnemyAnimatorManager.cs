using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay{
    
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats =GetComponentInParent<EnemyStats>();
        }

        public void AwardSoulOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if(playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulAwardOnDeath);
                
                if(soulCountBar != null) 
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }

            }
            else
            {
                Debug.Log("cant find playerStats");
            }
        }

        private void OnAnimatorMove() 
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity /* * enemyLocomotionManager.moveSpeed */;
        }


    }//class
}//Nay