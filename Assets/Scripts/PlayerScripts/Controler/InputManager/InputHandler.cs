using UnityEngine;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.GameUI;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Misc;
using SzymonPeszek.Enums;


namespace SzymonPeszek.PlayerScripts.Controller
{
    /// <summary>
    /// Class for managing inputs from InputSystem
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Handler",order = 0)]
        [Header("Movement",order = 1)]
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        [Header("Inputs", order = 1)]
        public bool bInput;
        public bool walkInput;
        public bool aInput;
        public bool yInput;
        public bool rbInput;
        public bool rtInput;
        public bool ltInput;
        public bool criticalAttackInput;
        public bool jumpInput;
        public bool inventoryInput;
        public bool lockOnInput;
        public bool rightStickRightInput;
        public bool rightStickLeftInput;
        public bool estusQuickSlotUse;

        public bool dPadUp;
        public bool dPadDown;
        public bool dPadLeft;
        public bool dPadRight;

        [Header("Flags", order = 1)]
        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool walkFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        
        [Header("Back Stabs", order = 1)]
        public Transform criticalAttackRayCastStartPoint;

        [Header("Timers", order = 1)]
        public float rollInputTimer;
        public float walkInputTimer;

        private PlayerControls _playerInputActions;
        private PlayerAttacker _playerAttacker;
        private PlayerInventory _playerInventory;
        private PlayerManager _playerManager;
        private PlayerStats _playerStats;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerAnimatorManager _playerAnimatorManager;

        [Header("Camera & UI", order = 1)]
        public CameraHandler cameraHandler;
        public UIManager uiManager;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            _playerAttacker = GetComponentInChildren<PlayerAttacker>();
            _playerInventory = GetComponent<PlayerInventory>();
            _playerManager = GetComponent<PlayerManager>();
            _playerStats = GetComponent<PlayerStats>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void OnEnable()
        {
            if (_playerInputActions == null)
            {
                _playerInputActions = new PlayerControls();
                _playerInputActions.PlayerMovement.Movement.performed += inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                _playerInputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
                _playerInputActions.PlayerActions.RB.performed += i => rbInput = true;
                _playerInputActions.PlayerActions.RT.performed += i => rtInput = true;
                _playerInputActions.PlayerActions.LT.performed += i => ltInput = true;
                _playerInputActions.PlayerQuickSlots.DPadRight.performed += i => dPadRight = true;
                _playerInputActions.PlayerQuickSlots.DPadLeft.performed += i => dPadLeft = true;
                _playerInputActions.PlayerActions.E.performed += i => aInput = true;
                _playerInputActions.PlayerActions.Roll.performed += i => bInput = true;
                _playerInputActions.PlayerActions.Roll.canceled += i => bInput = false;
                _playerInputActions.PlayerActions.Walk.performed += i => walkInput = true;
                _playerInputActions.PlayerActions.Walk.canceled += i => walkInput = false;
                _playerInputActions.PlayerActions.Jump.performed += i => jumpInput = true;
                _playerInputActions.PlayerActions.Inventory.performed += i => inventoryInput = true;
                _playerInputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                _playerInputActions.PlayerMovement.LockOnTargetRight.performed += i => rightStickRightInput = true;
                _playerInputActions.PlayerMovement.LockOnTargetLeft.performed += i => rightStickLeftInput = true;
                _playerInputActions.PlayerActions.Y.performed += i => yInput = true;
                _playerInputActions.PlayerActions.EstusQuickSlotUse.performed += i => estusQuickSlotUse = true;
                _playerInputActions.PlayerActions.CriticalAttack.performed += i => criticalAttackInput = true;
            }

            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

        /// <summary>
        /// Handle all inputs
        /// </summary>
        /// <param name="delta">Time stamp</param>
        public void TickInput(float delta)
        {
            if (_playerStats.isPlayerAlive && !_playerManager.isRestingAtBonfire && !_playerManager.isRemovingFog)
            {
                if (!inventoryFlag)
                {
                    HandleMoveInput();
                    HandleRollInput(delta);
                    HandleWalkInput(delta);
                    HandleAttackInput();
                    HandleQuickSlotsInput();
                    HandleLockOnInput();
                    HandleTwoHandInput();
                    HandleQuickHealInput();
                    HandleCriticalAttackInput();
                }

                HandleInventoryInput();
            }
        }

        /// <summary>
        /// Handle movement input
        /// </summary>
        private void HandleMoveInput()
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }

        /// <summary>
        /// Handle roll, back step and sprint input
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void HandleRollInput(float delta)
        {
            sprintFlag = bInput;
            
            if (bInput)
            {
                rollInputTimer += delta;

                if (_playerStats.currentStamina <= 0)
                {
                    bInput = false;
                    sprintFlag = false;
                }
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        /// <summary>
        /// Handle "slow" walking input
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void HandleWalkInput(float delta)
        {
            if (walkInput)
            {
                walkInputTimer += delta;

                walkFlag = moveAmount > 0.5f;
            }
            else
            {
                if (walkInputTimer < 0.5f)
                {
                    walkFlag = false;
                }

                walkInputTimer = 0.0f;
            }
        }

        /// <summary>
        /// Handle attack input
        /// </summary>
        private void HandleAttackInput()
        {
            if (_playerStats.currentStamina > 0)
            {
                #region Handle Light Attack
                if (rbInput)
                {
                    _playerAttacker.HandleRbAction();
                }
                #endregion

                #region Handle Heavy Attack
                if (rtInput)
                {
                    if (_playerManager.canDoCombo)
                    {
                        comboFlag = true;
                        _playerAttacker.HandleWeaponCombo(_playerInventory.rightWeapon);
                        comboFlag = false;
                    }
                    else
                    {
                        if (_playerManager.isInteracting)
                        {
                            return;
                        }

                        if (_playerManager.canDoCombo)
                        {
                            return;
                        }

                        _playerAnimatorManager.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingRightHandName], true);
                        _playerAttacker.HandleHeavyAttack(_playerInventory.rightWeapon);
                    }
                }
                #endregion

                #region Handle Left Hand Action
                if (ltInput)
                {
                    if (twoHandFlag)
                    {
                        //if two handing handle weapon art
                    }
                    else
                    {
                        _playerAttacker.HandleLtAction();
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Handle quick slot input
        /// </summary>
        private void HandleQuickSlotsInput()
        {
            if (dPadRight)
            {
                _playerInventory.ChangeRightWeapon();
            }
            else if (dPadLeft)
            {
                _playerInventory.ChangeLeftWeapon();
            }
        }

        /// <summary>
        /// Handle inventory input
        /// </summary>
        private void HandleInventoryInput()
        {
            if (inventoryInput)
            {
                uiManager.UpdateUI();
                uiManager.ResetTabsSelection();
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    horizontal = 0f;
                    vertical = 0f;
                    moveAmount = 0f;
                    mouseX = _cameraInput.x;
                    mouseY = _cameraInput.y;

                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.ResetTabsSelection();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Handle lock on input
        /// </summary>
        private void HandleLockOnInput()
        {
            if (lockOnInput)
            {
                if (lockOnFlag)
                {
                    lockOnInput = false;
                    lockOnFlag = false;
                    cameraHandler.ClearLockOnTargets();
                }
                else
                {
                    lockOnInput = false;
                    cameraHandler.HandleLockOn();

                    if (cameraHandler.nearestLockOnTarget != null)
                    {
                        cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                        lockOnFlag = true;
                    }
                }
            }

            if (lockOnFlag && rightStickLeftInput)
            {
                rightStickLeftInput = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && rightStickRightInput)
            {
                rightStickRightInput = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        /// <summary>
        /// Handle two hand input
        /// </summary>
        private void HandleTwoHandInput()
        {
            if (yInput)
            {
                yInput = false;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightWeapon, false);
                }
                else
                {
                    _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightWeapon, false);
                    _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.leftWeapon, true);
                }
            }
        }

        /// <summary>
        /// handle heal (estus use) input
        /// </summary>
        private void HandleQuickHealInput()
        {
            if (estusQuickSlotUse && uiManager.GetEstusCountInInventory() > 0)
            {
                ConsumableItem cI = _playerInventory.consumablesInventory.Find(e => e.consumableType == ConsumableType.HealItem);
                _playerStats.healthRefillAmount = cI.healAmount;
                _playerInventory.consumablesInventory.Remove(cI);
                _playerManager.shouldRefillHealth = true;
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EstusName], true);
                uiManager.UpdateEstusAmount();
            }
        }
        
        /// <summary>
        /// Handle back stab or riposte input
        /// </summary>
        private void HandleCriticalAttackInput()
        {
            if (criticalAttackInput)
            {
                criticalAttackInput = false;
                _playerAttacker.AttemptBackStabOrRiposte();
            }
        }
    }
}
