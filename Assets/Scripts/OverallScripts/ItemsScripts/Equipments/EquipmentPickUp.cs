using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Items.Equipment
{
    /// <summary>
    /// Class for managing equipment item pick up
    /// </summary>
    public class EquipmentPickUp : Interactable
    {
        [Header("Equipment Item Pick Up", order = 1)]
        [Header("Equipment Items List", order = 2)]
        public EquipmentItem[] equipments;

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

            if (equipments.Length > 0)
            {
                foreach (var equipment in equipments)
                {
                    if (equipment != null)
                    {
                        switch (equipment.itemType)
                        {
                            case ItemType.Helmet:
                                playerInventory.helmetsInventory.Add(equipment);
                                uIManager.GetHelmetInventorySlot();
                                uIManager.UpdateHelmetInventory();
                                break;
                            case ItemType.ChestArmor:
                                playerInventory.chestsInventory.Add(equipment);
                                uIManager.GetChestInventorySlot();
                                uIManager.UpdateChestInventory();
                                break;
                            case ItemType.ShoulderArmor:
                                playerInventory.shouldersInventory.Add(equipment);
                                uIManager.GetShoulderInventorySlot();
                                uIManager.UpdateShoulderInventory();
                                break;
                            case ItemType.HandArmor:
                                playerInventory.handsInventory.Add(equipment);
                                uIManager.GetHandInventorySlot();
                                uIManager.UpdateHandInventory();
                                break;
                            case ItemType.LegArmor:
                                playerInventory.legsInventory.Add(equipment);
                                uIManager.GetLegInventorySlot();
                                uIManager.UpdateLegInventory();
                                break;
                            case ItemType.FootArmor:
                                playerInventory.feetInventory.Add(equipment);
                                uIManager.GetFootInventorySlot();
                                uIManager.UpdateFootInventory();
                                break;
                            case ItemType.Ring:
                                playerInventory.ringsInventory.Add(equipment);
                                uIManager.GetRingInventorySlot();
                                uIManager.UpdateRingInventory();
                                break;
                        }
                    }
                }

                playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = equipments[0].itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = equipments[0].itemIcon.texture;
            }

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
