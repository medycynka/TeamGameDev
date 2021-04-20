using System;
using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using UnityEngine;


namespace SzymonPeszek.Npc
{
    public class NpcInteractionManager : Interactable
    {
        private NpcManager _npcManager;
        private bool _isQuestGiven;

        private void Awake()
        {
            _npcManager = GetComponent<NpcManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (_isQuestGiven)
            {
                CompleteQuest(playerManager);
            }
            else
            {
                GiveQuest(playerManager);
            }
        }

        private void GiveQuest(PlayerManager playerManager)
        {
            //base.PickUpItem(playerManager);
            _isQuestGiven = true;
            interactableText = "Complete a quest [E]";
            playerManager.AcceptNewQuest(_npcManager.GiveMainQuest());
        }

        private void CompleteQuest(PlayerManager playerManager)
        {
            if (_npcManager.EndCurrentMainQuest(playerManager))
            {
                Debug.Log("Quest is finished!");
                interactableText = "Take a quest [E]";
                _isQuestGiven = false;
            }
            else
            {
                Debug.Log("Quest is not finished!");
            }
        }
    }
}