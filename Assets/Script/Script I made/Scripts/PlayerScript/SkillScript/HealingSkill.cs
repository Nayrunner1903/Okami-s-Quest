using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{

    [CreateAssetMenu(menuName = "Skills/HealingSkill")]
    public class HealingSkill : SkillItem
    {
        public int healAmount;


        public override void AttemptToCastSkill(PlayerAnimatorManager animatorHandler , PlayerStats playerStats)
        {
            base.AttemptToCastSkill(animatorHandler , playerStats);

            //Debug.Log("AttemptToCastSkill");
            GameObject instantiatedWarmupSkillFX = Instantiate(skillWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(skillAnimation , true);

        }

        public override void SuccessfullyCastSkill(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSkill(animatorHandler , playerStats);

            GameObject instantiatedSkillFX = Instantiate(skillCastFX, animatorHandler.transform );
            playerStats.HealPlayer(healAmount);
            //Debug.Log("SuccessfullyCastSkill");
        }











        
    }//class

}//nay
