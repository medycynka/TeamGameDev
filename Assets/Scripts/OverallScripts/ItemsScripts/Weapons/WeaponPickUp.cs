using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Items.Weapons
{
    /// <summary>
    /// Class for managing equipment item pick up
    /// </summary>
    public class WeaponPickUp : Interactable
    {
        [Header("Consumable Item Pick Up", order = 1)]
        [Header("Consumable Items List", order = 2)]
        public WeaponItem[] weapons;

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            if (weapons.Length > 0)
            {
                PickUpItem(playerManager);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Pick up this item
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        protected override void PickUpItem(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            for (var i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                {
                    if (weapons[i].itemType == ItemType.Weapon)
                    {
                        playerInventory.weaponsInventory.Add(weapons[i]);
                        uIManager.GetWeaponInventorySlot();
                        uIManager.UpdateWeaponInventory();
                    }
                    else if (weapons[i].itemType == ItemType.Shield)
                    {
                        playerInventory.shieldsInventory.Add(weapons[i]);
                        uIManager.GetShieldInventorySlot();
                        uIManager.UpdateShieldInventory();
                    }
                }
            }

            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapons[0].itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapons[0].itemIcon.texture;

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

}
