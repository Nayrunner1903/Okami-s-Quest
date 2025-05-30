using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class SkillItem : Item
    {
        public GameObject skillWarmUpFX;
        public GameObject skillCastFX;
        public string skillAnimation;


        [Header("Skill Types")]
        public bool isAttackSkill;
        public bool isSupportSkill;

        [Header("Skill Cost")]
        public int focusPointCost;

        [Header("Skill Des")]
        public string skillDescription;


        public virtual void AttemptToCastSkill(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            Debug.Log(skillDescription+"AttemptToCastSkill");
            Debug.Log(skillDescription);
        }

        public virtual void SuccessfullyCastSkill(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            Debug.Log(skillDescription+"SuccessfullyCastSkill");
            playerStats.DeductFocusPoints(focusPointCost);
        }




    }//Class
}//Nay
