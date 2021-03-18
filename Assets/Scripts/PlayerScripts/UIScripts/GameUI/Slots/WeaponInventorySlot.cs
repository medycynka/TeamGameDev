using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Weapons;


namespace SzymonPeszek.GameUI.Slots
{
    /// <summary>
    /// Class representing weapon item slots in player's inventory
    /// </summary>
    public class WeaponInventorySlot : InventorySlotBase
    {
        private WeaponItem _item;

        /// <summary>
        /// Add item to this slot
        /// </summary>
        /// <param name="newItem">Weapon item</param>
        public void AddItem(WeaponItem newItem)
        {
            _item = newItem;
            icon.sprite = _item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Delete item from this slot
        /// </summary>
        /// <param name="lastSlot"></param>
        public void ClearInventorySlot(bool lastSlot)
        {
            _item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(lastSlot);
        }

        /// <summary>
        /// Equip item from this slot
        /// </summary>
        public void EquipThisItem()
        {
            if (_item != null)
            {
                if (uiManager.rightHandSlot01Selected)
                {
                    playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);
                    playerInventory.weaponsInRightHandSlots[0] = _item;
                    playerInventory.weaponsInventory.Remove(_item);
                }
                else if (uiManager.rightHandSlot02Selected)
                {
                    playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
                    playerInventory.weaponsInRightHandSlots[1] = _item;
                    playerInventory.weaponsInventory.Remove(_item);
                }
                else if (uiManager.leftHandSlot01Selected)
                {
                    playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
                    playerInventory.weaponsInLeftHandSlots[0] = _item;
                    playerInventory.weaponsInventory.Remove(_item);
                }
                else if (uiManager.leftHandSlot02Selected)
                {
                    playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
                    playerInventory.weaponsInLeftHandSlots[1] = _item;
                    playerInventory.weaponsInventory.Remove(_item);
                }
                else
                {
                    return;
                }

                playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
                playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

                uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
                uiManager.ResetAllSelectedSlots();
            }
        }
    }

}