using System;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Quests;
using UnityEngine;


namespace SzymonPeszek.PlayerScripts
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;

        public List<QuestContainer> mainQuests = new List<QuestContainer>();
        public List<QuestContainer> sideQuests = new List<QuestContainer>();
        
        private List<Quest> _currentQuests = new List<Quest>();

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
        }

        public void AddNewQuest(Quest quest)
        {
            if (!_currentQuests.Contains(quest))
            {
                if(mainQuests.Any(p => p.quest = quest))
                {
                    int prevId = mainQuests.Find(p => p.quest = quest).prevQuestId;

                    if (prevId < 0)
                    {
                        _currentQuests.Add(quest);
                    }
                    else if (mainQuests[prevId].isCompleted)
                    {
                        _currentQuests.Add(quest);
                    }
                }
            }
        }

        public void CompleteQuest(Quest quest)
        {
            mainQuests.Find(p => p.quest = quest).isCompleted = true;
            _currentQuests.Remove(quest);
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