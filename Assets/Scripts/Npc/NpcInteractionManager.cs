using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Npc.DialogueSystem;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using SzymonPeszek.PlayerScripts;
using UnityEngine;


namespace SzymonPeszek.Npc
{
    public class NpcInteractionManager : Interactable
    {
        public List<DialogueInfo> dialogueDataContainer;
        public DialogueUiManager dialogueUiManager;
        public bool isQuestGiven;
        public Dictionary<string, DialogueNodeStorage> dialogueMap;
        public DialogueInfo currentDialogue;
        
        //For "simulating" dialogue map in the inspector
        [SerializeField] private List<string> dialogueMapKeys;
        [SerializeField] [ItemCanBeNull] private List<DialogueNodeStorage> dialogueMapValues;

        private NpcManager _npcManager;

        private void Awake()
        {
            _npcManager = GetComponent<NpcManager>();

            InitializeDialogue();
        }

        public override void Interact(PlayerManager playerManager)
        {
            InitializeDialogue();
            
            if (!dialogueUiManager.isInitialized)
            {
                dialogueUiManager.Init(playerManager, _npcManager, this);
            }
            
            dialogueUiManager.HandleDialogue();
        }

        public void InitializeDialogue()
        {
            // Convert dialogue container data to dictionary for fast and easy access to options of given dialogue node
            currentDialogue = dialogueDataContainer.First(d => !d.isCompleted);
            dialogueMapKeys = DialogueDataConverter.ToGuidList(currentDialogue.dialogueData);
            dialogueMapValues = DialogueDataConverter.ToDialogueStorageList(currentDialogue.dialogueData);
            dialogueMap = DialogueDataConverter.ToDictionary(currentDialogue.dialogueData);
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

    [Serializable]
    public class DialogueInfo
    {
        public int dialogueId;
        public bool isCompleted;
        public DialogueContainer dialogueData;
    }
}