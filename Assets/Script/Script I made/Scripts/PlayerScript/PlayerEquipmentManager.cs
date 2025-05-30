using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nay{


    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        public BlockingColider blockingColider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            //blockingColider = GetComponentInChildren<BlockingColider>();
        }

        public void OpenBlockingColider()
        {
            blockingColider.SetColliderDamageAbsorbtion(playerInventory.leftWeapon);
            blockingColider.EnableBlockingColider();
        }

        public void CloseBlockingColider()
        {
            blockingColider.DisableBlockingColider();
        }
















    }//Class
}//Nay