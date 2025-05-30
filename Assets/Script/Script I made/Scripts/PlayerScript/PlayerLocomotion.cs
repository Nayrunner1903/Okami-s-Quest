using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
public class PlayerLocomotion : MonoBehaviour
{
    
    CameraHandler cameraHandler;

    Transform cameraObject;
    InputHandler inputHandler;
    PlayerManager playerManager;
    
    PlayerStats playerStats;

    public Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;

    [HideInInspector]
    public PlayerAnimatorManager animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;


    [Header("Grounded and Air Detect Stats")]
    [SerializeField] 
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] 
    float minimumDisToBeginFall = 1f;
    [SerializeField] 
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;


    [Header("Movement Stats")]
    [SerializeField] 
    float movementSpeed = 5;
    [SerializeField] 
    float sprintSpeed = 7;
    [SerializeField] 
    float rotationSpeed = 10;
    [SerializeField] 
    float fallingSpeed = 45;
    [SerializeField]
    float jumpHigh = 20f;


    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;


    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrouned = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);

        Physics.IgnoreCollision(characterCollider,characterCollisionBlockerCollider,true);
    }




    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleRotation(float delta)
    {
        if(animatorHandler.canRotate)
        {
                if(inputHandler.lockOnFlag)
            {
                if(inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if(targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation , tr , rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection) ;
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation , tr , rotationSpeed* Time.deltaTime);
                    transform.rotation = targetRotation;
                }
                

            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = inputHandler.moveAmount;

                targetDir = cameraObject.forward * inputHandler.vertical;
                targetDir += cameraObject.right * inputHandler.horizontal;

                targetDir.Normalize();
                targetDir.y =0;

                if(targetDir == Vector3.zero) {
                    targetDir = myTransform.forward;
                }

                float rs = rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation,tr,rs*delta);

                myTransform.rotation = targetRotation;
            }
        }

        
        
    }//HandleRotation

    public void HandleMovement(float delta)
    {

        if(inputHandler.rollFlag){return;}

        if(playerManager.isInteracting){return;}

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if(inputHandler.sprintFlag && inputHandler.moveAmount >0.5)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            moveDirection *= speed;
            playerManager.isSprinting = false;
        }


        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        if(inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
        {
            animatorHandler.UpdateAnimatorValue(inputHandler.vertical,inputHandler.horizontal , playerManager.isSprinting);
        }
        else
        {
            animatorHandler.UpdateAnimatorValue(inputHandler.moveAmount,0 , playerManager.isSprinting);
        }

        

        
    }

    public void HandleRollAndSprinting(float delta)
    {
        if(animatorHandler.anim.GetBool("isInteracting"))
        {
            return;
        }

        if(playerStats.rollStaminaUsage > playerStats.currentStamina)
        {
            Debug.Log("Stamina out");
            return;
        }

        if(inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if(inputHandler.moveAmount >0)
            {
                animatorHandler.PlayTargetAnimation("Rolling",true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                
                //playerStats.TakeStamina(playerStats.rollStaminaUsage);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Backstep",true);
            }
        }
    }
    

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrouned = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if(Physics.Raycast(origin , myTransform.forward , out hit ,0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if(playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin , -Vector3.up * minimumDisToBeginFall , Color.red , 0.1f , false);
        
        if(Physics.Raycast(origin , -Vector3.up , out hit , minimumDisToBeginFall , ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrouned = true;
            targetPosition.y = tp.y;

            if(playerManager.isInAir)
            {
                Debug.Log("In Air for"+ inAirTimer);
                animatorHandler.PlayTargetAnimation("Land" , true);
                inAirTimer = 0;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Locomotion" , false);
                inAirTimer = 0;
            }

            playerManager.isInAir = false;
        }
        else
        {
            if(playerManager.isGrouned)
            {
                playerManager.isGrouned = false;
            }

            if(playerManager.isInAir == false)
            {
                if(playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling" , true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed/2);
                playerManager.isInAir = true;
            } 
        }



        if(playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition , Time.deltaTime);
        }
        else
        {
            myTransform.position = targetPosition;
        }
    



    }



    public void HandleJumping()
    {
        if(playerManager.isInteracting)
        {
            return;
        }

        if(inputHandler.jump_input)
        {
            if(inputHandler.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                animatorHandler.PlayTargetAnimation("Jump" , true);
                moveDirection.y = 0;

                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;

                rigidbody.AddForce(Vector3.up * jumpHigh, ForceMode.Impulse);

                Debug.Log("jump!");
            }
            
        }
    }

    #endregion


}//Class
}//Nay
