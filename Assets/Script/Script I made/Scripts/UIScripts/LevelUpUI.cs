using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Nay{
    public class LevelUpUI : MonoBehaviour
    {
        PlayerStats playerStats;
        SoulCountBar soulCountBar;


        [Header("Player Level")]
        public int currentPlayerLevel;
        public TMP_Text currentPlayerLevelText;
        int playerlevelToCancel;



        [Header("Soul")]
        public int currentSoul;
        public int soulsRequiredToLevelUp;
        public int baseLevelUpCost = 5;
        int soulToCancel;
        public TMP_Text currentSoulText;
        public TMP_Text soulsRequiredToLevelUpText;
        



        [Header("Health")]
        //public Slider healthSlider;
        public int currentHealthLevel;
        public TMP_Text currentHealthLevelText;
        int healthlevelToCancel;



        [Header("Stamina")]
        //public Slider StaminaSlider;
        public int currentStaminaLevel;
        public TMP_Text currentStaminaLevelText;
        int staminalevelToCancel;



        [Header("INT")]
        //public Slider ManaSlider;
        public int currentINTLevel;
        public TMP_Text currentManaLevelText;
        int intlevelToCancel;



        [Header("STR")]
        //public Slider strengthSlider;
        public int currentSTRLevel;
        public TMP_Text currentStrengthLevelText;
        int strlevelToCancel;



        [Header("Luck")]
        //public Slider LuckSlider;
        public int currentLuckLevel;
        public TMP_Text currentLuckLevelText;
        int lucklevelToCancel;

        

        private void Awake()
        {
            playerStats = FindObjectOfType<PlayerStats>();
            soulCountBar  = FindObjectOfType<SoulCountBar>();
        }

        private void OnEnable() 
        {
            CalculateSoulCostToLevelUp(currentPlayerLevel);
            UpdateLevelUpSlider();
        }



        private void CalculateSoulCostToLevelUp(int playerLevel)
        {
            soulsRequiredToLevelUp = 0;

            for(int i = 0; i < currentPlayerLevel; i++ )
            {
                soulsRequiredToLevelUp = soulsRequiredToLevelUp + Mathf.RoundToInt( (playerLevel * baseLevelUpCost) * 1.5f );
            }

            soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();
            currentSoulText.text = currentSoul.ToString();
        }
        

        public void ConfirmToLevelUp()
        {
            if(playerStats != null)
            {

                playerStats.playerLevel = playerlevelToCancel;
                playerStats.healthLevel = healthlevelToCancel;
                playerStats.focusPointLevel = intlevelToCancel;
                playerStats.staminaLevel = staminalevelToCancel;
                playerStats.attackLevel = strlevelToCancel;
                playerStats.luckLevel = lucklevelToCancel;
                playerStats.soulCount = currentSoul;

                playerStats.SetMaxHealthFromHealthLevel();
                playerStats.SetMaxFocusPointFromFocusPointLevel();
                playerStats.SetMaxStaminaFromStaminaLevel();

                UpdateLevelUpSlider();
            }
            
            if(soulCountBar != null)
            {
                soulCountBar.SetSoulCountText(playerStats.soulCount);
            }
        }

        public void CancelLevelUP()
        {   
            UpdateLevelUpSlider();
            soulToCancel = 0;
            currentSoulText.text = playerStats.soulCount.ToString();
            //CalculateSoulCostToLevelUp(currentPlayerLevel);
        }


        private void UpdateLevelUpSlider()
        {
            if(playerStats != null)
            {
                currentPlayerLevel = playerStats.playerLevel;
                playerlevelToCancel = playerStats.playerLevel;
                currentPlayerLevelText.text = currentPlayerLevel.ToString();

                currentHealthLevel = playerStats.healthLevel;
                healthlevelToCancel = playerStats.healthLevel;
                currentHealthLevelText.text = playerStats.healthLevel.ToString();

                currentStaminaLevel = playerStats.staminaLevel;
                staminalevelToCancel = playerStats.staminaLevel;
                currentStaminaLevelText.text = playerStats.staminaLevel.ToString();

                currentINTLevel = playerStats.focusPointLevel;
                intlevelToCancel = playerStats.focusPointLevel;
                currentManaLevelText.text = playerStats.focusPointLevel.ToString();

                currentSTRLevel = playerStats.attackLevel;
                strlevelToCancel = playerStats.attackLevel;
                currentStrengthLevelText.text = playerStats.attackLevel.ToString();

                currentLuckLevel = playerStats.luckLevel;
                lucklevelToCancel = playerStats.luckLevel;
                currentLuckLevelText.text = playerStats.luckLevel.ToString();

                currentSoul = playerStats.soulCount;
                currentSoulText.text = playerStats.soulCount.ToString();

                //CalculateSoulCostToLevelUp(currentPlayerLevel);
            }
        }





        #region UpdateLevel
        public void UpdatePlayerLevel()
        {
            if(currentPlayerLevelText == null)
                return;

            playerlevelToCancel += 1;
            currentPlayerLevelText.text = playerlevelToCancel.ToString();

            CalculateSoulCostToLevelUp(playerlevelToCancel);

            currentHealthLevelText.text = healthlevelToCancel.ToString();
            currentStaminaLevelText.text = staminalevelToCancel.ToString();
            currentManaLevelText.text = intlevelToCancel.ToString();
            currentStrengthLevelText.text = strlevelToCancel.ToString();
            currentLuckLevelText.text = lucklevelToCancel.ToString();
        }

        public void UpdateHealthLevel()
        {
            if(currentHealthLevelText == null)
                return;

            if(currentSoul >= soulsRequiredToLevelUp)
            {

                healthlevelToCancel += 1 ;
                soulToCancel = soulToCancel + soulsRequiredToLevelUp;
                currentSoul = currentSoul - soulsRequiredToLevelUp;
                UpdatePlayerLevel();
            }
        }

        public void UpdateINTLevel()
        {
            if(currentManaLevelText == null)
                return;

            if(currentSoul >= soulsRequiredToLevelUp)
            {

                intlevelToCancel += 1 ;
                soulToCancel = soulToCancel + soulsRequiredToLevelUp;
                currentSoul = currentSoul - soulsRequiredToLevelUp;
                UpdatePlayerLevel();
            }
        }

        public void UpdateStaminaLevel()
        {
            if(currentStaminaLevelText == null)
                return;

            if(currentSoul >= soulsRequiredToLevelUp)
            {

                staminalevelToCancel += 1 ;
                soulToCancel = soulToCancel + soulsRequiredToLevelUp;
                currentSoul = currentSoul - soulsRequiredToLevelUp;
                UpdatePlayerLevel();
            }
        }

        public void UpdateStrengthLevel()
        {
            if(currentStrengthLevelText == null)
                return;

            if(currentSoul >= soulsRequiredToLevelUp)
            {

                strlevelToCancel += 1 ;
                soulToCancel = soulToCancel + soulsRequiredToLevelUp;
                currentSoul = currentSoul - soulsRequiredToLevelUp;
                UpdatePlayerLevel();
            }
        }

        public void UpdateLuckLevel()
        {
            if(currentLuckLevelText == null)
                return;

            if(currentSoul >= soulsRequiredToLevelUp)
            {

                lucklevelToCancel += 1 ;
                soulToCancel = soulToCancel + soulsRequiredToLevelUp;
                currentSoul = currentSoul - soulsRequiredToLevelUp;
                UpdatePlayerLevel();
            }
        }

        


        #endregion










    }//Class
}//Nay