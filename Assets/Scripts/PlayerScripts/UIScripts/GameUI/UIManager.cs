using UnityEngine;
using TMPro;
using System.Linq;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.GameUI.WindowsManagers;
using SzymonPeszek.GameUI.Slots;
using SzymonPeszek.Enums;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.GameUI
{
    /// <summary>
    /// Class for managing UI during game
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public PlayerStats playerStats;
        public EquipmentWindowUI equipmentWindowUI;
        public InputHandler inputHandler;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject uiWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;
        public SettingsManager settingsManager;

        [Header("Consumables Quick Slots")]
        public TextMeshProUGUI estusSlotAmount;

        [Header("Souls box")]
        public TextMeshProUGUI currentSoulsAmount;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Inventory")]
        public GameObject weaponInventoryTab;
        public GameObject weaponInventorySlotPrefab;
        public GameObject weaponInventorySlotsParent;
        public GameObject shieldInventoryTab;
        public GameObject shieldInventorySlotPrefab;
        public GameObject shieldInventorySlotsParent;
        public GameObject helmetInventoryTab;
        public GameObject helmetInventorySlotsPrefab;
        public GameObject helmetInventorySlotsParent;
        public GameObject chestInventoryTab;
        public GameObject chestInventorySlotsPrefab;
        public GameObject chestInventorySlotsParent;
        public GameObject shoulderInventoryTab;
        public GameObject shoulderInventorySlotsPrefab;
        public GameObject shoulderInventorySlotsParent;
        public GameObject handInventoryTab;
        public GameObject handInventorySlotsPrefab;
        public GameObject handInventorySlotsParent;
        public GameObject legInventoryTab;
        public GameObject legInventorySlotsPrefab;
        public GameObject legInventorySlotsParent;
        public GameObject footInventoryTab;
        public GameObject footInventorySlotsPrefab;
        public GameObject footInventorySlotsParent;
        public GameObject ringInventoryTab;
        public GameObject ringInventorySlotsPrefab;
        public GameObject ringInventorySlotsParent;
        public GameObject consumableInventoryTab;
        public GameObject consumableInventorySlotsPrefab;
        public GameObject consumableInventorySlotsParent;

        [SerializeField] private WeaponInventorySlot[] weaponInventorySlots;
        [SerializeField] private WeaponInventorySlot[] shieldInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] helmetInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] chestInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] shoulderInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] handInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] legInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] footInventorySlots;
        [SerializeField] private EquipmentInventorySlot[] ringInventorySlots;
        [SerializeField] private ConsumableInventorySlot[] consumableInventorySlots;

        private void Start()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            playerStats = playerInventory.GetComponent<PlayerStats>();
            inputHandler = playerInventory.GetComponent<InputHandler>();
            
            currentSoulsAmount.text = playerStats.soulsAmount.ToString();
            GetAllInventorySlots();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            UpdateEstusAmount();
        }

        /// <summary>
        /// Update UI
        /// </summary>
        public void UpdateUI()
        {
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            equipmentWindowUI.UpdateStatsWindow(playerStats);
            settingsManager.LoadSettings();
            GetAllInventorySlots();
            UpdateAllInventoryTabs();
        }

        /// <summary>
        /// Update soul value in UI
        /// </summary>
        public void UpdateSouls()
        {
            currentSoulsAmount.text = playerStats.soulsAmount.ToString();
        }

        /// <summary>
        /// Open option window
        /// </summary>
        public void OpenSelectWindow()
        {
            uiWindow.SetActive(true);
            selectWindow.SetActive(true);
        }

        /// <summary>
        /// Close option window
        /// </summary>
        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
            uiWindow.SetActive(false);
        }

        /// <summary>
        /// Close inventory's windows
        /// </summary>
        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }

        /// <summary>
        /// Reset selected weapon slots
        /// </summary>
        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
        
        /// <summary>
        /// Update estus amount in UI
        /// </summary>
        public void UpdateEstusAmount()
        {
            int estusCount = GetEstusCountInInventory();

            if(estusCount > 99)
            {
                estusCount = 99;
            }

            estusSlotAmount.text = estusCount.ToString();
        }

        /// <summary>
        /// Calculate how many estuses player currently owns
        /// </summary>
        /// <returns>Amount of estuses in player's inventory</returns>
        public int GetEstusCountInInventory()
        {
            return playerInventory.consumablesInventory.Count(checker => checker.consumableType == ConsumableType.HealItem);
        }

        /// <summary>
        /// Reset inventory flag when closing inventory
        /// </summary>
        public void ResetInventoryFlag()
        {
            inputHandler.inventoryFlag = false;
        }

        #region Manage Inventory Tabs
        /// <summary>
        /// Resets inventory tabs
        /// </summary>
        public void ResetTabsSelection()
        {
            weaponInventoryTab.SetActive(true);
            shieldInventoryTab.SetActive(false);
            helmetInventoryTab.SetActive(false);
            chestInventoryTab.SetActive(false);
            shoulderInventoryTab.SetActive(false);
            handInventoryTab.SetActive(false);
            legInventoryTab.SetActive(false);
            footInventoryTab.SetActive(false);
            ringInventoryTab.SetActive(false);
            consumableInventoryTab.SetActive(false);
        }

        #region Get Current Items list
        /// <summary>
        /// Get weapon slots from inventory
        /// </summary>
        public void GetWeaponInventorySlot()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }

        /// <summary>
        /// Get shield slots from inventory
        /// </summary>
        public void GetShieldInventorySlot()
        {
            shieldInventorySlots = shieldInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }

        /// <summary>
        /// Get helmet slots from inventory
        /// </summary>
        public void GetHelmetInventorySlot()
        {
            helmetInventorySlots = helmetInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get chest slots from inventory
        /// </summary>
        public void GetChestInventorySlot()
        {
            chestInventorySlots = chestInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get shoulder slots from inventory
        /// </summary>
        public void GetShoulderInventorySlot()
        {
            shoulderInventorySlots = shoulderInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get glove slots from inventory
        /// </summary>
        public void GetHandInventorySlot()
        {
            handInventorySlots = handInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get leg slots from inventory
        /// </summary>
        public void GetLegInventorySlot()
        {
            legInventorySlots = legInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get foot slots from inventory
        /// </summary>
        public void GetFootInventorySlot()
        {
            footInventorySlots = footInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get ring slots from inventory
        /// </summary>
        public void GetRingInventorySlot()
        {
            ringInventorySlots = ringInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
        }

        /// <summary>
        /// Get consumable item slots from inventory
        /// </summary>
        public void GetConsumableInventorySlot()
        {
            consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
        }

        /// <summary>
        /// Get all item slots from inventory
        /// </summary>
        public void GetAllInventorySlots()
        {
            GetWeaponInventorySlot();
            GetShieldInventorySlot();
            GetHelmetInventorySlot();
            GetChestInventorySlot();
            GetShoulderInventorySlot();
            GetHandInventorySlot();
            GetLegInventorySlot();
            GetFootInventorySlot();
            GetRingInventorySlot();
            GetConsumableInventorySlot();
        }
        #endregion

        #region Update Inventory Tabs
        /// <summary>
        /// Update all inventory tabs
        /// </summary>
        private void UpdateAllInventoryTabs()
        {
            UpdateWeaponInventory();
            UpdateShieldInventory();
            UpdateHelmetInventory();
            UpdateChestInventory();
            UpdateShoulderInventory();
            UpdateHandInventory();
            UpdateLegInventory();
            UpdateFootInventory();
            UpdateRingInventory();
            UpdateConsumableInventory();
        }
        
        /// <summary>
        /// Update weapon inventory tab
        /// </summary>
        public void UpdateWeaponInventory()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent.transform);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot(playerInventory.weaponsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update shield inventory tab
        /// </summary>
        public void UpdateShieldInventory()
        {
            #region Shield Inventory Slots
            for (int i = 0; i < shieldInventorySlots.Length; i++)
            {
                if (i < playerInventory.shieldsInventory.Count)
                {
                    if (shieldInventorySlots.Length < playerInventory.shieldsInventory.Count)
                    {
                        Instantiate(shieldInventorySlotPrefab, shieldInventorySlotsParent.transform);
                        shieldInventorySlots = shieldInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    shieldInventorySlots[i].AddItem(playerInventory.shieldsInventory[i]);
                }
                else
                {
                    shieldInventorySlots[i].ClearInventorySlot(playerInventory.shieldsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update helmet inventory tab
        /// </summary>
        public void UpdateHelmetInventory()
        {
            #region Helmet Inventory Slots
            for (int i = 0; i < helmetInventorySlots.Length; i++)
            {
                if (i < playerInventory.helmetsInventory.Count)
                {
                    if (helmetInventorySlots.Length < playerInventory.helmetsInventory.Count)
                    {
                        Instantiate(helmetInventorySlotsPrefab, helmetInventorySlotsParent.transform);
                        helmetInventorySlots = helmetInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    helmetInventorySlots[i].AddItem(playerInventory.helmetsInventory[i]);
                    helmetInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    helmetInventorySlots[i].ClearInventorySlot(playerInventory.helmetsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update chest inventory tab
        /// </summary>
        public void UpdateChestInventory()
        {
            #region Chest Inventory Slots
            for (int i = 0; i < chestInventorySlots.Length; i++)
            {
                if (i < playerInventory.chestsInventory.Count)
                {
                    if (chestInventorySlots.Length < playerInventory.chestsInventory.Count)
                    {
                        Instantiate(chestInventorySlotsPrefab, chestInventorySlotsParent.transform);
                        chestInventorySlots = chestInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    chestInventorySlots[i].AddItem(playerInventory.chestsInventory[i]);
                    chestInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    chestInventorySlots[i].ClearInventorySlot(playerInventory.chestsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update shoulder inventory tab
        /// </summary>
        public void UpdateShoulderInventory()
        {
            #region Shoulder Inventory Slots
            for (int i = 0; i < shoulderInventorySlots.Length; i++)
            {
                if (i < playerInventory.shouldersInventory.Count)
                {
                    if (shoulderInventorySlots.Length < playerInventory.shouldersInventory.Count)
                    {
                        Instantiate(shoulderInventorySlotsPrefab, shoulderInventorySlotsParent.transform);
                        shoulderInventorySlots = shoulderInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    shoulderInventorySlots[i].AddItem(playerInventory.shouldersInventory[i]);
                    shoulderInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    shoulderInventorySlots[i].ClearInventorySlot(playerInventory.shouldersInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update glove inventory tab
        /// </summary>
        public void UpdateHandInventory()
        {
            #region Hand Inventory Slots
            for (int i = 0; i < handInventorySlots.Length; i++)
            {
                if (i < playerInventory.handsInventory.Count)
                {
                    if (handInventorySlots.Length < playerInventory.handsInventory.Count)
                    {
                        Instantiate(handInventorySlotsPrefab, handInventorySlotsParent.transform);
                        handInventorySlots = handInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    handInventorySlots[i].AddItem(playerInventory.handsInventory[i]);
                    handInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    handInventorySlots[i].ClearInventorySlot(playerInventory.handsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update leg inventory tab
        /// </summary>
        public void UpdateLegInventory()
        {
            #region Leg Inventory Slots
            for (int i = 0; i < legInventorySlots.Length; i++)
            {
                if (i < playerInventory.legsInventory.Count)
                {
                    if (legInventorySlots.Length < playerInventory.legsInventory.Count)
                    {
                        Instantiate(legInventorySlotsPrefab, legInventorySlotsParent.transform);
                        legInventorySlots = legInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    legInventorySlots[i].AddItem(playerInventory.legsInventory[i]);
                    legInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    legInventorySlots[i].ClearInventorySlot(playerInventory.legsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update foot inventory tab
        /// </summary>
        public void UpdateFootInventory()
        {
            #region Foot Inventory Slots
            for (int i = 0; i < footInventorySlots.Length; i++)
            {
                if (i < playerInventory.feetInventory.Count)
                {
                    if (footInventorySlots.Length < playerInventory.feetInventory.Count)
                    {
                        Instantiate(footInventorySlotsPrefab, footInventorySlotsParent.transform);
                        footInventorySlots = footInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    footInventorySlots[i].AddItem(playerInventory.feetInventory[i]);
                    footInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    footInventorySlots[i].ClearInventorySlot(playerInventory.feetInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update ring inventory tab
        /// </summary>
        public void UpdateRingInventory()
        {
            #region Ring Inventory Slots
            for (int i = 0; i < ringInventorySlots.Length; i++)
            {
                if (i < playerInventory.ringsInventory.Count)
                {
                    if (ringInventorySlots.Length < playerInventory.ringsInventory.Count)
                    {
                        Instantiate(ringInventorySlotsPrefab, ringInventorySlotsParent.transform);
                        ringInventorySlots = ringInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>();
                    }
                    ringInventorySlots[i].AddItem(playerInventory.ringsInventory[i]);
                    ringInventorySlots[i].equipUnEquip = false;
                }
                else
                {
                    ringInventorySlots[i].ClearInventorySlot(playerInventory.ringsInventory.Count == 0);
                }
            }
            #endregion
        }

        /// <summary>
        /// Update consumable items inventory tab
        /// </summary>
        public void UpdateConsumableInventory()
        {
            #region Consumable Inventory Slots
            for (int i = 0; i < consumableInventorySlots.Length; i++)
            {
                if (i < playerInventory.consumablesInventory.Count)
                {
                    if (consumableInventorySlots.Length < playerInventory.consumablesInventory.Count)
                    {
                        Instantiate(consumableInventorySlotsPrefab, consumableInventorySlotsParent.transform);
                        consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
                    }
                    consumableInventorySlots[i].AddItem(playerInventory.consumablesInventory[i]);
                }
                else
                {
                    consumableInventorySlots[i].ClearInventorySlot(playerInventory.consumablesInventory.Count == 0);
                }
            }
            #endregion
        }
        #endregion
        #endregion
    }

}