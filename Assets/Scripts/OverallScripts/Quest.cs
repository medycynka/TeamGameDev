using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Consumable;
using UnityEngine;


namespace SzymonPeszek.Quests
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        public string questName;
        [TextArea] public string questTaskText;
        public bool isItemQuest;
        public ConsumableItem taskItem;
        public int taskItemAmount = 1;
        public bool isEnemyQuest;
        public string enemyToKillName;
        public int enemyToKillCount;
        public int moneyReward = 10;
        public Item[] rewardItems;
    }
}