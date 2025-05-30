using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public StrafeState strafeState;
        
        public override State Tick(EnemyManager enemyManager , EnemyStats enemyStats , EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.currentTarget == null)  // Prevent null reference error
                return this;

            if(enemyManager.isInteracting)
                return this;

            if(enemyManager.isPreformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical" , 0 ,0.1f , Time.deltaTime);  
            }
                

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance( enemyManager.currentTarget.transform.position , enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if(distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical",1,0.1f,Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager);
            

            if(distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            else if(distanceFromTarget > enemyManager.maximumStrafeRange)
            {
                return strafeState;
            }
            else
            {
                return this;
            }
            return this;


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


        public bool RandomToStrafe()
        {
            Random.InitState(System.DateTime.Now.Millisecond); // Initialize with current time to change seed
            int randomToStrafe = Random.Range(0,9);
            if(randomToStrafe == 0)
            {
                Debug.Log("0");
                return true;
            }
            else
            {
                Debug.Log(randomToStrafe);
                return false;
            }
        }
        



    }//class
}//Nay