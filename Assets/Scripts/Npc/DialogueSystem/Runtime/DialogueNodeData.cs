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
    }
}