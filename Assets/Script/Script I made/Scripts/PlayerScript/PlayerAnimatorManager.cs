using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay {


public class PlayerAnimatorManager : AnimatorManager
{

    PlayerManager playerManager;
    //public Animator anim;
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;
    

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    

    public void UpdateAnimatorValue(float verticalMove,float horizontalMove , bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMove > 0 && verticalMove < 0.55f)
        {
            v = 0.5f;
        }
        else if(verticalMove >0.55f)
        {
            v = 1;
        }
        else if (verticalMove < 0 && verticalMove > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMove < - 0.55f)
        {
            v=-1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMove > 0 && horizontalMove < 0.55f)
        {
            h = 0.5f;
        }
        else if(horizontalMove >0.55f)
        {
            h = 1;
        }
        else if (horizontalMove < 0 && horizontalMove > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMove < - 0.55f)
        {
            h=-1;
        }
        else
        {
            h = 0;
        }
        #endregion

        if(isSprinting)
        {
            v =2;
            h = horizontalMove;
        }

        anim.SetFloat(vertical , v ,0.1f,Time.deltaTime);
        anim.SetFloat(horizontal , h ,0.1f,Time.deltaTime);
    }

    /*public void PlayTargetAnimation(string targetAnim , bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting",isInteracting);
        anim.CrossFade(targetAnim,0.2f);
    }*/


    public void CanRotate()
    {
        anim.SetBool("canRotate" , true);
    }

    public void SopRotation()
    {
        anim.SetBool("canRotate" , false);
    }


    public void EnableCombo()
    {
        anim.SetBool("canDoCombo",true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo",false);
    }

    public void EnableIsInvulerable()
    {
        anim.SetBool("isInvulerable" , true);
    }

    public void DisableIsInvulerable()
    {
        anim.SetBool("isInvulerable" , false);
    }


    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false){
            return;
        }

        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition/delta;
        playerLocomotion.rigidbody.velocity = velocity;

    }





}//class    
}//Nay
