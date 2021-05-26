using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SzymonPeszek.Items.Bonfire;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Items.Equipment;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Npc;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Quests;
using SzymonPeszek.SaveScripts;
using QuestContainer = SzymonPeszek.PlayerScripts.QuestContainer;


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
        public QuestContainer[] quests;

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

                #region Quests Initialization
                QuestManager qM = FindObjectOfType<QuestManager>();
                for (int i = 0; i < dataManager.quests.Length; i++)
                {
                    qM.mainQuests.Add(new QuestContainer
                    {
                        prevQuestId = dataManager.quests[i].prevQuestId,
                        quest = GetQuestById(dataManager.quests[i].questId),
                        isCompleted = dataManager.quests[i].isCompleted,
                        questId = dataManager.quests[i].questId
                    });
                }
                if (dataManager.currentQuestIds[0] != -1)
                {
                    for (int i = 0; i < dataManager.currentQuestIds.Length; i++)
                    {
                        qM.currentQuests.Add(quests[dataManager.currentQuestIds[i]].quest);
                    }
                }

                if (dataManager.npcQuests.Length > 0)
                {
                    NpcManager[] npcManagers = FindObjectsOfType<NpcManager>();
                    Dictionary<string, NpcManager> tmp = new Dictionary<string, NpcManager>();
                    foreach (NpcManager npcManager in npcManagers)
                    {
                        npcManager.mainQuests.Clear();
                        tmp.Add(npcManager.npcId, npcManager);
                    }
                    
                    foreach (NpcQuests npcQ in dataManager.npcQuests)
                    {
                        tmp[npcQ.npcId].mainQuests.Add(quests[npcQ.questId]);
                    }
                }
                #endregion
            }
        }

        // private void FixedUpdate()
        // {
        //     if (Time.frameCount % FrameCheckRate == BossCheckVal)
        //     {
        //         for (int i = 0; i < bossAreaManagers.Length; i++)
        //         {
        //             SettingsHolder.bossAreaAlive[i] = bossAreaManagers[i].isBossAlive;
        //         }
        //     }
        //     else if (Time.frameCount % FrameCheckRate == BonfireCheckVal)
        //     {
        //         for (int i = 0; i < bonfireManagers.Length; i++)
        //         {
        //             SettingsHolder.bonfiresActivation[i] = bonfireManagers[i].isActivated;
        //         }
        //     }
        // }

        private Quest GetQuestById(int id)
        {
            for (int i = 0; i < quests.Length; i++)
            {
                if (quests[i].questId == id)
                {
                    return quests[i].quest;
                }
            }

            return null;
        }
    }
}