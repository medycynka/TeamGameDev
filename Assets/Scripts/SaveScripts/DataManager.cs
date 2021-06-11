using System;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.Enums;
using SzymonPeszek.Environment;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Npc;
using UnityEngine;
using Object = UnityEngine.Object;


namespace SzymonPeszek.SaveScripts
{
    /// <summary>
    /// Storage class for data that will be saved (and loaded) 
    /// </summary>
    [System.Serializable]
    public class DataManager
    {
        // Settings
        public int resolutionID;
        public bool isFullscreen;
        public int qualityID;
        public float mouseSensibility;
        public float soundVolume;

        // Main Menu
        public bool isCharacterCreated;
        public bool isFirstStart;
        public string playerName;
        public bool isMale;

        // Player
        public int[] partsID;
        public float[] partsArmor;
        public float currentHealth;
        public float currentStamina;
        public float baseArmor;
        public float strength;
        public float agility;
        public float defence;
        public float bonusHealth;
        public float bonusStamina;
        public float bonusFocus;
        public int playerLevel;
        public float soulsAmount;
        public float[] spawnPointPosition;
        public float[] spawnPointRotation;
        
        // Player Inventory
        public int[] leftHandSlots;
        public int[] rightHandSlots;
        public int[] weaponIds;
        public int[] shieldIds;
        public int[] helmetIds;
        public int[] chestIds;
        public int[] shoulderIds;
        public int[] handIds;
        public int[] legIds;
        public int[] footIds;
        public int[] ringIds;
        public int[] consumableIds;

        // Area Managers
        public bool[] areaBossesAlive;
        public bool[] bonfireActivators;
        
        //Quests
        public QuestContainer[] quests;
        public int[] currentQuestIds;
        
        // Npcs
        public NpcQuests[] npcQuests;
        public NpcDialogues[] npcDialogues;
        
        // PickUps
        public PickUpsActivation[] pickUps;
        
        // Switching active state objects
        public SwitchObjectActiveStateInfo[] actives;

        // Constructor
        /// <summary>
        /// Constructor for saving data from main menu
        /// </summary>
        public DataManager()
        {
            resolutionID = SettingsHolder.resolutionID;
            isFullscreen = SettingsHolder.isFullscreen;
            qualityID = SettingsHolder.qualityID;
            mouseSensibility = SettingsHolder.mouseSensibility;
            soundVolume = SettingsHolder.soundVolume;
            isCharacterCreated = SettingsHolder.isCharacterCreated;
            isFirstStart = SettingsHolder.firstStart;
            playerName = SettingsHolder.playerName;
            isMale = SettingsHolder.isMale;
            strength = SettingsHolder.currentStrength;
            agility = SettingsHolder.currentAgility;
            defence = SettingsHolder.currentDefence;
            bonusHealth = SettingsHolder.currentBonusHealth;
            bonusStamina = SettingsHolder.currentBonusStamina;
            bonusFocus = SettingsHolder.currentBonusFocus;
            playerLevel = SettingsHolder.currentLevel;
            soulsAmount = SettingsHolder.soulsAmount;

            partsID = new int[SettingsHolder.partsID.Length];
            partsArmor = new float[SettingsHolder.partsID.Length];

            for(int i = 0; i < SettingsHolder.partsID.Length; i++)
            {
                partsID[i] = SettingsHolder.partsID[i];
                partsArmor[i] = SettingsHolder.partsArmor[i];
            }
        }

        /// <summary>
        /// Constructor for saving data during game
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        /// <param name="playerStats">Player Stats</param>
        /// <param name="playerInventory">Player inventory</param>
        public DataManager(PlayerManager playerManager, PlayerStats playerStats, PlayerInventory playerInventory)
        {
            #region Setting
            resolutionID = SettingsHolder.resolutionID;
            isFullscreen = SettingsHolder.isFullscreen;
            qualityID = SettingsHolder.qualityID;
            mouseSensibility = SettingsHolder.mouseSensibility;
            soundVolume = SettingsHolder.soundVolume;
            isCharacterCreated = SettingsHolder.isCharacterCreated;
            isFirstStart = SettingsHolder.firstStart;
            playerName = SettingsHolder.playerName;
            isMale = SettingsHolder.isMale;
            #endregion

            partsID = new int[SettingsHolder.partsID.Length];
            partsArmor = new float[SettingsHolder.partsID.Length];

            for (var i = 0; i < SettingsHolder.partsID.Length; i++)
            {
                partsID[i] = SettingsHolder.partsID[i];
                partsArmor[i] = SettingsHolder.partsArmor[i];
            }

            #region Stats
            baseArmor = playerStats.baseArmor;
            strength = playerStats.strength;
            agility = playerStats.agility;
            defence = playerStats.defence;
            bonusHealth = playerStats.bonusHealth;
            bonusStamina = playerStats.bonusStamina;
            bonusFocus = playerStats.bonusFocus;
            playerLevel = playerStats.playerLevel;
            soulsAmount = playerStats.soulsAmount;

            spawnPointPosition = new float[3];
            var position = playerManager.currentSpawnPoint.transform.position;
            spawnPointPosition[0] = position.x;
            spawnPointPosition[1] = position.y;
            spawnPointPosition[2] = position.z;

            spawnPointRotation = new float[3];
            var rotation = playerManager.currentSpawnPoint.transform.rotation;
            spawnPointRotation[0] = rotation.eulerAngles.x;
            spawnPointRotation[1] = rotation.eulerAngles.y;
            spawnPointRotation[2] = rotation.eulerAngles.z;
            #endregion

            #region Area
            areaBossesAlive = new bool[SettingsHolder.bossAreaAlive.Length];
            for(int i = 0; i < areaBossesAlive.Length; i++)
            {
                areaBossesAlive[i] = SettingsHolder.bossAreaAlive[i];
            }

            bonfireActivators = new bool[SettingsHolder.bonfiresActivation.Length];
            for(int i = 0; i < bonfireActivators.Length; i++)
            {
                bonfireActivators[i] = SettingsHolder.bonfiresActivation[i];
            }
            #endregion
            
            #region Inventory
            #region Quick Slots
            leftHandSlots = new int[4];
            leftHandSlots[0] = playerInventory.weaponsInLeftHandSlots[0].meleeType == MeleeType.Shield ? 1 : 0;
            leftHandSlots[1] = playerInventory.weaponsInLeftHandSlots[0].itemId;
            leftHandSlots[2] = playerInventory.weaponsInLeftHandSlots[1].meleeType == MeleeType.Shield ? 1 : 0;
            leftHandSlots[3] = playerInventory.weaponsInLeftHandSlots[1].itemId;

            rightHandSlots = new int[4];
            rightHandSlots[0] = playerInventory.weaponsInRightHandSlots[0].meleeType == MeleeType.Shield ? 1 : 0;
            rightHandSlots[1] = playerInventory.weaponsInRightHandSlots[0].itemId;
            rightHandSlots[2] = playerInventory.weaponsInRightHandSlots[1].meleeType == MeleeType.Shield ? 1 : 0;
            rightHandSlots[3] = playerInventory.weaponsInRightHandSlots[1].itemId;
            #endregion

            #region Current Weapons in Inventory
            int count = 0;
            var currentWeapons = playerInventory.weaponsInventory.GroupBy(i => i.itemId).ToList();
            weaponIds = new int[2 * currentWeapons.Count];
            foreach (var kvp in currentWeapons)
            {
                weaponIds[count] = kvp.Key;
                count++;
                weaponIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Shield in Inventory
            count = 0;
            currentWeapons = playerInventory.shieldsInventory.GroupBy(i => i.itemId).ToList();
            shieldIds = new int[2 * currentWeapons.Count];
            foreach (var kvp in currentWeapons)
            {
                shieldIds[count] = kvp.Key;
                shieldIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Helmets in Inventory
            count = 0;
            var currentEq = playerInventory.helmetsInventory.GroupBy(i => i.itemId).ToList();
            helmetIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                helmetIds[count] = kvp.Key;
                count++;
                helmetIds[count] = kvp.Count();
                count++;
            }
            #endregion

            #region Current Chest Armor in Inventory
            count = 0;
            currentEq = playerInventory.chestsInventory.GroupBy(i => i.itemId).ToList();
            chestIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                chestIds[count] = kvp.Key;
                count++;
                chestIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Shoulder Armor in Inventory
            count = 0;
            currentEq = playerInventory.shouldersInventory.GroupBy(i => i.itemId).ToList();
            shoulderIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                shoulderIds[count] = kvp.Key;
                shoulderIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Hand Armor in Inventory
            count = 0;
            currentEq = playerInventory.handsInventory.GroupBy(i => i.itemId).ToList();
            handIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                handIds[count] = kvp.Key;
                count++;
                handIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Leg Armor in Inventory
            count = 0;
            currentEq = playerInventory.legsInventory.GroupBy(i => i.itemId).ToList();
            legIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                legIds[count] = kvp.Key;
                count++;
                legIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Foot Armor in Inventory
            count = 0;
            currentEq = playerInventory.feetInventory.GroupBy(i => i.itemId).ToList();
            footIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                footIds[count] = kvp.Key;
                count++;
                footIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Rings in Inventory
            count = 0;
            currentEq = playerInventory.ringsInventory.GroupBy(i => i.itemId).ToList();
            ringIds = new int[2 * currentEq.Count];
            foreach (var kvp in currentEq)
            {
                ringIds[count] = kvp.Key;
                count++;
                ringIds[count] = kvp.Count();
                count++;
            }
            #endregion
            
            #region Current Shoulder Armor in Inventory
            count = 0;
            var currentCons = playerInventory.consumablesInventory.GroupBy(i => i.itemId).ToList();
            consumableIds = new int[2 * currentCons.Count];
            foreach (var kvp in currentCons)
            {
                consumableIds[count] = kvp.Key;
                count++;
                consumableIds[count] = kvp.Count();
                count++;
            }
            #endregion

            #endregion

            #region Quests
            // Player
            quests = new QuestContainer[SettingsHolder.worldManager.quests.Count];
            for (int i = 0; i < SettingsHolder.worldManager.quests.Count; i++)
            {
                quests[i] = new QuestContainer
                {
                    prevQuestId = SettingsHolder.worldManager.quests[i].prevQuestId,
                    questId = SettingsHolder.worldManager.quests[i].questId,
                    isCompleted = SettingsHolder.worldManager.quests[i].isCompleted
                };
            }

            if (QuestManager.instance.currentQuests.Count > 0)
            {
                currentQuestIds = new int[QuestManager.instance.currentQuests.Count];
                for (int i = 0; i < currentQuestIds.Length; i++)
                {
                    currentQuestIds[i] =
                        QuestManager.instance.mainQuests.FindIndex(q =>
                            q.quest == QuestManager.instance.currentQuests[i]);
                }
            }
            else
            {
                currentQuestIds = new[] {-1};
            }
            
            // NPC
            NpcManager[] npcs = Object.FindObjectsOfType<NpcManager>();
            List<NpcQuests> tmpNpcQuestList = new List<NpcQuests>();
            foreach (NpcManager npc in npcs)
            {
                foreach (var qC in npc.mainQuests)
                {
                    tmpNpcQuestList.Add(new NpcQuests
                    {
                        npcId = npc.npcId,
                        questId = qC.questId
                    });
                }
            }
            npcQuests = tmpNpcQuestList.ToArray();
            #endregion
            
            #region Dialogues
            npcDialogues = new NpcDialogues[npcs.Length];
            for (int i = 0; i < npcDialogues.Length; i++)
            {
                NpcInteractionManager npcInteract = npcs[i].GetComponent<NpcInteractionManager>();
                npcDialogues[i] = new NpcDialogues
                {
                    npcId = npcs[i].npcId,
                    dialogueCompleted = new bool[npcInteract.dialogueDataContainer.Count]
                };
                for (int j = 0; j < npcDialogues[i].dialogueCompleted.Length; j++)
                {
                    npcDialogues[i].dialogueCompleted[j] = npcInteract.dialogueDataContainer[j].isCompleted;
                }
            }
            #endregion
            
            #region Pick Ups
            pickUps = WorldPickUpsManager.instance.SavePickUps();
            #endregion
            
            #region Activating items
            List<SwitchObjectActiveStateInfo> tmp = new List<SwitchObjectActiveStateInfo>();
            foreach (var aO in SwitchObjectActiveState.activeObjectsMap)
            {
                tmp.Add(new SwitchObjectActiveStateInfo
                {
                    id = aO.Key,
                    isActive = aO.Value.currentActiveState
                });
            }
            actives = tmp.ToArray();
            #endregion
        }
    }

    
    [Serializable]
    public class QuestContainer
    {
        public int prevQuestId;
        public int questId;
        public bool isCompleted;
    }

    [Serializable]
    public class NpcQuests
    {
        public string npcId;
        public int questId;
    }

    [Serializable]
    public class NpcDialogues
    {
        public string npcId;
        public bool[] dialogueCompleted;
    }

    [Serializable]
    public class PickUpsActivation
    {
        public string pickUpId;
        public bool isCollected;
    }

    [Serializable]
    public class SwitchObjectActiveStateInfo
    {
        public string id;
        public bool isActive;
    }
}