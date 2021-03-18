using UnityEngine;
using BattleDrakeStudios.ModularCharacters;


namespace SzymonPeszek.Items.Equipment
{
    /// <summary>
    /// Class representing equipment item
    /// </summary>
    [CreateAssetMenu(menuName = "Items/Equipment Item")]
    public class EquipmentItem : BaseClasses.Item
    {
        [Header("Equipment Item", order = 1)]
        [Header("Item Parts", order = 2)]
        public ModularBodyPart[] bodyParts;
        public int[] partsIds;

        [Header("Bools", order = 2)]
        public bool removeHead;
        public bool removeHeadFeatures;
        public bool canDeactivate;

        [Header("Item Stats", order = 2)]
        public float armor;
        public int weight;
        public int durability;
    }

}
