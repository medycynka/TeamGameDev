using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.Items.Weapons;


namespace SzymonPeszek.GameUI.Slots
{
    /// <summary>
    /// Class representing weapon item slots in player's inventory
    /// </summary>
    public class WeaponInventorySlot : InventorySlotBase
    {
        public WeaponItem unarmedItem;
        public bool equipUnEquip;
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

        public void HandleEquipping()
        {
            if (_item != null)
            {
                equipUnEquip = !equipUnEquip;

                if (equipUnEquip)
                {
                    EquipThisItem();
                }
                else
                {
                    UnequipThisItem();
                }
            }
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
                if (_item.itemType == ItemType.Shield)
                {
                    playerInventory.leftWeapon = _item;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
                else
                {
                    playerInventory.rightWeapon = _item;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }

                slotIcon.sprite = _item.itemIcon;
                uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
                uiManager.ResetAllSelectedSlots();
            }
        }

        public void UnequipThisItem()
        {
            if (_item != null)
            {
                if (_item.itemType == ItemType.Shield)
                {
                    playerInventory.leftWeapon = unarmedItem;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
                else
                {
                    playerInventory.rightWeapon = unarmedItem;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }

                slotIcon.sprite = baseSlotIcon;
                uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
                uiManager.ResetAllSelectedSlots();
            }
        }
    }

}