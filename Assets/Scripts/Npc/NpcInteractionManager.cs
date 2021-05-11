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
        [Serializable]
        public class LinkData
        {
            public string targetGuid;
            public string portName;
        }
        
        [Serializable]
        public class DialogueNodeStorage
        {
            public string guid;
            public string dialogueText;
            public bool giver;
            public bool completer;
            public bool exit;
            public List<LinkData> links;
        }
        
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
            dialogueMapKeys = new List<string>();
            dialogueMapValues = new List<DialogueNodeStorage>();
            dialogueMap = new Dictionary<string, DialogueNodeStorage>();
            foreach (var dialogueNode in dialogueData.dialogueNodeData)
            {
                dialogueMapKeys.Add(dialogueNode.nodeGuid);
                dialogueMapValues.Add(new DialogueNodeStorage
                {
                    guid = dialogueNode.nodeGuid,
                    dialogueText = dialogueNode.dialogueText,
                    giver = dialogueNode.isQuestGiver,
                    completer = dialogueNode.isQuestCompleter,
                    exit = dialogueNode.isExit,
                    links = GetNodeLinks(dialogueNode.nodeGuid)
                });
            }
            for (int i = 0; i < dialogueMapKeys.Count; i++)
            {
                dialogueMap.Add(dialogueMapKeys[i], dialogueMapValues[i]);
            }
        }

        private List<LinkData> GetNodeLinks(string guid)
        {
            List<LinkData> nodeLinks = new List<LinkData>();

            dialogueData.nodeLinks.ForEach(link =>
            {
                if (link.baseNodeGuid == guid)
                {
                    nodeLinks.Add(new LinkData
                    {
                        targetGuid = link.targetNodeGuid,
                        portName = link.portName
                    });
                }
            });
            
            return nodeLinks;
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