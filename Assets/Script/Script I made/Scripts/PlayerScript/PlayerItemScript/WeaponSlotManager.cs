using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Nay{


public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager playerManager;

    public WeaponItem attackingWeapon;

    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    DamageCollider leftDamageCollider;
    DamageCollider rightDamageCollider;

    Animator animator;

    QuickSlotsUI quickSlotsUI;

    PlayerStats playerStats;




    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();

        WeaponHolderSlot[] weaponHolderSlots= GetComponentsInChildren<WeaponHolderSlot>();
        foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if(weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
        
    }//awake


    public void LoadWeaponOnSlot(WeaponItem weaponItem , bool isLeft)
    {
        if(isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(true,weaponItem);

            #region Handle weapon idle anim
            if(weaponItem != null)
            {
                animator.CrossFade(weaponItem.LEFT_HAND_IDLE,0.2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty",0.2f);
            }
            #endregion
        }
        else
        {
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false,weaponItem);

            #region Handle weapon idle anim
            if(weaponItem != null)
            {
                animator.CrossFade(weaponItem.RIGHT_HAND_IDLE,0.2f);
            }
            else
            {
                animator.CrossFade("Right Arm Empty",0.2f);
            }
            #endregion
        }
    }



    #region Handle damage collider
    private void LoadLeftWeaponDamageCollider()
{
    leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
}

    private void LoadRightWeaponDamageCollider()
{
    rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
}

public void OpenDamageCollider()
{
    if(playerManager.isUsingRightHand)
    {
        rightDamageCollider.EnableDamageCollider();
    }
    else if(playerManager.isUsingLeftHand)
    {
        leftDamageCollider.EnableDamageCollider();
    }
    
}

public void CloseDamageCollider()
{
    rightDamageCollider.DisableDamageCollider();
    leftDamageCollider.DisableDamageCollider();
}

/*public void OpenRightDamageCollider()
{
    rightDamageCollider.EnableDamageCollider();
}

public void OpenleftDamageCollider()
{
    leftDamageCollider.EnableDamageCollider();
}

public void CloseRightDamageCollider()
{
    rightDamageCollider.DisableDamageCollider();
}

public void CloseLeftDamageCollider()
{
    leftDamageCollider.DisableDamageCollider();
}*/
#endregion


    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }



}//class
}//Nay
