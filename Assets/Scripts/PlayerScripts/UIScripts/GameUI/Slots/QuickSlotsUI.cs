using UnityEngine;
using UnityEngine.UI;
using SzymonPeszek.Items.Weapons;


namespace SzymonPeszek.GameUI.Slots
{
    /// <summary>
    /// Class for displaying items in quick slots
    /// </summary>
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        /// <summary>
        /// Update displayed weapon in left or right quick slot
        /// </summary>
        /// <param name="isLeft">Is weapon in left hand?</param>
        /// <param name="weapon">Weapon to show</param>
        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
        {
            if (isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }
    }

}
