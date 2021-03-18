using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;


namespace SzymonPeszek.Items.Consumable
{
    /// <summary>
    /// Class representing consumable item
    /// </summary>
    [CreateAssetMenu(menuName = "Items/Consumable Item")]
    public class ConsumableItem : Item
    {
        [Header("Consumable Item", order = 1)]
        [Header("Item Type", order = 2)]
        public ConsumableType consumableType;

        [Header("Item Properties", order = 2)]
        public float healAmount;
        public float soulAmount;
        public float manaAmount;

        [Header("Is Item Death Drop?", order = 2)]
        public bool isDeathDrop = false;
    }

}