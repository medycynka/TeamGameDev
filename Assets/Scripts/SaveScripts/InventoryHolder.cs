using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Items.Equipment;
using SzymonPeszek.Items.Consumable;

namespace SzymonPeszek.SaveScripts {
    public class InventoryHolder : MonoBehaviour
    {
        public List<WeaponItem> weaponsInventory;
        public List<WeaponItem> shieldsInventory;
        public List<EquipmentItem> helmetsInventory;
        public List<EquipmentItem> chestsInventory;
        public List<EquipmentItem> shouldersInventory;
        public List<EquipmentItem> handsInventory;
        public List<EquipmentItem> legsInventory;
        public List<EquipmentItem> feetInventory;
        public List<EquipmentItem> ringsInventory;
        public List<ConsumableItem> consumablesInventory;
        public void InitInventoryHolder(DataManager dataManager)
        {
            if(dataManager != null)
            {
                for (int i = 0; i < SettingsHolder.partsID.Length; i++)
                {
                    SettingsHolder.partsID[i] = dataManager.partsID[i];
                    SettingsHolder.partsArmor[i] = dataManager.partsArmor[i];
                }

                if (!dataManager.isFirstStart)
                {

                }
            }

            CurrentEquipments player = GameObject.FindObjectOfType<CurrentEquipments>();
            if (player != null)
            {
                player.InitializeCurrentEquipment();
                player.EquipPlayerWithCurrentItems();
                player.UpdateArmorValue();
            }
        }
    }
}