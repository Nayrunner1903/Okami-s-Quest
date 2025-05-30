using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Nay{
public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;
    
    //NavMeshAgent navmeshAgent;
    //public Rigidbody enemyRigidbody;

    public LayerMask detectionLayer;

    //public CharacterStats currentTarget;

    

    private void Awake() 
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        //navmeshAgent = GetComponentInChildren<NavMeshAgent> ();
        //enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider,characterCollisionBlockerCollider,true);
        //navmeshAgent.enabled = false;
        //enemyRigidbody.isKinematic = false;
    }
    
    /*public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius , detectionLayer );

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }//HandleDetection*/


    /*public void HandleMoveToTarget()
    {
        if (enemyManager.currentTarget == null)  // Prevent null reference error
        return;

        if(enemyManager.isPreformingAction)
        return;

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance( enemyManager.currentTarget.transform.position , transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if(enemyManager.isPreformingAction)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical",0,0.1f,Time.deltaTime);
            navmeshAgent.enabled = false;
        }
        else
        {
            if(distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical",1,0.1f,Time.deltaTime);
            }
            else if(distanceFromTarget <= stoppingDistance)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical",0,0.1f,Time.deltaTime);
            }
        }

        HandleRotateTowardsTarget() ;
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;

    }*/ //HandleMoveToTarget


    /*private void HandleRotateTowardsTarget()
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
            transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation , rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDir = transform.InverseTransformDirection(navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidbody.velocity;

            navmeshAgent.enabled = true;
            navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyRigidbody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation , navmeshAgent.transform.rotation
                                                    , rotationSpeed / Time.deltaTime);
        }

        /*navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;*/

    //}*/ //HandleRotateTowardsTarget 




}//class
}//Nay
