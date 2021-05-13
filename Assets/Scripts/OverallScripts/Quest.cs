using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Consumable;
using UnityEngine;


namespace SzymonPeszek.Quests
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        [Header("Quest object", order = 0)]
        [Header("Quest properties", order = 1)]
        public string questName;
        [TextArea] public string questTaskText;
        
        [Header("Quest task", order = 1)]
        public bool isItemQuest;
        public ConsumableItem taskItem;
        public int taskItemAmount = 1;
        public bool isEnemyQuest;
        public string enemyToKillName;
        public int enemyToKillCount;
        
        [Header("Rewards", order = 1)]
        public int moneyReward = 10;
        public Item[] rewardItems;

        [Header("Objects activated at quest get/complete", order = 1)]
        public List<GameObject> activateOnGive = new List<GameObject>();
        public List<GameObject> activateOnComplete = new List<GameObject>();
    }
}