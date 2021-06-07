using System;
using UnityEngine;


namespace SzymonPeszek.Npc.DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueNodeData
    {
        public string nodeGuid;
        public string dialogueText;
        public Vector2 nodePosition;
        public bool isQuestGiver;
        public bool isQuestCompleter;
        public bool isExit;
        public bool isEnding;
        public bool isItem;
    }
}