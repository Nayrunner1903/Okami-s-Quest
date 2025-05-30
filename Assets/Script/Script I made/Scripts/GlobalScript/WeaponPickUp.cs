using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Nay{
public class WeaponPickUp : Interactable
{

    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }


    private void PickUpItem (PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        PlayerAnimatorManager animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pick Up Item" , true);
        playerInventory.weaponsInventory.Add(weapon);

        if(weapon.itemName != null)
        {
            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableGameObject.SetActive(true);
        }
        

        Destroy(gameObject);
    }






}//class
}//Nay
