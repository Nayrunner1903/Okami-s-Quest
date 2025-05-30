using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool a_input;
        public bool b_input;
        public bool rb_input;
        public bool rt_input;
        public bool lb_input;
        public bool jump_input;

        public bool inventory_input;

        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;


        public bool d_pad_up;
        public bool d_pad_down;
        public bool d_pad_left;
        public bool d_pad_right;

        public bool comboFlag;
        public bool rollFlag;
        public bool sprintFlag;
        public bool inventoryFlag;

        public bool lockOnFlag;

        public float rollInputTimer;
        

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        PlayerStats playerStats;
        CameraHandler cameraHandler;
        public BlockingColider blockingColider;

        PlayerAnimatorManager animatorHandler;

        UIManager uIManager;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            uIManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
            blockingColider = GetComponentInChildren<BlockingColider>();
        }


        public void OnEnable()
        {
            if (inputActions == null){
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                //b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

                inputActions.PlayerActions.RB.performed += i => rb_input = true;
                inputActions.PlayerActions.RT.performed += i => rt_input = true;

                inputActions.PlayerActions.LB.performed += i => lb_input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_input = false;

                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_left = true;

                inputActions.PlayerActions.A.performed += i => a_input = true;

                inputActions.PlayerActions.Jump.performed += i => jump_input = true;

                inputActions.PlayerActions.Inventory.performed += i => inventory_input = true;

                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
        
            }

            inputActions.Enable();
        }//OnEnable

        private void OnDisable() 
        {
            inputActions.Disable();
        }//OnDisable

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            //HandleInteractingButtonInput();
            //HandleJumpInput();
            HandleInventoryInput();
            HandleLockOnInput();
        }//TickInput

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01( Mathf.Abs(horizontal) + Mathf.Abs(vertical) );

            if(!inventoryFlag)
            {
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            
        }//MoveInput

        private void HandleRollInput(float delta)
        {
            b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            sprintFlag = b_input;


            if(b_input)
            {
                rollInputTimer += delta;
                //sprintFlag = true;
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer< 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }//HandleRollInput


        private void HandleAttackInput(float delta)
        {
        

            if(rb_input)
            {
                playerAttacker.HandleRBAction();
            }

            if(rt_input)
            {
                //Debug.Log("RT input detected");
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }

            //Blocking
            if(lb_input)
            {
                //Debug.Log("LB input detected");
                //playerManager.isBlocking = true;
                playerAttacker.HandleLBAction();
            }
            else
            {
                playerManager.isBlocking = false;

                if(blockingColider.blockingCollider.enabled)
                {
                    blockingColider.DisableBlockingColider();
                }
            }

        }//AttackInput


        private void HandleQuickSlotsInput()
        {
            //inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_right = true;
            //inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_left = true;

            if(d_pad_right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_pad_left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }



        /*private void HandleInteractingButtonInput()
        {
            inputActions.PlayerActions.A.performed += i => a_input = true;
        }*/


        /*private void HandleJumpInput()
        {
            inputActions.PlayerActions.Jump.performed += i => jump_input = true;
        }*/


        private void HandleInventoryInput()
        {
            //inputActions.PlayerActions.Inventory.performed += i => inventory_input = true;

            if(inventory_input)
            {
                inventoryFlag = !inventoryFlag;

                if(inventoryFlag)
                {
                    uIManager.OpenSelectWindow();
                    uIManager.UpdateUI();
                    uIManager.hudWindow.SetActive(false);
                }
                else
                {
                    uIManager.CloseSelectWindow();
                    uIManager.CloseAllInventoryWindows();
                    uIManager.hudWindow.SetActive(true);
                }
            }
        }



        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                //cameraHandler.ClearLockOnTarget();
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget();
            }

            if(lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();



        }// HandleLockOnInput




    }//class
}//Nay
