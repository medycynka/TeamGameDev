using UnityEngine;
using SzymonPeszek.Enums;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for item object
    /// </summary>
    public class Item : ScriptableObject
    {
        [Header("Item",order = 0)]
        [Header("Item Properties",order = 1)]
        public Sprite itemIcon;
        public string itemName;
        public int itemId;
        public ItemType itemType;
    }

}