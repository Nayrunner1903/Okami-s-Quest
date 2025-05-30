using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {

            if(enemyManager.isInteracting)
                return this;


            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            // potentially circle player or walk around them

            if(enemyManager.isPreformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical" , 0 ,0.1f , Time.deltaTime);  
            }
                
            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }

            
            else if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }//Tick



        

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPreformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0 ;
                direction.Normalize();
                
                if( direction == Vector3.zero )
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation , enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDir = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation , enemyManager.navmeshAgent.transform.rotation
                                                        , enemyManager.rotationSpeed / Time.deltaTime);
            }

            /*navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;*/

        }//HandleRotateTowardsTarget 





    }//class

}//Nay
