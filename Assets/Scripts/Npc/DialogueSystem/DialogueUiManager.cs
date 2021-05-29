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

        private PlayerManager _playerManager;
        private NpcManager _npcManager;
        private NpcInteractionManager _npcInteractionManager;

        public void Init(PlayerManager playerManager, NpcManager npcManager, 
            NpcInteractionManager npcInteractionManager)
        {
            _playerManager = playerManager;
            _npcManager = npcManager;
            _npcInteractionManager = npcInteractionManager;
        }
        public void HandleDialogue()
        {
            _playerManager.dialogueFlag = true;
            mainText.text = _npcInteractionManager.dialogueMap[GetFirstNode()].dialogueText;
            hudWindow.SetActive(false);
            uIWindow.SetActive(true);
            dialogueWindow.SetActive(true);
            
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
                options[i].button.onClick.AddListener(()=>{ HandleDialogueOption(potentialOptions[i1].targetGuid); });
                options[i].optionText.text = potentialOptions[i].portName;
                options[i].optionObject.SetActive(true);
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
            if (_npcInteractionManager.dialogueMap[targetGuid].ender)
            {
                if (_npcInteractionManager.dialogueMap[targetGuid].completer)
                {
                    if (_npcInteractionManager.TryCompleteQuest(_playerManager))
                    {
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
                    mainText.text = _npcManager.currentMainQuest.quest.questTaskText;
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(GiveQuest);
                    options[0].optionText.text = _npcInteractionManager.dialogueMap[targetGuid].links.Count > 0
                        ? _npcInteractionManager.dialogueMap[targetGuid].links[0].portName
                        : "Exit";
                }
                else if(_npcInteractionManager.dialogueMap[targetGuid].completer && _npcInteractionManager.isQuestGiven)
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
                        options[0].optionText.text = "Exit";
                    }
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