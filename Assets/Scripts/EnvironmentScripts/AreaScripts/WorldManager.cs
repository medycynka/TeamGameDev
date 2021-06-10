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
using Random = System.Random;


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
        public List<QuestContainer> quests;

        private const int FrameCheckRate = 3;
        private const int BossCheckVal = 0;
        private const int BonfireCheckVal = 1;
        private static Random _random = new Random();
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{};:<>,./?|";
        
        private void Awake()
        {
            SettingsHolder.worldManager = this;

            if (bossAreaManagers.Length == 0)
            {
                bossAreaManagers = GetComponentsInChildren<BossAreaManager>();
            }
            if (bonfireManagers.Length == 0)
            {
                bonfireManagers = GetComponentsInChildren<BonfireManager>();
            }

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

                NpcManager[] npcManagers = FindObjectsOfType<NpcManager>();
                Dictionary<string, NpcManager> tmp = new Dictionary<string, NpcManager>();
                foreach (NpcManager npcManager in npcManagers)
                {
                    npcManager.mainQuests.Clear();
                    if (tmp.ContainsKey(npcManager.npcId))
                    {
                        npcManager.npcId = npcManager.name + GetRandomNpcIdSuffix(5);
                    }
                    tmp.Add(npcManager.npcId, npcManager);
                }
                if (dataManager.npcQuests.Length > 0)
                {
                    foreach (NpcQuests npcQ in dataManager.npcQuests)
                    {
                        if (quests.Any(q => q.questId == npcQ.questId))
                        {
                            tmp[npcQ.npcId].mainQuests.Add(quests.First(q => q.questId == npcQ.questId));
                        }
                    }
                }
                #endregion

                #region Dialogue Initializetion
                for (int i = 0; i < dataManager.npcDialogues.Length; i++)
                {
                    NpcInteractionManager npcInteract =
                        tmp[dataManager.npcDialogues[i].npcId].GetComponent<NpcInteractionManager>();
                    for (int j = 0; j < dataManager.npcDialogues[i].dialogueCompleted.Length; j++)
                    {
                        npcInteract.dialogueDataContainer[j].isCompleted =
                            dataManager.npcDialogues[i].dialogueCompleted[j];
                    }
                }
                #endregion
            }
        }

        private string GetRandomNpcIdSuffix(int suffixLength = 10, string separator = "_")
        {
            return separator + new string(Enumerable.Repeat(Chars, suffixLength).Select(s => s[_random.Next(s.Length)])
                .ToArray());
        }

        private Quest GetQuestById(int id)
        {
            if (quests.Any(q => q.questId == id))
            {
                return quests.First(q => q.questId == id).quest;
            }

            return null;
        }
    }
}