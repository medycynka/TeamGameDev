using System;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Npc;
using SzymonPeszek.Quests;
using SzymonPeszek.SaveScripts;
using UnityEngine;


namespace SzymonPeszek.PlayerScripts
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;

        public List<QuestContainer> mainQuests = new List<QuestContainer>();
        public List<QuestContainer> sideQuests = new List<QuestContainer>();
        public List<Quest> currentQuests = new List<Quest>();
        
        private Dictionary<string, NpcInteractionManager> npcMap;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

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
            
            npcMap = new Dictionary<string, NpcInteractionManager>();
            NpcInteractionManager[] tmp = FindObjectsOfType<NpcInteractionManager>();
            foreach(NpcInteractionManager npc in tmp)
            {
                npcMap.Add(npc.npcManager.npcId, npc);
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
                        
                        if (quest.activateOnGive.Count > 0)
                        {
                            foreach (GameObject toActivate in quest.activateOnGive)
                            {
                                toActivate.SetActive(true);
                            }
                        }
                        
                        if (quest.changeToNextDialogueOnGive.Count > 0)
                        {
                            for (int i = 0; i < quest.changeToNextDialogueOnGive.Count; i++)
                            {
                                npcMap[quest.changeToNextDialogueOnGive[i]].dialogueDataContainer
                                    .First(d => !d.isCompleted).isCompleted = true;
                            }
                        }
                    }
                    else if (mainQuests[prevId].isCompleted)
                    {
                        currentQuests.Add(quest);
                        
                        if (quest.activateOnGive.Count > 0)
                        {
                            foreach (GameObject toActivate in quest.activateOnGive)
                            {
                                toActivate.SetActive(true);
                            }
                        }
                        
                        if (quest.changeToNextDialogueOnGive.Count > 0)
                        {
                            for (int i = 0; i < quest.changeToNextDialogueOnGive.Count; i++)
                            {
                                npcMap[quest.changeToNextDialogueOnGive[i]].dialogueDataContainer
                                    .First(d => !d.isCompleted).isCompleted = true;
                            }
                        }
                    }
                }
            }
        }

        public void CompleteQuest(Quest quest)
        {
            QuestContainer q = mainQuests.Find(p => p.quest = quest);
            q.isCompleted = true;
            SettingsHolder.worldManager.quests[q.questId].isCompleted = true;
            
            if (quest.activateOnComplete.Count > 0)
            {
                foreach (GameObject toActivate in quest.activateOnComplete)
                {
                    toActivate.SetActive(true);
                }
            }

            if (quest.changeToNextDialogueOnComplete.Count > 0)
            {
                for (int i = 0; i < quest.changeToNextDialogueOnComplete.Count; i++)
                {
                    npcMap[quest.changeToNextDialogueOnComplete[i]].dialogueDataContainer
                        .First(d => !d.isCompleted).isCompleted = true;
                }
            }
            
            currentQuests.Remove(quest);
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