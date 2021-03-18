using UnityEngine;
using TMPro;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.GameUI.Slots;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    /// <summary>
    /// Class for displaying and managing player's current equipment
    /// </summary>
    public class EquipmentWindowUI : MonoBehaviour
    {
        [Header("Weapon Quick Slots")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public HandEquipmentSlotUI[] handEquipmentSlotUI;

        [Header("Stats Values")]
        public TextMeshProUGUI strengthValue;
        public TextMeshProUGUI agilityValue;
        public TextMeshProUGUI defenceValue;
        public TextMeshProUGUI allDefenceValue;
        public TextMeshProUGUI healthValue;
        public TextMeshProUGUI maxHealthValue;
        public TextMeshProUGUI staminaValue;
        public TextMeshProUGUI maxStaminaValue;

        /// <summary>
        /// Load current weapons to equipment screen
        /// </summary>
        /// <param name="playerInventory">Player's inventory</param>
        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                if (handEquipmentSlotUI[i].rightHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        /// <summary>
        /// Select right hand main slot
        /// </summary>
        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        /// <summary>
        /// Select right hand secondary slot
        /// </summary>
        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        /// <summary>
        /// Select left hand main slot
        /// </summary>
        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        /// <summary>
        /// Select left hand secondary slot
        /// </summary>
        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }

        /// <summary>
        /// Update visible player's stats
        /// </summary>
        /// <param name="playerStats">Player's stats</param>
        public void UpdateStatsWindow(PlayerStats playerStats)
        {
            strengthValue.text = playerStats.strength.ToString();
            agilityValue.text = playerStats.agility.ToString();
            defenceValue.text = playerStats.defence.ToString();
            allDefenceValue.text = playerStats.currentArmorValue.ToString();
            healthValue.text = playerStats.bonusHealth.ToString();
            maxHealthValue.text = playerStats.maxHealth.ToString();
            staminaValue.text = playerStats.bonusStamina.ToString();
            maxStaminaValue.text = playerStats.maxStamina.ToString();
        }
    }

}