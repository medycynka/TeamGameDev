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
                for (var i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (item != null)
                    {
                        if (item is WeaponItem)
                        {
                            if (item.itemType == ItemType.Weapon)
                            {
                                playerInventory.weaponsInventory.Add((WeaponItem) item);
                                uIManager.GetWeaponInventorySlot();
                                uIManager.UpdateWeaponInventory();
                            }
                            else if (item.itemType == ItemType.Shield)
                            {
                                playerInventory.shieldsInventory.Add((WeaponItem) item);
                                uIManager.GetShieldInventorySlot();
                                uIManager.UpdateShieldInventory();
                            }
                        }
                        else if (item is EquipmentItem)
                        {
                            switch (item.itemType)
                            {
                                case ItemType.Helmet:
                                    playerInventory.helmetsInventory.Add((EquipmentItem) item);
                                    uIManager.GetHelmetInventorySlot();
                                    uIManager.UpdateHelmetInventory();
                                    break;
                                case ItemType.ChestArmor:
                                    playerInventory.chestsInventory.Add((EquipmentItem) item);
                                    uIManager.GetChestInventorySlot();
                                    uIManager.UpdateChestInventory();
                                    break;
                                case ItemType.ShoulderArmor:
                                    playerInventory.shouldersInventory.Add((EquipmentItem) item);
                                    uIManager.GetShoulderInventorySlot();
                                    uIManager.UpdateShoulderInventory();
                                    break;
                                case ItemType.HandArmor:
                                    playerInventory.handsInventory.Add((EquipmentItem) item);
                                    uIManager.GetHandInventorySlot();
                                    uIManager.UpdateHandInventory();
                                    break;
                                case ItemType.LegArmor:
                                    playerInventory.legsInventory.Add((EquipmentItem) item);
                                    uIManager.GetLegInventorySlot();
                                    uIManager.UpdateLegInventory();
                                    break;
                                case ItemType.FootArmor:
                                    playerInventory.feetInventory.Add((EquipmentItem) item);
                                    uIManager.GetFootInventorySlot();
                                    uIManager.UpdateFootInventory();
                                    break;
                                case ItemType.Ring:
                                    playerInventory.ringsInventory.Add((EquipmentItem) item);
                                    uIManager.GetRingInventorySlot();
                                    uIManager.UpdateRingInventory();
                                    break;
                            }
                        }
                        else if (item is ConsumableItem)
                        {
                            playerInventory.consumablesInventory.Add((ConsumableItem) item);
                            uIManager.GetConsumableInventorySlot();
                            uIManager.UpdateConsumableInventory();
                        }
                    }
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = items[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = items[0].itemIcon.texture;
                uIManager.UpdateEstusAmount();
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}