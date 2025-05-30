using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class StrafeState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;


        public override State Tick(EnemyManager enemyManager , EnemyStats enemyStats , EnemyAnimatorManager enemyAnimatorManager)
        {

            if (enemyManager.currentTarget == null)  // Prevent null reference error
                return this;

            if(enemyManager.isInteracting)
                return this ;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);


            HandleRotateTowardsTarget(enemyManager);

            if(distanceFromTarget > enemyManager.maximumStrafeRange)
            {
                Debug.Log("distanceFromTarget = "+ distanceFromTarget + "Enter pursueTargetState From StrafeState");
                return pursueTargetState;
            }
            else if(distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                Debug.Log("distanceFromTarget = "+ distanceFromTarget + "Enter combatStanceState From StrafeState");
                return combatStanceState;
            }
            else
            {
                Debug.Log("Strafe");
                enemyAnimatorManager.anim.SetFloat("Vertical" , randomStrafeAnimation() ,0.1f , Time.deltaTime);  
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



        public float randomStrafeAnimation()
        {
            // Generate a random integer between 0 and 3
            Random.InitState(System.DateTime.Now.Millisecond);// Initialize with current time to change seed
            int randomIndex = Random.Range(0, 4);

            // Map the random index to one of the desired values
            float[] values = { 2.25f, 2.5f, 2.75f, 3f };
            float randomValue = values[randomIndex];

            Debug.Log("Random Value: " + randomValue);
            return randomValue;
        }
        








    }//Class
}//Nay