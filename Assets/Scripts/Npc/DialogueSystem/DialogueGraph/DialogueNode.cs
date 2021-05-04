using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace SzymonPeszek.Npc.DialogueSystem.DialogueGraph
{
    public class DialogueNode : Node
    {
        public string guID;
        public string dialogueText;
        public bool entryPoint;
        public bool isQuestGiver;
        public bool isQuestCompleter;
    }
}