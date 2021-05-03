using System;


namespace SzymonPeszek.Npc.DialogueSystem.Runtime
{
    [Serializable]
    public class NodeLinkData
    {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;
    }
}