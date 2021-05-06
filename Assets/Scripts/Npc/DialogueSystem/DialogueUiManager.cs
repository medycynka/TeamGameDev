using System;
using System.Collections.Generic;
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
            mainText.text = _npcInteractionManager.dialogueMap[GetFirstNode()].dialogueText;
            hudWindow.SetActive(false);
            uIWindow.SetActive(true);
            dialogueWindow.SetActive(true);
            
            foreach (DialogueOption option in options)
            {
                option.optionObject.SetActive(false);
                option.button.onClick.RemoveAllListeners();
            }

            List<NpcInteractionManager.LinkData> potentialOptions =
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
            foreach (var link in _npcInteractionManager.dialogueData.nodeLinks)
            {
                if (link.portName == "Next")
                {
                    return link.targetNodeGuid;
                }
            }

            return "ERROR";
        }

        private void HandleDialogueOption(string targetGuid)
        {
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
                    options[0].optionText.text = "Exit";
                }
                else if(_npcInteractionManager.dialogueMap[targetGuid].completer && _npcInteractionManager.isQuestGiven)
                {
                    options[0].optionObject.SetActive(true);
                    options[0].button.onClick.AddListener(CompleteQuest);
                    options[0].optionText.text = "Exit";
                }
                else
                {
                    List<NpcInteractionManager.LinkData> potentialOptions = _npcInteractionManager.dialogueMap[targetGuid].links;
                    
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