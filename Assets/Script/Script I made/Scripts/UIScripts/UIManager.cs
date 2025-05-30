using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        [SerializeField] private EquipmentWindowUI equipmentWindowUI;

        [Header("UI window")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;
        public GameObject leftPanel;

        [Header("weaponInventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Awake() 
        {
            equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();

            CloseSelectWindow();
            CloseAllInventoryWindows();
        }

        public void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();

            if (equipmentWindowUI == null)
                {
                    Debug.LogError("EquipmentWindowUI not found in the scene. Make sure it is active and present.");
                    return;
                }

            equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
        }


        public void UpdateUI()
        {
            #region weapon inventory slots

            for(int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if(i< playerInventory.weaponsInventory.Count)
                {
                    if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab , weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();   
                }
            }

            #endregion
        }



        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }


        public void CloseAllInventoryWindows()
        {
            leftPanel.SetActive(false);
            //weaponInventoryWindow.SetActive(false);
            //equipmentWindowUI.SetActive(false);
        }




    }//class
}//Nay
