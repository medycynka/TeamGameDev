using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Npc;
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

        [Header("Quest ending property", order = 1)]
        public bool isEndingNotInGiver;
        public string giverNpcId;
        
        [Header("Rewards", order = 1)]
        public int moneyReward = 10;
        public Item[] rewardItems;

        [Header("Objects activated at quest get/complete", order = 1)]
        public List<string> switchActiveStateOnGive = new List<string>();
        public List<string> switchActiveStateOnComplete = new List<string>();

        [Header("Npc which dialogue should be change to next one on get/complete", order = 1)]
        public List<string> changeToNextDialogueOnGive = new List<string>();
        public List<string> changeToNextDialogueOnComplete = new List<string>();
    }
}