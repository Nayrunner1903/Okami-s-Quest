using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class BlockingColider : MonoBehaviour
    {
        public BoxCollider blockingCollider;

        public float blockingPhysicalDamageAbsorbtion;

        private void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorbtion(WeaponItem weapon)
        {
            if(weapon != null)
            {
                blockingPhysicalDamageAbsorbtion = weapon.physicalDamageAbsorbtion;
            }
        }//SetColliderDamageAbsorbtion


        public void EnableBlockingColider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingColider()
        {
            blockingCollider.enabled = false;
        }



    }//class
}//Nay