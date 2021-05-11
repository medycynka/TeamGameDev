using SzymonPeszek.PlayerScripts;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Enums;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Misc;

    
namespace SzymonPeszek.GameUI.Slots
{
    /// <summary>
    /// Class representing consumable item slots in player's inventory 
    /// </summary>
    public class ConsumableInventorySlot : InventorySlotBase
    {
        public PlayerStats playerStats;
        public PlayerManager playerManager;
        public PlayerAnimatorManager playerAnimatorManager;

        private ConsumableItem _item;

        /// <summary>
        /// Add consumable item to this slot
        /// </summary>
        /// <param name="newItem">Consumable item</param>
        public void AddItem(ConsumableItem newItem)
        {
            _item = newItem;
            icon.sprite = _item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Delete item from this slot
        /// </summary>
        /// <param name="lastSlot">Is it last slot in inventory tab?</param>
        public void ClearInventorySlot(bool lastSlot)
        {
            _item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(lastSlot);
        }

        /// <summary>
        /// Use item represents by this slot
        /// </summary>
        public void UseThisItem()
        {
            if (_item != null)
            {
                switch (_item.consumableType)
                {
                    case ConsumableType.HealItem:
                        playerStats.healthRefillAmount = _item.healAmount;
                        playerManager.shouldRefillHealth = true;
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EstusName], true);
                        break;
                    case ConsumableType.SoulItem:
                        playerStats.soulsAmount += _item.soulAmount;
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.UseItemName], true);
                        break;
                    case ConsumableType.ManaItem:
                        break;
                }

                playerInventory.consumablesInventory.Remove(_item);
                uiManager.GetConsumableInventorySlot();
                uiManager.UpdateConsumableInventory();
                uiManager.UpdateEstusAmount();
                uiManager.UpdateSouls();
            }
        }
    }
}