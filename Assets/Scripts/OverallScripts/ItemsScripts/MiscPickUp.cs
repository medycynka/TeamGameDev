using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Items.Equipment;
using SzymonPeszek.Items.Consumable;


namespace SzymonPeszek.Items
{
    /// <summary>
    /// Class for managing misc (different types) items pick up
    /// </summary>
    public class MiscPickUp : Interactable
    {
        public List<Item> items;

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            PickUpItem(playerManager);
        }

        /// <summary>
        /// Pick up this item
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        protected override void PickUpItem(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            if (items.Count > 0)
            {
                List<ItemVisuals> icons = new List<ItemVisuals>();
                
                for (var i = 0; i < items.Count; i++)
                {
                    if (items[i] != null)
                    {
                        if (items[i] is WeaponItem)
                        {
                            if (items[i].itemType == ItemType.Weapon)
                            {
                                playerInventory.weaponsInventory.Add((WeaponItem) items[i]);
                                uIManager.GetWeaponInventorySlot();
                                uIManager.UpdateWeaponInventory();
                            }
                            else if (items[i].itemType == ItemType.Shield)
                            {
                                playerInventory.shieldsInventory.Add((WeaponItem) items[i]);
                                uIManager.GetShieldInventorySlot();
                                uIManager.UpdateShieldInventory();
                            }
                        }
                        else if (items[i] is EquipmentItem)
                        {
                            switch (items[i].itemType)
                            {
                                case ItemType.Helmet:
                                    playerInventory.helmetsInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetHelmetInventorySlot();
                                    uIManager.UpdateHelmetInventory();
                                    break;
                                case ItemType.ChestArmor:
                                    playerInventory.chestsInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetChestInventorySlot();
                                    uIManager.UpdateChestInventory();
                                    break;
                                case ItemType.ShoulderArmor:
                                    playerInventory.shouldersInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetShoulderInventorySlot();
                                    uIManager.UpdateShoulderInventory();
                                    break;
                                case ItemType.HandArmor:
                                    playerInventory.handsInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetHandInventorySlot();
                                    uIManager.UpdateHandInventory();
                                    break;
                                case ItemType.LegArmor:
                                    playerInventory.legsInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetLegInventorySlot();
                                    uIManager.UpdateLegInventory();
                                    break;
                                case ItemType.FootArmor:
                                    playerInventory.feetInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetFootInventorySlot();
                                    uIManager.UpdateFootInventory();
                                    break;
                                case ItemType.Ring:
                                    playerInventory.ringsInventory.Add((EquipmentItem) items[i]);
                                    uIManager.GetRingInventorySlot();
                                    uIManager.UpdateRingInventory();
                                    break;
                            }
                        }
                        else if (items[i] is ConsumableItem)
                        {
                            playerInventory.consumablesInventory.Add((ConsumableItem) items[i]);
                            uIManager.GetConsumableInventorySlot();
                            uIManager.UpdateConsumableInventory();
                        }
                        
                        icons.Add(new ItemVisuals
                        {
                            itemName = items[i].itemName,
                            itemIcon = items[i].itemIcon
                        });
                    }
                }

                // playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = items[0].itemName;
                // playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = items[0].itemIcon.texture;
                playerManager.ShowPickUps(icons);
                uIManager.UpdateEstusAmount();
            }
            
            // playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}