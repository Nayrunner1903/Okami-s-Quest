using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nay
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxFocusPoint(int maxFocusPoint)
        {
            slider.maxValue = maxFocusPoint;
            slider.value = maxFocusPoint;
        }

        public void SetCurrentFocusPoint(int currentFocusPoint)
        {
            slider.value = currentFocusPoint;
        }











    }//Class
}//Nay