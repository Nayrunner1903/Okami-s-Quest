using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nay{ 

[CreateAssetMenu(menuName = "Item/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;


    [Header("Idle ANIM")]
    public string RIGHT_HAND_IDLE;
    public string LEFT_HAND_IDLE;

    [Header("ONE HANDED ATTACK ANIM")]
    public string OH_LIGHT_ATTACK_1;
    public string OH_LIGHT_ATTACK_2;
    public string OH_HEAVY_ATTACK_1;


    [Header("ONE HANDED ATTACK ANIM")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon Type")]
    public bool isAttackSkillCaster;
    public bool isSupportSkillCaster;
    public bool isMaleeWeapon;

    
    [Header("Defense")]
    public float physicalDamageAbsorbtion;








}//class
}//Nay