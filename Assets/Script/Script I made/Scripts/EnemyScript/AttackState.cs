using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public StrafeState strafeState;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        bool isComboing = false;

        public override State Tick(EnemyManager enemyManager , EnemyStats enemyStats , EnemyAnimatorManager enemyAnimatorManager)
        {
            if(enemyManager.isInteracting)
                return this;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            HandleRotateTowardsTarget(enemyManager);

            if(enemyManager.isPreformingAction && isComboing == false)
            {
                return combatStanceState;
            }
            else if(isComboing)
            {
                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                isComboing = false;
            }
                


            if (currentAttack != null)
            {
                // If we are too close to the enemy to perform the current attack, get a new attack
                if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                // If we are close enough to attack, then let us proceed
                else if (distanceFromTarget < currentAttack.maximumAttackAngle)
                {
                    // If our enemy is within our attack's viewable angle, we attack
                    if (viewableAngle <= currentAttack.maximumAttackAngle &&
                        viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                        {
                            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPreformingAction = true;

                            if(currentAttack.canCombo)
                            {
                                currentAttack = currentAttack.comboAction;
                                Debug.Log("Do the Combo");
                                return this;
                            }
                            else
                            {
                                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                currentAttack = null;

                                if(distanceFromTarget > enemyManager.maximumStrafeRange)
                                {
                                    Debug.Log("EnterStrafeState From AttackState");
                                    return strafeState;
                                }
                                else
                                {
                                    
                                }
                                return combatStanceState;
                            }                            
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }
            return combatStanceState;


        }//Tick




        #region Attacks

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position , enemyManager.transform.position);

            int maxScore = 0;

            for( int i = 0 ; i< enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if(distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                    {
                        if(viewableAngle <= enemyAttackAction.maximumAttackAngle
                            && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                            {
                                maxScore += enemyAttackAction.attackScore;
                            }
                    }
            }
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore =0;

            for (int i= 0; i< enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if(distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                    {
                        if(viewableAngle <= enemyAttackAction.maximumAttackAngle
                            && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                            {
                                if(currentAttack != null) 
                                    return;

                                temporaryScore += enemyAttackAction.attackScore;

                                if(temporaryScore > randomValue)
                                {
                                    currentAttack = enemyAttackAction;
                                }
                            }
                    }
            }
        }//GetNewAttack

        #endregion


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
}//NAy