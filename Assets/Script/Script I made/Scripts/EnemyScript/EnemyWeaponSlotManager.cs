using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot leftHandSlot;

        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;


        public void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots= GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    //Debug.Log("found LeftHandSlot");
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    //Debug.Log("found LeftHandSlot");
                    rightHandSlot = weaponSlot;
                }
            }
        }

        private void Start()
        {
            LoadWeaponONbothHAnd();
        }

        public void LoadWeaponONbothHAnd()
        {
            if(rightHandWeapon != null)
            {
                //Debug.Log("rightHandWeapon != null");
                LoadWeaponOnSlot(rightHandWeapon,false);
            }
            if(leftHandWeapon != null)
            {
                //Debug.Log("leftHandWeapon != null");
                LoadWeaponOnSlot(leftHandWeapon,true);
            }
        }

        public void LoadWeaponOnSlot(WeaponItem Weapon, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.currentWeapon = Weapon;
                leftHandSlot.LoadWeaponModel(Weapon);

                LoadWeaponDamageColider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = Weapon;
                rightHandSlot.LoadWeaponModel(Weapon);

                LoadWeaponDamageColider(false);
            }

        }//LoadWeaponOnSlot



        public void LoadWeaponDamageColider(bool isLeft)
        {
            if(isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentsInChildren<DamageCollider>()[0];
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentsInChildren<DamageCollider>()[0];
            }
        }

        public void OpenDamageCollider()
        {
            //Debug.Log("EnableDamageCollider");
            if(rightHandDamageCollider == null){
                Debug.Log("Null coilder");
            }
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }



    }//Class
}//Nay