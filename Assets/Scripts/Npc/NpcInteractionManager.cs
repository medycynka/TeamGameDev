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
        public Dictionary<string, DialogueNodeStorage> dialogueMap;
        
        //For "simulating" dialogue map in the inspector
        [SerializeField] private List<string> dialogueMapKeys;
        [SerializeField] private List<DialogueNodeStorage> dialogueMapValues;

        private NpcManager _npcManager;

        private void Awake()
        {
            _npcManager = GetComponent<NpcManager>();

            // Convert dialogue container data to dictionary for fast and easy access to options of given dialogue node
            dialogueMapKeys = DialogueDataConverter.ToGuidList(dialogueData);
            dialogueMapValues = DialogueDataConverter.ToDialogueStorageList(dialogueData);
            dialogueMap = DialogueDataConverter.ToDictionary(dialogueData);
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
                interactableText = "[E] Talk";
                isQuestGiven = false;
            }
        }
    }
}