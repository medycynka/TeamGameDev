using UnityEngine;
using UnityEngine.UI;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.GameUI;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for inventory slots storing items in inventory UI
    /// </summary>
    public class InventorySlotBase : MonoBehaviour
    {
        [Header("Inventory Slot", order = 0)]
        [Header("Inventory Slot Basic Components", order = 1)]
        public PlayerInventory playerInventory;
        public WeaponSlotManager weaponSlotManager;
        public UIManager uiManager;

        [Header("Inventory Slot Icon", order = 1)]
        public Image icon;
    }
}