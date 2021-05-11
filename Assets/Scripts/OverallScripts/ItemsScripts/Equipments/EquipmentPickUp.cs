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
            if (equipments.Length > 0)
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

            for (var i = 0; i < equipments.Length; i++)
            {
                if (equipments[i] != null)
                {
                    switch (equipments[i].itemType)
                    {
                        case ItemType.Helmet:
                            playerInventory.helmetsInventory.Add(equipments[i]);
                            uIManager.GetHelmetInventorySlot();
                            uIManager.UpdateHelmetInventory();
                            break;
                        case ItemType.ChestArmor:
                            playerInventory.chestsInventory.Add(equipments[i]);
                            uIManager.GetChestInventorySlot();
                            uIManager.UpdateChestInventory();
                            break;
                        case ItemType.ShoulderArmor:
                            playerInventory.shouldersInventory.Add(equipments[i]);
                            uIManager.GetShoulderInventorySlot();
                            uIManager.UpdateShoulderInventory();
                            break;
                        case ItemType.HandArmor:
                            playerInventory.handsInventory.Add(equipments[i]);
                            uIManager.GetHandInventorySlot();
                            uIManager.UpdateHandInventory();
                            break;
                        case ItemType.LegArmor:
                            playerInventory.legsInventory.Add(equipments[i]);
                            uIManager.GetLegInventorySlot();
                            uIManager.UpdateLegInventory();
                            break;
                        case ItemType.FootArmor:
                            playerInventory.feetInventory.Add(equipments[i]);
                            uIManager.GetFootInventorySlot();
                            uIManager.UpdateFootInventory();
                            break;
                        case ItemType.Ring:
                            playerInventory.ringsInventory.Add(equipments[i]);
                            uIManager.GetRingInventorySlot();
                            uIManager.UpdateRingInventory();
                            break;
                    }
                }
            }

            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = equipments[0].itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = equipments[0].itemIcon.texture;

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
