using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SzymonPeszek.Npc;
using SzymonPeszek.Quests;
using SzymonPeszek.SaveScripts;
using UnityEditor.UIElements;
using UnityEngine;


namespace SzymonPeszek.PlayerScripts
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance => _instance;

        public List<QuestContainer> mainQuests = new List<QuestContainer>();
        public List<QuestContainer> sideQuests = new List<QuestContainer>();
        public List<Quest> currentQuests = new List<Quest>();
        public Dictionary<string, NpcInteractionManager> npcMap;

        private static QuestManager _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            } else {
                _instance = this;
            }

            if (SettingsHolder.firstStart)
            {
                foreach (QuestContainer quest in SettingsHolder.worldManager.quests)
                {
                    instance.mainQuests.Add(new QuestContainer
                    {
                        prevQuestId = quest.prevQuestId,
                        questId = quest.questId,
                        quest = quest.quest,
                        isCompleted = quest.isCompleted
                    });
                }
            }

            npcMap = new Dictionary<string, NpcInteractionManager>();
            NpcInteractionManager[] tmp = FindObjectsOfType<NpcInteractionManager>();
            foreach(NpcInteractionManager npc in tmp)
            {
                if (npc.npcManager != null)
                {
                    if (!npcMap.ContainsKey(npc.npcManager.npcId))
                    {
                        npcMap.Add(npc.npcManager.npcId, npc);
                    }
                }
                else
                {
                    NpcManager tmpManager = npc.GetComponent<NpcManager>();
                    if (!npcMap.ContainsKey(tmpManager.npcId))
                    {
                        npcMap.Add(tmpManager.npcId, npc);
                    }
                }
            }
        }

        public void AddNewQuest(Quest quest)
        {
            if (!currentQuests.Contains(quest))
            {
                if(mainQuests.Any(p => p.quest = quest))
                {
                    int prevId = mainQuests.Find(p => p.quest = quest).prevQuestId;

                    if (prevId < 0)
                    {
                        currentQuests.Add(quest);
                        
                        if (quest.switchActiveStateOnGive.Count > 0)
                        {
                            foreach (GameObject toActivate in quest.switchActiveStateOnGive)
                            {
                                toActivate.SetActive(!toActivate.activeSelf);
                            }
                        }
                        
                        if (quest.changeToNextDialogueOnGive.Count > 0)
                        {
                            for (int i = 0; i < quest.changeToNextDialogueOnGive.Count; i++)
                            {
                                if (npcMap[quest.changeToNextDialogueOnGive[i]].dialogueDataContainer
                                    .Any(d => !d.isCompleted))
                                {
                                    npcMap[quest.changeToNextDialogueOnGive[i]].dialogueDataContainer
                                        .First(d => !d.isCompleted).isCompleted = true;
                                }
                            }
                        }
                    }
                    else if (mainQuests.Any(q => q.questId == prevId))
                    {
                        if (mainQuests.First(q => q.questId == prevId).isCompleted)
                        {
                            currentQuests.Add(quest);

                            if (quest.switchActiveStateOnGive.Count > 0)
                            {
                                foreach (GameObject toActivate in quest.switchActiveStateOnGive)
                                {
                                    toActivate.SetActive(!toActivate.activeSelf);
                                }
                            }

                            if (quest.changeToNextDialogueOnGive.Count > 0)
                            {
                                for (int i = 0; i < quest.changeToNextDialogueOnGive.Count; i++)
                                {
                                    if (npcMap[quest.changeToNextDialogueOnGive[i]].dialogueDataContainer
                                        .Any(d => !d.isCompleted))
                                    {
                                        npcMap[quest.changeToNextDialogueOnGive[i]].dialogueDataContainer
                                            .First(d => !d.isCompleted).isCompleted = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CompleteQuest(Quest quest)
        {
            Debug.Log("QuestManager: CompleteQuest");
            if (mainQuests.Any(q => q.quest == quest))
            {
                Debug.Log("QuestManager: CompleteQuest - quest found");
                QuestContainer q = mainQuests.First(p => p.quest == quest);
                q.isCompleted = true;
                SettingsHolder.worldManager.quests[q.questId].isCompleted = true;

                if (quest.switchActiveStateOnComplete.Count > 0)
                {
                    Debug.Log("Activating objects...");
                    foreach (GameObject toActivate in quest.switchActiveStateOnComplete)
                    {
                        Debug.Log($"{(toActivate.activeSelf ? "Deactivating" : "Activating")} {toActivate.name}");
                        toActivate.SetActive(!toActivate.activeSelf);
                    }
                }

                if (quest.changeToNextDialogueOnComplete.Count > 0)
                {
                    for (int i = 0; i < quest.changeToNextDialogueOnComplete.Count; i++)
                    {
                        if (npcMap[quest.changeToNextDialogueOnComplete[i]].dialogueDataContainer
                            .Any(d => !d.isCompleted))
                        {
                            npcMap[quest.changeToNextDialogueOnComplete[i]].dialogueDataContainer
                                .First(d => !d.isCompleted).isCompleted = true;
                        }
                    }
                }

                currentQuests.Remove(quest);
            }
        }
    }

    [Serializable]
    public class QuestContainer
    {
        [Tooltip("Previous quest's id. Set it to -1 if quest doesn't have any prerequisite, otherwise set to prerequisite quest's id")]
        public int prevQuestId;
        [Tooltip("Quest's id")]
        public int questId;
        [Tooltip("Actual quest scriptable object")]
        public Quest quest;
        [Tooltip("Is quest completed?")]
        public bool isCompleted;
    }
}