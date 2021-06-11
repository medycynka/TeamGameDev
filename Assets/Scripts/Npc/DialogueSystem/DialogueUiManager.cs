using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private NpcManager _npcManager;
        [SerializeField] private NpcInteractionManager _npcInteractionManager;

        public void Init(PlayerManager playerManager, NpcManager npcManager, 
            NpcInteractionManager npcInteractionManager)
        {
            _playerManager = playerManager;
            _npcManager = npcManager;
            _npcInteractionManager = npcInteractionManager;
            _npcInteractionManager.InitializeDialogue();
            _playerManager.dialogueFlag = true;
        }
        public void HandleDialogue()
        {
            _playerManager.dialogueFlag = true;
            _playerManager.EnableCursor();
            string firstNode = GetFirstNode();
            
            if (_npcInteractionManager.dialogueMap.ContainsKey(firstNode))
            {
                mainText.text = _npcInteractionManager.dialogueMap[firstNode].dialogueText;
                hudWindow.SetActive(false);
                uIWindow.SetActive(true);
                dialogueWindow.SetActive(true);

                if (_npcInteractionManager.dialogueMap[firstNode].item)
                {
                    Debug.Log("Adding items during dialogue...");
                    GiveItems();
                }

                if (_npcInteractionManager.dialogueMap[firstNode].giver)
                {
                    // Debug.Log("Getting quest from initial node");
                    _npcInteractionManager.GiveQuest(_playerManager);
                }

                if (_npcInteractionManager.dialogueMap[firstNode].completer)
                {
                    // Debug.Log("Completing quest in initial node");
                    _npcInteractionManager.CompleteQuest(_playerManager);
                }

                foreach (DialogueOption option in options)
                {
                    option.optionObject.SetActive(false);
                    option.button.onClick.RemoveAllListeners();
                }

                List<LinkData> potentialOptions =
                    _npcInteractionManager.dialogueMap[GetFirstNode()].links;
                for (int i = 0; i < potentialOptions.Count; i++)
                {
                    int i1 = i;
                    options[i].button.onClick.AddListener(() =>
                    {
                        HandleDialogueOption(potentialOptions[i1].targetGuid);
                    });
                    options[i].optionText.text = potentialOptions[i].portName;
                    options[i].optionObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"Error on dialogue loading with initial loaded node: {firstNode}");
                options[0].optionText.text = "ERROR";
                options[0].button.onClick.AddListener(CloseDialogue);
                options[0].optionObject.SetActive(true);
            }
        }

        private string GetFirstNode()
        {
            foreach (var link in _npcInteractionManager.currentDialogue.dialogueData.nodeLinks)
            {
                if (link.portName == "Next")
                {
                    return link.targetNodeGuid;
                }
            }

            throw new IndexOutOfRangeException("Can't find first node!");
        }

        private void HandleDialogueOption(string targetGuid)
        {
            _playerManager.dialogueFlag = true;
            if (_npcInteractionManager.dialogueMap[targetGuid].ender)
            {
                if (_npcInteractionManager.dialogueMap[targetGuid].completer)
                {
                    // Debug.Log("DialogUiManager: Ender + Completer");
                    if (_npcInteractionManager.TryCompleteQuest(_playerManager))
                    {
                        // Debug.Log("DialogUiManager: Can complete quest");
                        _npcInteractionManager.dialogueDataContainer.First(
                                d => d.dialogueId == _npcInteractionManager.currentDialogue.dialogueId)
                            .isCompleted = true;
                    }
                }
                else
                {
                    _npcInteractionManager.dialogueDataContainer.First(
                            d => d.dialogueId == _npcInteractionManager.currentDialogue.dialogueId)
                        .isCompleted = true;
                }
            }

            if (_npcInteractionManager.dialogueMap[targetGuid].item)
            {
                GiveItems();
            }
            
            if (_npcInteractionManager.dialogueMap[targetGuid].exit)
            {
                CloseDialogue();
            }
            else
            {
                foreach (DialogueOption option in options)
                {
                    option.optionObject.SetActive(false);
                    option.button.onClick.RemoveAllListeners();
                }
                
                mainText.text = _npcInteractionManager.dialogueMap[targetGuid].dialogueText;
                
                if (_npcInteractionManager.dialogueMap[targetGuid].giver && !_npcInteractionManager.isQuestGiven)
                {
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(GiveQuest);
                    options[0].optionText.text = _npcInteractionManager.dialogueMap[targetGuid].links.Count > 0
                        ? _npcInteractionManager.dialogueMap[targetGuid].links[0].portName
                        : "Exit";
                }
                else if(_npcInteractionManager.dialogueMap[targetGuid].completer && (_npcInteractionManager.isQuestGiven || 
                    _npcManager.currentMainQuest.quest.isEndingNotInGiver))
                {
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(CompleteQuest);
                    options[0].optionText.text = _npcInteractionManager.dialogueMap[targetGuid].links.Count > 0
                        ? _npcInteractionManager.dialogueMap[targetGuid].links[0].portName
                        : "Exit";
                }
                else
                {
                    List<LinkData> potentialOptions = _npcInteractionManager.dialogueMap[targetGuid].links;
                    
                    if (potentialOptions.Count > 0)
                    {
                        for (int i = 0; i < potentialOptions.Count; i++)
                        {
                            var i1 = i;
                            options[i].button.onClick.AddListener(()=>{
                                HandleDialogueOption(potentialOptions[i1].targetGuid);
                            });
                            options[i].optionText.text = potentialOptions[i1].portName;
                            options[i].optionObject.SetActive(true);
                        }
                    }
                    else
                    {
                        options[0].optionObject.SetActive(true);
                        options[0].button.onClick.AddListener(CloseDialogue);
                        options[0].optionText.text = "[Odejdź]";
                    }
                }
            }
        }

        private void CloseDialogue()
        {
            uIWindow.SetActive(false);
            dialogueWindow.SetActive(false);
            hudWindow.SetActive(true);

            for (int i = 0; i < options.Length; i++)
            {
                options[i].optionObject.SetActive(false);
                options[i].button.onClick.RemoveAllListeners();
            }
            
            _playerManager.DisableDialogueFlag();
            _playerManager.DisableCursor();
            
            _playerManager = null;
            _npcManager = null;
            _npcInteractionManager = null;
        }

        private void GiveQuest()
        {
            _npcInteractionManager.GiveQuest(_playerManager);
            CloseDialogue();
        }

        private void CompleteQuest()
        {
            // Debug.Log("DialogUiManager: Complete quest");
            _npcInteractionManager.CompleteQuest(_playerManager);
            CloseDialogue();
        }

        private void GiveItems()
        {
            int currDialogueId = _npcInteractionManager.currentDialogue.dialogueId;

            while (_npcManager.itemsToGiveOnDialogue.Any(i => i.dialogueId == currDialogueId))
            {
                ItemOnDialogueGetter tmp = _npcManager.itemsToGiveOnDialogue.First(i => i.dialogueId == currDialogueId);
                Debug.Log($"Adding {tmp.itemToGive.name}");
                _playerManager.GetItemFromQuest(tmp.itemToGive);
                _npcManager.itemsToGiveOnDialogue.Remove(tmp);
            }
        }
    }
}