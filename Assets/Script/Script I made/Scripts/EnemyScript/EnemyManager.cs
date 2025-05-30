using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Nay{
public class EnemyManager : CharactorManager
{
    public State currentState;
    public CharacterStats currentTarget;
    public bool isPreformingAction;

    public bool isInteracting = false;

    /*public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;*/

    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidbody;

    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    [Header("AI SETTING")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float maximumStrafeRange = 5f;
    public float moveSpeed = 3f;


    //public float distanceFromTarget;
    //public float stoppingDistance = 1f;
    public float rotationSpeed = 15;
    public float maximumAttackRange = 2f;

    public float currentRecoveryTime = 0;

    

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager> ();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidbody = GetComponent<Rigidbody>();

        navmeshAgent = GetComponentInChildren<NavMeshAgent> ();
        navmeshAgent.enabled = false;
    }
    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        //HandleCurrentAction();
        HandleRecoveryTimer();
        HandleStateMachine();

        canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
    }

    private void LateUpdate() 
    {
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }


    /*private void HandleCurrentAction()
    {
        if(enemyLocomotionManager.currentTarget != null)
        {
            enemyLocomotionManager.distanceFromTarget = 
                            Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position,  transform.position);
        }


        if(enemyLocomotionManager.currentTarget == null)
        {
            enemyLocomotionManager.HandleDetection();

            if (enemyLocomotionManager.currentTarget == null) // Double-check if detection found a target
                return;

        }
        else if(enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
        {
            enemyLocomotionManager.HandleMoveToTarget();
        }
        else if(enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
        {
            AttackTarget();
        }
    }//HandleCurrentAction
*/

    private void HandleStateMachine()
    {
        if( currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats , enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }


    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime >0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if(isPreformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPreformingAction = false;
            }
        }
    }





    #region Attacks

    private void GetNewAttack()
    {
        /*Vector3 targetsDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position , transform.position);

        int maxScore = 0;

        for( int i = 0 ; i< enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if(enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
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

            if(enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
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
        }*/

    }//GetNewAttack


    private void AttackTarget()
    {
        /*if(isPreformingAction)
            return;


        if(currentAttack == null)
        {
            GetNewAttack();
        }

        else
        {
            isPreformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation,true);
            currentAttack = null;
        }*/

    }//AttackTarget

    #endregion






}//class
}//Nay
