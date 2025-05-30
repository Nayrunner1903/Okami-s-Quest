using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{ 
public class WeaponHolderSlot : MonoBehaviour
{
    public WeaponItem currentWeapon;
    public Transform parentOverride;
    public bool isLeftHandSlot;
    public bool isRightHandSlot; 

    public GameObject currentWeaponModel;


    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }//Unloadweapon


    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }//Unloadweaponanddestroy


    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        if (weaponItem == null)
        { 
            UnloadWeapon();
            return; 
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        //Debug.Log("Weapon model instantiated at: " + model.transform.position);

        if(model != null)
        {
            if(parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;

            // ✅ Check for DamageCollider component
            DamageCollider damageCollider = model.GetComponentInChildren<DamageCollider>();
            if (damageCollider != null)
            {
                Debug.Log("✅ DamageCollider found and loaded with weapon: " + weaponItem.name);
            }
            else
            {
                Debug.LogWarning("⚠️ No DamageCollider found on weapon: " + weaponItem.name);
            }


        }

        currentWeaponModel = model;

    }//LoadWeaponModel









}//class
}//Nay
