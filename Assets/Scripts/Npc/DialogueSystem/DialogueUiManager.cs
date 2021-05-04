using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Misc;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using SzymonPeszek.PlayerScripts;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace SzymonPeszek.Npc.DialogueSystem
{
    public class DialogueUiManager : MonoBehaviour
    {
        [Serializable]
        public class DialogueOption
        {
            public GameObject optionObject;
            public Button button;
            public TextMeshProUGUI optionText;
        }
        
        public GameObject hudWindow;
        public GameObject uIWindow;
        public GameObject dialogueWindow;
        public TextMeshProUGUI mainText;
        public DialogueOption[] options;
        public bool isInitialized;

        private PlayerManager _playerManager;
        private NpcManager _npcManager;
        private NpcInteractionManager _npcInteractionManager;

        public void Init(PlayerManager playerManager, NpcManager npcManager, 
            NpcInteractionManager npcInteractionManager)
        {
            _playerManager = playerManager;
            _npcManager = npcManager;
            _npcInteractionManager = npcInteractionManager;
            isInitialized = true;
        }
        public void HandleDialogue()
        {
            _playerManager.dialogueFlag = true;
            mainText.text = _npcInteractionManager.dialogueData.dialogueNodeData[0].dialogueText;
            string baseGuid = _npcInteractionManager.dialogueData.dialogueNodeData[0].nodeGuid;
            hudWindow.SetActive(false);
            uIWindow.SetActive(true);
            dialogueWindow.SetActive(true);
            List<NodeLinkData> availableOptions = _npcInteractionManager.dialogueData.nodeLinks.Where(p => p.baseNodeGuid == baseGuid)
                .ToList();
            DialogueNodeData tmpData; // For optional hiding options
            switch (availableOptions.Count)
            {
                case 1:
                    tmpData = _npcInteractionManager.dialogueData.dialogueNodeData
                        .First(p => p.nodeGuid == availableOptions[0].targetNodeGuid);
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(()=>{HandleDialogueOption(availableOptions[0].targetNodeGuid);});
                    options[0].optionText.text = tmpData.dialogueText;
                    break;
                case 2:
                    tmpData = _npcInteractionManager.dialogueData.dialogueNodeData
                        .First(p => p.nodeGuid == availableOptions[0].targetNodeGuid);
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(()=>{HandleDialogueOption(availableOptions[0].targetNodeGuid);});
                    options[0].optionText.text = tmpData.dialogueText;
                    tmpData = _npcInteractionManager.dialogueData.dialogueNodeData
                        .First(p => p.nodeGuid == availableOptions[1].targetNodeGuid);
                    options[1].optionObject.SetActive(true);
                    options[1].button.onClick.AddListener(()=>{HandleDialogueOption(availableOptions[1].targetNodeGuid);});
                    options[1].optionText.text = tmpData.dialogueText;
                    break;
                case 3:
                    tmpData = _npcInteractionManager.dialogueData.dialogueNodeData
                        .First(p => p.nodeGuid == availableOptions[0].targetNodeGuid);
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(()=>{HandleDialogueOption(availableOptions[0].targetNodeGuid);});
                    options[0].optionText.text = tmpData.dialogueText;
                    tmpData = _npcInteractionManager.dialogueData.dialogueNodeData
                        .First(p => p.nodeGuid == availableOptions[1].targetNodeGuid);
                    options[1].optionObject.SetActive(true);
                    options[1].button.onClick.AddListener(()=>{HandleDialogueOption(availableOptions[1].targetNodeGuid);});
                    options[1].optionText.text = tmpData.dialogueText;
                    tmpData = _npcInteractionManager.dialogueData.dialogueNodeData
                        .First(p => p.nodeGuid == availableOptions[2].targetNodeGuid);
                    options[2].optionObject.SetActive(true);
                    options[2].button.onClick.AddListener(()=>{HandleDialogueOption(availableOptions[2].targetNodeGuid);});
                    options[2].optionText.text = tmpData.dialogueText;
                    break;
                default:
                    break;
            }
        }

        private void HandleDialogueOption(string targetGuid)
        {
            if (_npcInteractionManager.dialogueData.dialogueNodeData
                .First(p => p.nodeGuid == targetGuid).dialogueText == "Exit")
            {
                CloseDialogue();
            }
            else
            {
                if (_npcInteractionManager.dialogueData.dialogueNodeData
                    .First(p => p.nodeGuid == targetGuid).isQuestGiver && !_npcInteractionManager.isQuestGiven)
                {
                    foreach (DialogueOption option in options)
                    {
                        option.optionObject.SetActive(false);
                    }
                    
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(GiveQuest);
                    options[0].optionText.text = "Exit";
                }
                else if(_npcInteractionManager.dialogueData.dialogueNodeData
                    .First(p => p.nodeGuid == targetGuid).isQuestCompleter && _npcInteractionManager.isQuestGiven)
                {
                    foreach (DialogueOption option in options)
                    {
                        option.optionObject.SetActive(false);
                    }
                    
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(CompleteQuest);
                    options[0].optionText.text = "Exit";
                }
            }
        }

        private void CloseDialogue()
        {
            uIWindow.SetActive(false);
            dialogueWindow.SetActive(false);
            hudWindow.SetActive(true);
            _playerManager.DisableDialogueFlag();
        }

        private void GiveQuest()
        {
            _npcInteractionManager.GiveQuest(_playerManager);
            CloseDialogue();
        }

        private void CompleteQuest()
        {
            _npcInteractionManager.CompleteQuest(_playerManager);
            CloseDialogue();
        }
    }
}