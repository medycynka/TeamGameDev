using System;
using System.Collections.Generic;
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

        public void EndCurrentMainQuestQuest(PlayerManager playerManager)
        {
            playerManager.CompleteQuest(_currentMainQuest.quest);
        }
    }
}