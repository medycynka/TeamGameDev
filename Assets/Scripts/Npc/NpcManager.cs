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
        public List<QuestContainer> mainQuests = new List<QuestContainer>();
        public List<QuestContainer> sideQuests = new List<QuestContainer>();

        private int _mainQuestCounter;
        private QuestContainer _currentMainQuest;
        private int _sideQuestCounter;

        private void Awake()
        {
            _currentMainQuest = mainQuests[_mainQuestCounter];
            _mainQuestCounter++;
        }

        public Quest GiveMainQuest()
        {
            if (mainQuests.Count > _mainQuestCounter)
            {
                if (_currentMainQuest.isCompleted)
                {
                    if (QuestManager.instance.mainQuests[mainQuests[_mainQuestCounter].prevQuestId].prevQuestId < 0)
                    {
                        _currentMainQuest = mainQuests[_mainQuestCounter];
                        _mainQuestCounter++;

                        return _currentMainQuest.quest;
                    }

                    if (QuestManager.instance.mainQuests[mainQuests[_mainQuestCounter].prevQuestId].isCompleted)
                    {
                        _currentMainQuest = mainQuests[_mainQuestCounter];
                        _mainQuestCounter++;

                        return _currentMainQuest.quest;
                    }
                }

                return _currentMainQuest.quest;
            }

            return null;
        }

        public bool EndCurrentMainQuest(PlayerManager playerManager)
        {
            if (playerManager.CompleteQuest(_currentMainQuest.quest))
            {
                mainQuests.First(q => q == _currentMainQuest).isCompleted = true;
                _currentMainQuest.isCompleted = true;
                
                return true;
            }

            return false;
        }
    }
}