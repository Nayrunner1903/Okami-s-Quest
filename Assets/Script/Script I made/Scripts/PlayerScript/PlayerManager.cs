using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{


    public class PlayerManager : CharactorManager
    {
        
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        InteractableUI interactableUI;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerStats playerStats;

        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        public bool isInteracting;
        public bool canDoCombo;

        [Header("PlayerFlags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrouned;

        public bool isUsingRightHand;
        public bool isUsingLeftHand;

        public bool isInvulerable;

        //[HideInInspector]
        //public bool isBlocking;



        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStats = GetComponent<PlayerStats>();
        }


        /*void Start()
        {
            if (cameraHandler == null)
            {
                cameraHandler = FindObjectOfType<CameraHandler>();
                if (cameraHandler != null)
                {
                    CameraHandler.singleton = cameraHandler;
                }
                else
                {
                    Debug.LogError("CameraHandler not found in the scene.");
                }
            }   
        }*/

        

        
        void Update()
        {
            float delta =Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isUsingRightHand = anim.GetBool("isUsingRightHand");

            isInvulerable = anim.GetBool("isInvulerable");

            anim.SetBool("isInAir" , isInAir);

            anim.SetBool("isBlocking" , isBlocking);

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = anim.GetBool("canRotate");
            playerLocomotion.HandleRollAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerLocomotion.HandleRotation(delta);
            playerStats.RegenerateStamina();


            
            
            //playerLocomotion.HandleMovement(delta);
            //playerLocomotion.HandleRollAndSprinting(delta);
            //playerLocomotion.HandleFalling(delta,playerLocomotion.moveDirection);
            //playerLocomotion.HandleJumping();

            CheckForInteractableObject();
        }

        private void FixedUpdate() 
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta,playerLocomotion.moveDirection);
            
            /*if(cameraHandler != null){
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX,inputHandler.mouseY);
            }
            else{Debug.Log("cameraHandler = null");}*/
        }

        private void LateUpdate() 
        {
            inputHandler.rollFlag = false;
            //inputHandler.sprintFlag = false;
            inputHandler.rb_input = false;
            inputHandler.rt_input = false;
            inputHandler.d_pad_down = false;
            inputHandler.d_pad_up = false;
            inputHandler.d_pad_left = false;
            inputHandler.d_pad_right = false;
            inputHandler.a_input = false;
            inputHandler.jump_input = false;
            inputHandler.inventory_input = false;

            float delta = Time.deltaTime;
            if(cameraHandler != null){
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX,inputHandler.mouseY);
            }
            else{Debug.Log("cameraHandler = null");}

            if(isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }


        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position,0.3f,transform.forward,out hit , 1f , cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if(inputHandler.a_input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableGameObject != null && inputHandler.a_input)
                {
                    itemInteractableGameObject.SetActive(false);
                }

            }
        }   




    }//class
}//Nay
