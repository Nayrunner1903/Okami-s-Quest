using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Levels Up")]
    public int playerLevel = 1;
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusPointLevel = 10;
    public int attackLevel = 10;
    public int luckLevel = 10;

    [Header("SOULS")]
    public int soulCount = 0;


    [Header("HP AND DEAD")]
    public bool isDead = false;
    public int maxHealth;
    public int currentHealth;


    [Header("Stamina")]
    public float maxStamina;
    public float currentStamina;
    

    [Header("MANA (INtelligent)")]
    public int maxFocusPoint;
    public int currentFocusPoint;

    

}
