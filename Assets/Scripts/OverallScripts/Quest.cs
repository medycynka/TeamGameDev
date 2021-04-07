using SzymonPeszek.BaseClasses;
using UnityEngine;

namespace SzymonPeszek.Quests
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        public string questName;
        [TextArea] public string questTaskText;
        public Item taskItem;
        public int taskItemAmount = 1;
        public int moneyReward = 10;
        public Item[] rewardItems;
    }
}