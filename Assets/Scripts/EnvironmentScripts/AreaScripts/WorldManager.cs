using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.Items.Bonfire;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Items.Equipment;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.SaveScripts;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.Environment.Areas
{
    /// <summary>
    /// Class for monitoring area events and loading saved inventory
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        public BossAreaManager[] bossAreaManagers;
        public BonfireManager[] bonfireManagers;

        public WeaponItem[] weaponsHolder;
        public WeaponItem[] shieldsHolder;
        public EquipmentItem[] helmetsHolder;
        public EquipmentItem[] chestsHolder;
        public EquipmentItem[] shouldersHolder;
        public EquipmentItem[] handsHolder;
        public EquipmentItem[] legsHolder;
        public EquipmentItem[] feetHolder;
        public EquipmentItem[] ringsHolder;
        public ConsumableItem[] consumableHolder;

        private const int FrameCheckRate = 3;
        private const int BossCheckVal = 0;
        private const int BonfireCheckVal = 1;
        
        private void Awake()
        {
            SettingsHolder.worldManager = this;
            
            bossAreaManagers = GetComponentsInChildren<BossAreaManager>();
            bonfireManagers = GetComponentsInChildren<BonfireManager>();

            DataManager dataManager = SettingsHolder.dataManager;

            if (dataManager != null)
            {
                if (!dataManager.isFirstStart)
                {
                    #region Boss Initializetion
                    for (int i = 0; i < bossAreaManagers.Length; i++)
                    {
                        bossAreaManagers[i].isBossAlive = dataManager.areaBossesAlive[i];
                    }
                    #endregion

                    #region Bonfire Initialization
                    for (int i = 0; i < bonfireManagers.Length; i++)
                    {
                        bonfireManagers[i].isActivated = dataManager.bonfireActivators[i];
                        bonfireManagers[i].showRestPopUp = dataManager.bonfireActivators[i];
                    }
                    #endregion
                }
                
                #region Current Equipment Initialization
                for (int i = 0; i < SettingsHolder.partsID.Length; i++)
                {
                    SettingsHolder.partsID[i] = dataManager.partsID[i];
                    SettingsHolder.partsArmor[i] = dataManager.partsArmor[i];
                }
                
                // if (playerEq != null)
                // {
                //     playerEq.InitializeCurrentEquipment();
                //     playerEq.EquipPlayerWithCurrentItems();
                //     playerEq.UpdateArmorValue();
                // }
                #endregion
            }
        }

        private void FixedUpdate()
        {
            if (Time.frameCount % FrameCheckRate == BossCheckVal)
            {
                for (int i = 0; i < bossAreaManagers.Length; i++)
                {
                    SettingsHolder.bossAreaAlive[i] = bossAreaManagers[i].isBossAlive;
                }
            }
            else if (Time.frameCount % FrameCheckRate == BonfireCheckVal)
            {
                for (int i = 0; i < bonfireManagers.Length; i++)
                {
                    SettingsHolder.bonfiresActivation[i] = bonfireManagers[i].isActivated;
                }
            }
        }
    }
}