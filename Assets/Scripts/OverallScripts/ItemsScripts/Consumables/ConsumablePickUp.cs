using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Items.Consumable
{
    /// <summary>
    /// Class for managing consumable item pick up
    /// </summary>
    public class ConsumablePickUp : Interactable
    {
        [Header("Consumable Item Pick Up", order = 1)]
        [Header("Consumable Items List", order = 2)]
        public ConsumableItem[] consumableItems;

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            if (consumableItems.Length > 0)
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
            
            List<ItemVisuals> icons = new List<ItemVisuals>();

            if (consumableItems[0].isDeathDrop)
            {
                playerManager.GetComponent<PlayerStats>().soulsAmount += consumableItems[0].soulAmount;
                uIManager.UpdateSouls();
                
                icons.Add(new ItemVisuals
                {
                    itemName = consumableItems[0].itemName,
                    itemIcon = consumableItems[0].itemIcon
                });
            }
            else
            {
                for (var i = 0; i < consumableItems.Length; i++)
                {
                    if (consumableItems[i] != null)
                    {
                        playerInventory.consumablesInventory.Add(consumableItems[i]);
                    }

                    icons.Add(new ItemVisuals
                    {
                        itemName = consumableItems[i].itemName,
                        itemIcon = consumableItems[i].itemIcon
                    });
                }

                uIManager.GetConsumableInventorySlot();
                uIManager.UpdateConsumableInventory();
                uIManager.UpdateEstusAmount();
            }

            // playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text =
            //     consumableItems[0].itemName;
            // playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture =
            //     consumableItems[0].itemIcon.texture;
            //
            // playerManager.itemInteractableGameObject.SetActive(true);
            playerManager.ShowPickUps(icons);
            Destroy(gameObject);
        }
    }
}