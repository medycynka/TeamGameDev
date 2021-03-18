using BattleDrakeStudios.ModularCharacters;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.Items.Equipment;


namespace SzymonPeszek.GameUI.Slots
{
    /// <summary>
    /// Class representing equipment item slots in player's inventory 
    /// </summary>
    public class EquipmentInventorySlot : InventorySlotBase
    {
        public CurrentEquipments currentEquipments;
        public ModularCharacterManager modularCharacterManager;
        public bool equipUnEquip;
        
        private EquipmentItem _item;
        private bool _shouldDeactivate;

        /// <summary>
        /// Add equipment item to this slot
        /// </summary>
        /// <param name="newItem">Equipment item</param>
        public void AddItem(EquipmentItem newItem)
        {
            _item = newItem;
            icon.sprite = _item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Delete item from this slot
        /// </summary>
        /// <param name="lastSlot">Is it last slot in inventory tab?</param>
        public void ClearInventorySlot(bool lastSlot)
        {
            _item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(lastSlot);
        }

        /// <summary>
        /// Equip or unequip item from this slot
        /// </summary>
        public void HandleEquipping()
        {
            if (_item != null)
            {
                equipUnEquip = !equipUnEquip;

                if (equipUnEquip)
                {
                    EquipThisItem();

                    switch (_item.itemType)
                    {
                        case ItemType.Helmet:
                            uiManager.GetHelmetInventorySlot();
                            uiManager.UpdateHelmetInventory();
                            break;
                        case ItemType.ChestArmor:
                            uiManager.GetChestInventorySlot();
                            uiManager.UpdateChestInventory();
                            break;
                        case ItemType.ShoulderArmor:
                            uiManager.GetShoulderInventorySlot();
                            uiManager.UpdateShoulderInventory();
                            break;
                        case ItemType.HandArmor:
                            uiManager.GetHandInventorySlot();
                            uiManager.UpdateHandInventory();
                            break;
                        case ItemType.LegArmor:
                            uiManager.GetLegInventorySlot();
                            uiManager.UpdateLegInventory();
                            break;
                        case ItemType.FootArmor:
                            uiManager.GetFootInventorySlot();
                            uiManager.UpdateFootInventory();
                            break;
                        case ItemType.Ring:
                            uiManager.GetRingInventorySlot();
                            uiManager.UpdateRingInventory();
                            break;
                    }

                    equipUnEquip = true;
                }
                else
                {
                    UnequipThisItem();
                }
            }
        }
        
        /// <summary>
        /// Equip item from this slot
        /// </summary>
        private void EquipThisItem()
        {
            if (_item.itemType == ItemType.Ring)
            {
                
            }
            else
            {
                #region Deactivate Head if necessary
                if (_item.removeHead)
                {
                    modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Head].eqPart);
                }
                #endregion

                for (var i = 0; i < _item.bodyParts.Length; i++)
                {
                    var bodyPart = _item.bodyParts[i];

                    #region Deactivate Unnecesary Head Parts
                    if (_item.removeHeadFeatures)
                    {
                        if (bodyPart == ModularBodyPart.Helmet)
                        {
                            if (currentEquipments.currentEq[ModularBodyPart.Hat].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hat]
                                    .eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Hat].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Hat].armorValue = 0;
                            }

                            if (currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments
                                    .currentEq[ModularBodyPart.HeadCovering].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Ear]
                                .eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hair]
                                .eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Eyebrow]
                                .eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments
                                .currentEq[ModularBodyPart.FacialHair].eqPart);
                        }
                        else if (bodyPart == ModularBodyPart.HeadCovering)
                        {
                            if (currentEquipments.currentEq[ModularBodyPart.Hat].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hat]
                                    .eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Hat].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Hat].armorValue = 0;
                            }

                            if (currentEquipments.currentEq[ModularBodyPart.Helmet].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments
                                    .currentEq[ModularBodyPart.Helmet].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Helmet].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Helmet].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Ear]
                                .eqPart);
                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hair]
                                .eqPart);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.Head].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.Head].partID);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.FacialHair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.Hat)
                        {
                            if (currentEquipments.currentEq[ModularBodyPart.Helmet].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments
                                    .currentEq[ModularBodyPart.Helmet].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.Helmet].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.Helmet].armorValue = 0;
                            }

                            if (currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID > -1)
                            {
                                modularCharacterManager.DeactivatePart(currentEquipments
                                    .currentEq[ModularBodyPart.HeadCovering].eqPart);
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].partID = -1;
                                currentEquipments.currentEq[ModularBodyPart.HeadCovering].armorValue = 0;
                            }

                            modularCharacterManager.DeactivatePart(currentEquipments.currentEq[ModularBodyPart.Hair]
                                .eqPart);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.Head].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.Head].partID);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.Ear].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(
                                currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart,
                                currentEquipments.currentEq[ModularBodyPart.FacialHair].partID);
                        }
                    }
                    #endregion

                    #region Equip Item
                    modularCharacterManager.ActivatePart(bodyPart, _item.partsIds[i]);
                    currentEquipments.currentEq[bodyPart].partID = _item.partsIds[i];
                    currentEquipments.currentEq[bodyPart].armorValue = _item.armor;
                    #endregion
                }
            }

            currentEquipments.SaveCurrentEqIds();
            currentEquipments.UpdateArmorValue();
        }

        /// <summary>
        /// Unequip item from this slot
        /// </summary>
        private void UnequipThisItem()
        {
            if (_item.itemType == ItemType.Ring)
            {

            }
            else
            {
                #region Activate Head if necessary
                if (_item.removeHead)
                {
                    modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Head].eqPart, currentEquipments.currentEq[ModularBodyPart.Head].partID);
                }
                #endregion

                foreach (var bodyPart in _item.bodyParts)
                {
                    #region Activate Head Parts
                    if (_item.removeHeadFeatures)
                    {
                        if (bodyPart == ModularBodyPart.Helmet)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart, currentEquipments.currentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart, currentEquipments.currentEq[ModularBodyPart.Hair].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Eyebrow].eqPart, currentEquipments.currentEq[ModularBodyPart.Eyebrow].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.FacialHair].eqPart, currentEquipments.currentEq[ModularBodyPart.FacialHair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.HeadCovering)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Ear].eqPart, currentEquipments.currentEq[ModularBodyPart.Ear].partID);
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart, currentEquipments.currentEq[ModularBodyPart.Hair].partID);
                        }
                        else if (bodyPart == ModularBodyPart.Hat)
                        {
                            modularCharacterManager.ActivatePart(currentEquipments.currentEq[ModularBodyPart.Hair].eqPart, currentEquipments.currentEq[ModularBodyPart.Hair].partID);
                        }
                    }
                    #endregion

                    #region Unequip Item
                    _shouldDeactivate = _item.canDeactivate || (bodyPart == ModularBodyPart.ShoulderAttachmentLeft
                        || bodyPart == ModularBodyPart.ShoulderAttachmentRight
                        || bodyPart == ModularBodyPart.ElbowAttachmentRight
                        || bodyPart == ModularBodyPart.ElbowAttachmentLeft
                        || bodyPart == ModularBodyPart.BackAttachment
                        || bodyPart == ModularBodyPart.KneeAttachmentRight
                        || bodyPart == ModularBodyPart.KneeAttachmentLeft);

                    if (_shouldDeactivate)
                    {
                        modularCharacterManager.DeactivatePart(bodyPart);
                        currentEquipments.currentEq[bodyPart].partID = -1;
                    }
                    else
                    {
                        modularCharacterManager.ActivatePart(bodyPart, 0);
                        currentEquipments.currentEq[bodyPart].partID = 0;
                    }

                    currentEquipments.currentEq[bodyPart].armorValue = 0;
                    #endregion
                }
            }

            currentEquipments.SaveCurrentEqIds();
            currentEquipments.UpdateArmorValue();
        }
    }

}