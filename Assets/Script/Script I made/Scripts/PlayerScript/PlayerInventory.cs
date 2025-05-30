using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
        public class PlayerInventory : MonoBehaviour
        {
        WeaponSlotManager weaponSlotManager;

        QuickSlotsUI  quickSlotsUI;

        public SkillItem currentSkill;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;

        public WeaponItem[] weaponInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        public List<WeaponItem> weaponsInventory;

        private void Awake()
        {
                weaponSlotManager = GetComponentInChildren<WeaponSlotManager> ();
                quickSlotsUI = FindObjectOfType<QuickSlotsUI>();


        }

        private void Start()
        {
                rightWeapon = unarmedWeapon;
                leftWeapon = unarmedWeapon;

                if(quickSlotsUI != null)
                {
                        quickSlotsUI.UpdateWeaponQuickSlotsUI(true,leftWeapon);
                        quickSlotsUI.UpdateWeaponQuickSlotsUI(false,rightWeapon);
                }
        }


        public void ChangeRightWeapon()
        {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;

                if (currentRightWeaponIndex > weaponInRightHandSlots.Length - 1)
                {
                        currentRightWeaponIndex = -1;
                        rightWeapon = unarmedWeapon;
                        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
                }
                else if (weaponInRightHandSlots[currentRightWeaponIndex] != null)
                {
                        rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
                        weaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
                }
                else
                {
                        currentRightWeaponIndex = currentRightWeaponIndex + 1;
                }
        }//ChangeRightWeapon



        public void ChangeLeftWeapon()
        {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

                if (currentLeftWeaponIndex > weaponInLeftHandSlots.Length - 1)
                {
                        currentLeftWeaponIndex = -1;
                        leftWeapon = unarmedWeapon;
                        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
                }
                else if (weaponInLeftHandSlots[currentLeftWeaponIndex] != null)
                {
                        leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
                        weaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
                }
                else
                {
                        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
                }
        }



        }//class
}//Nay
