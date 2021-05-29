using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
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
        [SerializeField] private List<DialogueNodeStorage> dialogueMapValues;

        [HideInInspector] public NpcManager npcManager;

        private void Awake()
        {
            npcManager = GetComponent<NpcManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            InitializeDialogue();

            dialogueUiManager.Init(playerManager, npcManager, this);
            dialogueUiManager.HandleDialogue();
        }

        public void InitializeDialogue()
        {
            // Convert dialogue container data to dictionary for fast and easy access to options of given dialogue node
            if (dialogueDataContainer.Count > 0)
            {
                currentDialogue = dialogueDataContainer.First(d => !d.isCompleted);
                dialogueMapKeys = DialogueDataConverter.ToGuidList(currentDialogue.dialogueData);
                dialogueMapValues = DialogueDataConverter.ToDialogueStorageList(currentDialogue.dialogueData);
                dialogueMap = DialogueDataConverter.ToDictionary(currentDialogue.dialogueData);
            }
        }

        public void GiveQuest(PlayerManager playerManager)
        {
            //base.PickUpItem(playerManager);
            isQuestGiven = true;
            interactableText = "[E] Talk";
            playerManager.AcceptNewQuest(npcManager.GiveMainQuest());
            playerManager.dialogueFlag = false;
        }

        public void CompleteQuest(PlayerManager playerManager)
        {
            if (npcManager.EndCurrentMainQuest(playerManager))
            {
                interactableText = "[E] Talk";
                isQuestGiven = false;
            }
        }

        public bool TryCompleteQuest(PlayerManager playerManager)
        {
            return playerManager.CanCompleteQuest(npcManager.currentMainQuest.quest);
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