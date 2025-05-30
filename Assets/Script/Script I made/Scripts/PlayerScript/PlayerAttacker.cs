using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{ 
public class PlayerAttacker : MonoBehaviour
{
    PlayerEquipmentManager playerEquipmentManager;
    PlayerAnimatorManager animatorHandler;
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;
    public string lastAttack;

    private void Awake()
    {
        animatorHandler = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory> ();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if(inputHandler.comboFlag)
        {
            animatorHandler.anim.SetBool("canDoCombo" , false);

            if(lastAttack == weapon.OH_LIGHT_ATTACK_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_LIGHT_ATTACK_2,true);
            }
        }
    }



    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if((weapon.baseStamina * weapon.lightAttackMultiplier) > playerStats.currentStamina)
        {
            Debug.Log("Stamina out");
        }
        else
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_LIGHT_ATTACK_1 , true);
            lastAttack = weapon.OH_LIGHT_ATTACK_1;
        }

    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if((weapon.baseStamina * weapon.lightAttackMultiplier) > playerStats.currentStamina)
        {
            Debug.Log("Stamina out");
        }
        else
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_HEAVY_ATTACK_1 , true);
            lastAttack = weapon.OH_HEAVY_ATTACK_1 ;
        }
    }


    public void HandleRBAction()
    {

        if(playerInventory.rightWeapon.isMaleeWeapon)
        {
            PerformRBMaleeAction();
        }
        else if(playerInventory.rightWeapon.isAttackSkillCaster || playerInventory.rightWeapon.isSupportSkillCaster)
        {
            PerformRBSkillAction(playerInventory.rightWeapon);
        }

        //Debug.Log("RB input detected");

    }//HandleRBAction


    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void PerformRBMaleeAction()
    {
                if(playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {   
            if(playerManager.isInteracting){return;}
            if(playerManager.canDoCombo){return;}

            animatorHandler.anim.SetBool("isUsingRightHand",true);
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }


    public void PerformRBSkillAction(WeaponItem weapon)
    {
        if(playerManager.isInteracting)
            return;

        if(weapon.isSupportSkillCaster)
        {
            if(playerInventory.currentSkill != null && playerInventory.currentSkill.isSupportSkill)
            {
                //Check For FP
                if(playerStats.currentFocusPoint >= playerInventory.currentSkill.focusPointCost)
                {
                    playerInventory.currentSkill.AttemptToCastSkill(animatorHandler , playerStats);
                }
                else
                {
                    //animatorHandler.PlayTargetAnimation("shrug",true);
                    Debug.Log("No Focus Points");
                }

                
            }
        }
    }

    private void SuccessfullyCastSkill()
    {
        playerInventory.currentSkill.SuccessfullyCastSkill(animatorHandler,playerStats);
    }

    
    #region def Actions

    private void PerformLBBlockingAction()
    {
        if(playerManager.isInteracting)
            return;
        
        if(playerManager.isBlocking)
            return;

        animatorHandler.PlayTargetAnimation("Block" , false , true);
        playerEquipmentManager.OpenBlockingColider();
        playerManager.isBlocking = true;


    }//PerformLBBlockingAction





    #endregion





}//class
}//Nay