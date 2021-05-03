using System;
using System.Collections.Generic;
using UnityEngine;


namespace SzymonPeszek.Npc.DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueContainer: ScriptableObject
    {
        public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
        public List<DialogueNodeData> dialogueNodeData = new List<DialogueNodeData>();
        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();
        public List<CommentBlockData> commentBlockData = new List<CommentBlockData>();
    }
}