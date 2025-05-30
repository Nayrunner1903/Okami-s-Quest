using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nay{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        Slider slider;
        float timeUntilBarISHidden = 0;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilBarISHidden = 3;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            timeUntilBarISHidden = timeUntilBarISHidden - Time.deltaTime;

            if(slider != null)
            {
                if(timeUntilBarISHidden <= 0)
                {
                    timeUntilBarISHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if(!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);
                    }
                }

                if(slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }

        }//update

        private void LateUpdate() 
        {
            transform.LookAt(Camera.main.transform);
        }













    }//Class
}//Nay
