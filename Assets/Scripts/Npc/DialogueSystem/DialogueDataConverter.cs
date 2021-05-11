using System;
using System.Collections.Generic;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using UnityEngine;


namespace SzymonPeszek.Npc.DialogueSystem
{
    public static class DialogueDataConverter
    {
        public static Dictionary<string, DialogueNodeStorage> ToDictionary(DialogueContainer dialogueContainer)
        {
            Dictionary<string, DialogueNodeStorage> dialogueMap = new Dictionary<string, DialogueNodeStorage>();
            
            foreach (var dialogueNode in dialogueContainer.dialogueNodeData)
            {
                dialogueMap.Add(dialogueNode.nodeGuid, new DialogueNodeStorage
                {
                    guid = dialogueNode.nodeGuid,
                    dialogueText = dialogueNode.dialogueText,
                    giver = dialogueNode.isQuestGiver,
                    completer = dialogueNode.isQuestCompleter,
                    exit = dialogueNode.isExit,
                    links = GetNodeLinks(dialogueNode.nodeGuid, dialogueContainer)
                });
            }

            return dialogueMap;
        }

        public static List<string> ToGuidList(DialogueContainer dialogueContainer)
        {
            List<string> dialogueKeys = new List<string>();

            foreach (var dialogueNode in dialogueContainer.dialogueNodeData)
            {
                dialogueKeys.Add(dialogueNode.nodeGuid);
            }
            
            return dialogueKeys;
        }
        
        public static List<DialogueNodeStorage> ToDialogueStorageList(DialogueContainer dialogueContainer)
        {
            List<DialogueNodeStorage> dialogueStorage = new List<DialogueNodeStorage>();

            foreach (var dialogueNode in dialogueContainer.dialogueNodeData)
            {
                dialogueStorage.Add(new DialogueNodeStorage
                {
                    guid = dialogueNode.nodeGuid,
                    dialogueText = dialogueNode.dialogueText,
                    giver = dialogueNode.isQuestGiver,
                    completer = dialogueNode.isQuestCompleter,
                    exit = dialogueNode.isExit,
                    links = GetNodeLinks(dialogueNode.nodeGuid, dialogueContainer)
                });
            }
            
            return dialogueStorage;
        }
        
        public static List<LinkData> GetNodeLinks(string guid, DialogueContainer dialogueContainer)
        {
            List<LinkData> nodeLinks = new List<LinkData>();

            dialogueContainer.nodeLinks.ForEach(link =>
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
    }
    
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
}