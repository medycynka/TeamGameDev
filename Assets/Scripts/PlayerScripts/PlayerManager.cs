using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.GameUI;
using SzymonPeszek.Misc;
using SzymonPeszek.Items.Bonfire;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Items.Equipment;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Npc;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.Quests;


namespace SzymonPeszek.PlayerScripts
{
    /// <summary>
    /// Class which manages player's behaviour
    /// </summary>
    public class PlayerManager : CharacterManager
    {
        private InputHandler _inputHandler;
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerLocomotion _playerLocomotion;
        private PlayerStats _playerStats;
        private Animator _animator;
        private QuestManager _questManager;
        private PlayerInventory _playerInventory;
        
        [Header("Player Components", order = 1)]
        [Header("Camera Component", order = 2)]
        public CameraHandler cameraHandler;
        
        [Header("UI", order = 2)]
        public InteractableUI interactableUI;

        [Header("Interactable Objects UI", order = 2)]
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        [Header("Animator Interaction Bool", order = 2)]
        public bool isInteracting;

        [Header("Helper bools", order = 2)]
        public bool shouldRefillHealth;
        public bool shouldRefillStamina;
        public bool shouldRefillFocus;
        public bool shouldRefillHealthBg;
        public bool isRestingAtBonfire;
        public bool isRemovingFog;
        public bool isJumping;

        [Header("Player Flags", order = 2)]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;
        public bool dialogueFlag;

        [Header("Respawn Places", order = 2)]
        public GameObject quickMoveScreen;
        public GameObject currentSpawnPoint;

        private float _healthBgRefillTimer = 0.0f;
        private float _staminaRefillTimer = 0.0f;
        private float _focusRefillTimer = 0.0f;

        private const string BonfireTag = "Bonfire";
        private const string InteractableTag = "Interactable";
        private const string FogWallTag = "Fog Wall";
        private const string ChestTag = "Chest";
        private const string NpcTag = "NPC";
        private LayerMask _pickUpLayer;
        private Collider[] _interactColliders;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            characterTransform = transform;
            
            _inputHandler = GetComponent<InputHandler>();
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerStats = GetComponent<PlayerStats>();
            _animator = GetComponentInChildren<Animator>();
            _questManager = GetComponent<QuestManager>();
            _playerInventory = GetComponent<PlayerInventory>();
            _pickUpLayer = 1 << LayerMask.NameToLayer("Pick Up");
            interactableUI = FindObjectOfType<InteractableUI>();
            _interactColliders = new Collider[8];
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            
            isInteracting = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName]);
            canDoCombo = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanDoComboName]);
            isUsingRightHand = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingRightHandName]);
            isUsingLeftHand = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingLeftHandName]);
            isInvulnerable = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInvulnerableName]);
            isBlocking = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsBlockingName]);
            _animator.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInAirName], isInAir);
            _playerAnimatorManager.canRotate = _animator.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanRotateName]);

            _inputHandler.TickInput(delta);
            _playerLocomotion.HandleRollingAndSprinting(delta);

            CheckAllFunctions(delta);

            if (Time.time > _playerLocomotion.nextJump)
            {
                _playerLocomotion.HandleJumping(delta);
            }
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            _playerLocomotion.HandleMovement(delta);
            _playerLocomotion.HandleRotation(delta);
            _playerLocomotion.HandleFalling(delta);
        }

        private void LateUpdate()
        {
            ResetInputs();

            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                if (!_inputHandler.inventoryFlag && !dialogueFlag)
                {
                    cameraHandler.FollowTarget(delta);
                    cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
                }

                if(cameraHandler.currentLockOnTarget == null && _inputHandler.lockOnFlag)
                {
                    _inputHandler.lockOnFlag = false;
                }
            }

            if (isInAir)
            {
                _playerLocomotion.inAirTimer += delta;
            }
        }

        private void ResetInputs()
        {
            _inputHandler.rollFlag = false;
            _inputHandler.rbInput = false;
            _inputHandler.rtInput = false;
            _inputHandler.ltInput = false;
            _inputHandler.dPadUp = false;
            _inputHandler.dPadDown = false;
            _inputHandler.dPadLeft = false;
            _inputHandler.dPadRight = false;
            _inputHandler.aInput = false;
            _inputHandler.jumpInput = false;
            _inputHandler.inventoryInput = false;
            _inputHandler.estusQuickSlotUse = false;
        }

        #region Checking Funkctions
        /// <summary>
        /// Use all "checkers" functions
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void CheckAllFunctions(float delta)
        {
            CheckForInteractableObject();
            FillHealthBarBackGround(delta);
            CheckForHealthRefill();
            CheckForStaminaRefill(delta);
            CheckForFocusRefill(delta);
            CheckForSprintStaminaDrain(delta);
        }

        /// <summary>
        /// Check for interactable objects (pick ups, bonfires and fog walls) in range
        /// </summary>
        private void CheckForInteractableObject()
        {
            if (dialogueFlag)
            {
                return;
            }
            
            int collidersLength = Physics.OverlapSphereNonAlloc(transform.position, 1f, _interactColliders, _pickUpLayer);
            
            if (collidersLength > 0)
            {
                for (int i = 0; i < collidersLength; i++)
                {
                    if (_interactColliders[i].CompareTag(BonfireTag))
                    {
                        BonfireManager bonManager = _interactColliders[i].GetComponent<BonfireManager>();

                        if (!bonManager.isActivated)
                        {
                            BonfireActivator interactableObject = bonManager.GetComponent<BonfireActivator>();

                            if (interactableObject != null)
                            {
                                interactableUI.interactableText.text = interactableObject.interactableText;
                                interactableUIGameObject.SetActive(true);

                                if (_inputHandler.aInput)
                                {
                                    StopPlayer();
                                    interactableObject.Interact(this);
                                    interactableUIGameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            BonfireInteraction interactableObject = bonManager.GetComponent<BonfireInteraction>();

                            if (interactableObject != null)
                            {
                                if (bonManager.showRestPopUp)
                                {
                                    interactableUI.interactableText.text = interactableObject.interactableText;
                                    interactableUIGameObject.SetActive(true);

                                    if (_inputHandler.aInput)
                                    {
                                        StopPlayer();
                                        interactableObject.Interact(this);
                                    }
                                }
                            }
                        }
                    }
                    else if (_interactColliders[i].CompareTag(InteractableTag) || _interactColliders[i].CompareTag(ChestTag))
                    {
                        Interactable interactableObject = _interactColliders[i].GetComponent<Interactable>();

                        if (interactableObject != null)
                        {
                            interactableUI.interactableText.text = interactableObject.interactableText;
                            interactableUIGameObject.SetActive(true);

                            if (_inputHandler.aInput)
                            {
                                StopPlayer();
                                interactableObject.Interact(this);
                            }
                        }
                    }
                    else if (_interactColliders[i].CompareTag(FogWallTag))
                    {
                        FogWallManager interactableObject = _interactColliders[i].GetComponent<FogWallManager>();

                        if (interactableObject != null)
                        {
                            if (interactableObject.canInteract)
                            {
                                interactableUI.interactableText.text = interactableObject.interactableText;
                                interactableUIGameObject.SetActive(true);

                                if (_inputHandler.aInput)
                                {
                                    StopPlayer();
                                    interactableObject.Interact(this);
                                }
                            }
                        }
                    }
                    else if (_interactColliders[i].CompareTag(NpcTag))
                    {
                        NpcInteractionManager interactableObject = _interactColliders[i].GetComponent<NpcInteractionManager>();

                        if (interactableObject != null)
                        {
                            interactableUI.interactableText.text = interactableObject.interactableText;
                            interactableUIGameObject.SetActive(true);

                            if (_inputHandler.aInput)
                            {
                                StopPlayer();
                                interactableObject.Interact(this);
                            }
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && _inputHandler.aInput)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Fills health bar background image for smooth damage takes
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void FillHealthBarBackGround(float delta)
        {
            shouldRefillHealthBg = _playerStats.healthBar.backgroundSlider.value > _playerStats.healthBar.healthBarSlider.value;

            if (shouldRefillHealthBg)
            {
                if(_healthBgRefillTimer > 1.5f)
                {
                    _playerStats.healthBar.backgroundSlider.value -= _playerStats.healthBgRefillAmount * delta;

                    if(_playerStats.healthBar.backgroundSlider.value <= _playerStats.healthBar.healthBarSlider.value)
                    {
                        _playerStats.healthBar.backgroundSlider.value = _playerStats.healthBar.healthBarSlider.value;
                    }
                }
                else
                {
                    _healthBgRefillTimer += delta;
                }
            }
        }

        /// <summary>
        /// Check if health should be refiled
        /// </summary>
        private void CheckForHealthRefill()
        {
            if (shouldRefillHealth)
            {
                _playerStats.RefillHealth();

                if(_playerStats.currentHealth >= _playerStats.maxHealth)
                {
                    shouldRefillHealth = false;
                }
            }
        }

        /// <summary>
        /// Check if stamina should be refiled
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void CheckForStaminaRefill(float delta)
        {
            shouldRefillStamina = !isInteracting && !_inputHandler.comboFlag && !_inputHandler.sprintFlag && (_playerStats.currentStamina < _playerStats.maxStamina);

            if (shouldRefillStamina)
            {
                if (_staminaRefillTimer > 2.0f)
                {
                    _playerStats.RefillStamina();
                }
                else
                {
                    _staminaRefillTimer += delta;
                }
            }
            else
            {
                _staminaRefillTimer = 0.0f;
            }
        }

        /// <summary>
        /// Check if focus should be refiled
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void CheckForFocusRefill(float delta)
        {
            shouldRefillFocus = !isInteracting && (_playerStats.currentFocus < _playerStats.maxFocus);

            if (shouldRefillFocus)
            {
                if (_focusRefillTimer > 1.0f)
                {
                    _playerStats.RefillFocus();
                }
                else
                {
                    _focusRefillTimer += delta;
                }
            }
            else
            {
                _focusRefillTimer = 0.0f;
            }
        }

        /// <summary>
        /// Check if stamina should be drained during sprint, roll or back step
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void CheckForSprintStaminaDrain(float delta)
        {
            if (_inputHandler.sprintFlag)
            {
                if (_inputHandler.moveAmount > 0)
                {
                    _playerStats.TakeStaminaDamage((_playerLocomotion.sprintStaminaCost * _playerStats.bonusBuffEndurance) * delta);
                }

                if(_playerStats.currentStamina <= 0f)
                {
                    isSprinting = false;
                }
            }
        }
        #endregion
        
        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            _playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from ice skating
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.ChestOpeningName], true);            
        }

        public void AcceptNewQuest(Quest quest)
        {
            if (quest)
            {
                _questManager.AddNewQuest(quest);
                _playerStats.isHavingQuest = true;
            }
        }

        public bool CanCompleteQuest(Quest quest)
        {
            if (quest.isEnemyQuest)
            {
                return _playerStats.IsKillCountFulfilled(quest.enemyToKillName, quest.enemyToKillCount);
            }

            return quest.isItemQuest && _playerInventory.HasNeededItems(quest.taskItem, quest.taskItemAmount);
        }
        
        public bool CompleteQuest(Quest quest)
        {
            bool isCompleted = CanCompleteQuest(quest);

            if (isCompleted)
            {
                _questManager.CompleteQuest(quest);
                // claim rewards
                _playerStats.isHavingQuest = false;
                _playerStats.soulsAmount += quest.moneyReward;
                _inputHandler.uiManager.UpdateSouls();

                if (quest.isItemQuest)
                {
                    for (int i = 0; i < quest.taskItemAmount; i++)
                    {
                        _playerInventory.consumablesInventory.Remove(quest.taskItem);
                    }
                }

                if (quest.rewardItems.Length > 0)
                {
                    for (int i = 0; i < quest.rewardItems.Length; i++)
                    {
                        if (quest.rewardItems[i] != null)
                        {
                            switch (quest.rewardItems[i].itemType)
                            {
                                case ItemType.Weapon:
                                    _playerInventory.weaponsInventory.Add((WeaponItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.Shield:
                                    _playerInventory.shieldsInventory.Add((WeaponItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.Helmet:
                                    _playerInventory.helmetsInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.ChestArmor:
                                    _playerInventory.chestsInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.ShoulderArmor:
                                    _playerInventory.shouldersInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.HandArmor:
                                    _playerInventory.handsInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.LegArmor:
                                    _playerInventory.legsInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.FootArmor:
                                    _playerInventory.feetInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.Ring:
                                    _playerInventory.ringsInventory.Add((EquipmentItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.Consumable:
                                    _playerInventory.consumablesInventory.Add((ConsumableItem) quest.rewardItems[i]);
                                    break;
                                case ItemType.Spell:
                                    break;
                                case ItemType.QuestItem:
                                    _playerInventory.consumablesInventory.Add((ConsumableItem) quest.rewardItems[i]);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                }
                
                foreach (var key in _playerStats.killCount.Keys.ToList())
                {
                    _playerStats.killCount[key] = 0;
                }
            }

            return isCompleted;
        }

        public void DisableDialogueFlag()
        {
            StartCoroutine(EnablePlayerManager());
        }

        public void StopPlayer()
        {
            _inputHandler.vertical = 0;
            _inputHandler.horizontal = 0;
            _inputHandler.moveAmount = 0;
            _playerLocomotion.rigidbody.velocity = Vector3.zero;
            _playerAnimatorManager.anim.SetFloat(StaticAnimatorIds.animationIds[StaticAnimatorIds.VerticalName], 0);
            _playerAnimatorManager.anim.SetFloat(StaticAnimatorIds.animationIds[StaticAnimatorIds.HorizontalName], 0);
        }
        
        private IEnumerator EnablePlayerManager()
        {
            yield return CoroutineYielder.waitFor025Second;
            
            dialogueFlag = false;
        }
    }
}