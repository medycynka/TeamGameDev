using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Weapons;


namespace SzymonPeszek.GameUI.Slots
{
    /// <summary>
    /// Class representing weapon item slots in player's equipment screen
    /// </summary>
    public class HandEquipmentSlotUI : InventorySlotBase
    {
        private WeaponItem _weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool leftHandSlot01;
        public bool leftHandSlot02;

        /// <summary>
        /// Add weapon to this slot
        /// </summary>
        /// <param name="newWeapon">Weapon item</param>
        public void AddItem(WeaponItem newWeapon)
        {
            _weapon = newWeapon;
            icon.sprite = _weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Remove item from this slot
        /// </summary>
        public void ClearItem()
        {
            _weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Select this slot for weapon changing
        /// </summary>
        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected = true;
            }
            else
            {
                uiManager.leftHandSlot02Selected = true;
            }
        }
    }

}