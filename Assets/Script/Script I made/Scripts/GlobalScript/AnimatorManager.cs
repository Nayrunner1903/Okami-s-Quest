using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay{
public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    public bool canRotate = true;
    
    public void PlayTargetAnimation(string targetAnim , bool isInteracting , bool canRotate = false)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("canRotate",true);
        //anim.SetBool("canRotate",canRotate);
        anim.SetBool("isInteracting",isInteracting);
        anim.CrossFade(targetAnim,0.2f);
    }


}//class
}//Nay