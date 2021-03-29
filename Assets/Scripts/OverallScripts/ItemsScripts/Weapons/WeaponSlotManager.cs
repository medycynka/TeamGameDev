using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.Damage;
using SzymonPeszek.Enums;
using SzymonPeszek.GameUI.Slots;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.Items.Weapons
{
    /// <summary>
    /// Class for managing weapons in right hand slot, left hand slot and back slot
    /// </summary>
    public class WeaponSlotManager : MonoBehaviour
    {
        [Header("Weapon Slot Manager", order = 0)]
        [Header("Current Weapon", order = 1)]
        public WeaponItem attackingWeapon;

        [Header("Left and Right Damage Colliders", order = 1)]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Quick Slots", order = 1)]
        public QuickSlotsUI quickSlotsUI;

        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;
        private WeaponHolderSlot _backSlot;
        private PlayerManager _playerManager;
        private PlayerInventory _playerInventory;
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerStats _playerStats;
        private InputHandler _inputHandler;
        private bool _isUsingBow;

        private void Awake()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            _playerStats = GetComponentInParent<PlayerStats>();
            _inputHandler = GetComponentInParent<InputHandler>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

            foreach (WeaponHolderSlot weaponSlot in GetComponentsInChildren<WeaponHolderSlot>())
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    _leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    _rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot) {
                    _backSlot = weaponSlot;
                }
            }
        }

        /// <summary>
        /// Load weapon on given slot
        /// </summary>
        /// <param name="weaponItem">Weapon to load</param>
        /// <param name="isLeft">Is it left hand slot?</param>
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem.weaponType == WeaponType.Shooting)
            {
                _inputHandler.twoHandFlag = true;
                _isUsingBow = true;
                
                _backSlot.LoadWeaponModel(_rightHandSlot.currentWeapon);
                _rightHandSlot.UnloadWeaponAndDestroy();
                
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weaponItem.thIdle], false, true);
                
                _leftHandSlot.currentWeapon = weaponItem;
                _leftHandSlot.LoadWeaponModel(weaponItem);
                
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
            else
            {
                if (isLeft)
                {
                    if (_leftHandSlot.currentWeapon != null)
                    {
                        if (_leftHandSlot.currentWeapon.weaponType == WeaponType.Shooting)
                        {
                            _inputHandler.twoHandFlag = false;
                            _isUsingBow = false;
                            
                            LoadWeaponOnSlot(_playerInventory.rightWeapon, false);
                            
                            _backSlot.UnloadWeaponAndDestroy();
                        }
                    }
                    
                    #region Handle Left Weapon Idle Animation

                    if (weaponItem != null)
                    {
                        _playerAnimatorManager.PlayTargetAnimation(
                            StaticAnimatorIds.animationIds[weaponItem.leftHandIdle], false, true);
                    }
                    else
                    {
                        _playerAnimatorManager.PlayTargetAnimation(
                            StaticAnimatorIds.animationIds[StaticAnimatorIds.LeftArmEmptyName], false, true);
                    }

                    #endregion

                    if (!_inputHandler.twoHandFlag)
                    {
                        _leftHandSlot.currentWeapon = weaponItem;
                        _leftHandSlot.LoadWeaponModel(weaponItem);
                        LoadLeftWeaponDamageCollider();
                        quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    }
                    else
                    {
                        _backSlot.currentWeapon = weaponItem;
                        _backSlot.LoadWeaponModel(weaponItem);
                        quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    }
                }
                else
                {
                    if (_isUsingBow)
                    {
                        _backSlot.LoadWeaponModel(weaponItem);
                        _rightHandSlot.UnloadWeaponAndDestroy();
                    }
                    else
                    {
                        if (_inputHandler.twoHandFlag)
                        {
                            _backSlot.LoadWeaponModel(_leftHandSlot.currentWeapon);
                            _leftHandSlot.UnloadWeaponAndDestroy();
                            _playerAnimatorManager.PlayTargetAnimation(
                                StaticAnimatorIds.animationIds[weaponItem.thIdle], false);
                        }
                        else
                        {
                            #region Handle Right Weapon Idle Animation

                            _playerAnimatorManager.PlayTargetAnimation(
                                StaticAnimatorIds.animationIds[StaticAnimatorIds.BothArmsEmptyName], false, true);
                            _backSlot.UnloadWeaponAndDestroy();

                            if (weaponItem != null)
                            {
                                _playerAnimatorManager.PlayTargetAnimation(
                                    StaticAnimatorIds.animationIds[weaponItem.rightHandIdle], false, true);
                            }
                            else
                            {
                                _playerAnimatorManager.PlayTargetAnimation(
                                    StaticAnimatorIds.animationIds[StaticAnimatorIds.RightArmEmptyName], false, true);
                            }

                            #endregion
                        }

                        _rightHandSlot.currentWeapon = weaponItem;
                        _rightHandSlot.LoadWeaponModel(weaponItem);
                        LoadRightWeaponDamageCollider();
                        quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    }
                }
            }
        }

        #region Handle Weapon's Damage Collider
        /// <summary>
        /// Load damage collider from weapon in left hand slot
        /// </summary>
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = _leftHandSlot.currentWeapon.baseAttack;
        }

        /// <summary>
        /// Load damage collider from weapon in right hand slot
        /// </summary>
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = _rightHandSlot.currentWeapon.baseAttack;
        }

        /// <summary>
        /// Enable collider during attack
        /// </summary>
        public void OpenDamageCollider()
        {
            if (_playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (_playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }
        
        /// <summary>
        /// Disable collider
        /// </summary>
        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }
        #endregion

        #region Handle Weapon's Stamina Drainage
        /// <summary>
        /// Drain stamina during light attack
        /// </summary>
        public void DrainStaminaLightAttack()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.ohLightAttackMultiplier));
        }

        /// <summary>
        /// Drain stamina during heavy attack
        /// </summary>
        public void DrainStaminaHeavyAttack()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.ohHeavyAttackMultiplier));
        }

        /// <summary>
        /// Drain stamina during two-handed light attack
        /// </summary>
        public void DrainStaminaLightAttackTh()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.thLightAttackMultiplier));
        }

        /// <summary>
        /// Drain stamina during two-handed heavy attack
        /// </summary>
        public void DrainStaminaHeavyAttackTh()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.thHeavyAttackMultiplier));
        }

        #endregion
    }

}
