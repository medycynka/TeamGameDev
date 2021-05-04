using System;
using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Npc.DialogueSystem;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using SzymonPeszek.PlayerScripts;
using UnityEngine;


namespace SzymonPeszek.Npc
{
    public class NpcInteractionManager : Interactable
    {
        public DialogueContainer dialogueData;
        public DialogueUiManager dialogueUiManager;
        public bool isQuestGiven;
        
        private NpcManager _npcManager;

        private void Awake()
        {
            _npcManager = GetComponent<NpcManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (!dialogueUiManager.isInitialized)
            {
                dialogueUiManager.Init(playerManager, _npcManager, this);
            }
            
            dialogueUiManager.HandleDialogue();
        }

        public void GiveQuest(PlayerManager playerManager)
        {
            //base.PickUpItem(playerManager);
            isQuestGiven = true;
            interactableText = "Complete a quest [E]";
            playerManager.AcceptNewQuest(_npcManager.GiveMainQuest());
            playerManager.dialogueFlag = false;
        }

        public void CompleteQuest(PlayerManager playerManager)
        {
            if (_npcManager.EndCurrentMainQuest(playerManager))
            {
                Debug.Log("Quest is finished!");
                interactableText = "[E] Talk";
                isQuestGiven = false;
            }
            else
            {
                Debug.Log("Quest is not finished!");
            }
        }
    }
}