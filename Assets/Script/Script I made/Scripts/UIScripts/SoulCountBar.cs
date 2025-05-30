using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Nay{

    public class SoulCountBar : MonoBehaviour
    {
        public int soulCountNumber;
        public TMP_Text soulCountText;

        

        public void SetSoulCountText(int soulCount)
        {
            soulCountText.text = soulCount.ToString();
        }











    }//class
}//Nay