using System;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Quests;
using UnityEngine;


namespace SzymonPeszek.Npc
{
    public class NpcManager : CharacterManager
    {
        public string npcId = "name_number";
        public List<QuestContainer> mainQuests = new List<QuestContainer>();
        public List<QuestContainer> sideQuests = new List<QuestContainer>();
        public QuestContainer currentMainQuest;
        public QuestContainer currentSideQuest;

        private void Awake()
        {
            if (mainQuests.Count > 0)
            {
                currentMainQuest = mainQuests.First(q => !q.isCompleted);
            }
            if (sideQuests.Count > 0)
            {
                currentSideQuest = sideQuests.First(q => !q.isCompleted);
            }
        }

        public Quest GiveMainQuest()
        {
            if (!currentMainQuest.isCompleted)
            {
                return currentMainQuest.quest;
            }
            if (mainQuests.Any(q => !q.isCompleted))
            {
                if (currentMainQuest.isCompleted && 
                    QuestManager.instance.mainQuests.Any(q => !q.isCompleted))
                {
                    currentMainQuest = mainQuests.First(q => !q.isCompleted);
                    if (currentMainQuest.prevQuestId < 0 || QuestManager.instance.mainQuests.First(q =>
                        q.questId == currentMainQuest.prevQuestId).isCompleted)
                    {
                        return currentMainQuest.quest;
                    }
                }
                else
                {
                    if (currentMainQuest.prevQuestId < -1)
                    {
                        return currentMainQuest.quest;
                    }
                    
                    if (QuestManager.instance.mainQuests.Any(q => q.questId == currentMainQuest.prevQuestId))
                    {
                        if (QuestManager.instance.mainQuests.First(q => q.questId == currentMainQuest.prevQuestId)
                            .isCompleted)
                        {
                            return currentMainQuest.quest;
                        }
                    }
                }
            }

            return null;
        }

        public bool EndCurrentMainQuest(PlayerManager playerManager)
        {
            Debug.Log("NpcManager: Complete quest");
            if (currentMainQuest.quest == null)
            {
                Debug.Log("Current quest is null");
            }
            if (playerManager.CompleteQuest(currentMainQuest.quest))
            {
                Debug.Log("NpcManager: Completed quest");
                mainQuests.First(q => q.questId == currentMainQuest.questId).isCompleted = true;
                currentMainQuest.isCompleted = true;

                if (currentMainQuest.quest.isEndingNotInGiver)
                {
                    QuestManager.instance.npcMap[currentMainQuest.quest.giverNpcId].isQuestGiven = false;
                    QuestManager.instance.npcMap[currentMainQuest.quest.giverNpcId].npcManager.currentMainQuest.isCompleted = true;
                }
                
                return true;
            }

            return false;
        }
        
        public Quest GiveSideQuest()
        {
            if (sideQuests.Any(q => !q.isCompleted))
            {
                if (currentSideQuest.isCompleted)
                {
                    currentSideQuest = sideQuests.First(q => !q.isCompleted);
                    if (QuestManager.instance.sideQuests.First(q => q == currentSideQuest).prevQuestId < 0 || 
                        QuestManager.instance.sideQuests.First(q => q.questId == currentSideQuest.prevQuestId).isCompleted)
                    {
                        return currentSideQuest.quest;
                    }
                }
                else
                {
                    return currentSideQuest.quest;
                }
            }

            return null;
        }

        public bool EndCurrentSideQuest(PlayerManager playerManager)
        {
            if (playerManager.CompleteQuest(currentSideQuest.quest))
            {
                sideQuests.First(q => q == currentSideQuest).isCompleted = true;
                currentSideQuest.isCompleted = true;
                
                return true;
            }

            return false;
        }
    }
}