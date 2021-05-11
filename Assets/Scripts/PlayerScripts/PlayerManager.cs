using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.GameUI;
using SzymonPeszek.Misc;
using SzymonPeszek.Items.Bonfire;
using SzymonPeszek.Environment.Areas;


namespace SzymonPeszek.PlayerScripts
{
    /// <summary>
    /// Class which manages player's behaviour
    /// </summary>
    public class PlayerManager : CharacterManager
    {
        private InputHandler _inputHandler;
        private PlayerAnimatorManager _playerAnimatorManager;
        [Header("Player Components", order = 1)]
        [Header("Camera Component", order = 2)]
        public CameraHandler cameraHandler;
        private PlayerLocomotion _playerLocomotion;
        private PlayerStats _playerStats;
        private Animator _animator;
        
        [Header("UI", order = 2)]
        public InteractableUI interactableUI;

        [Header("Interactable Objects UI", order = 2)]
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        [Header("Animator Interaction Bool", order = 2)]
        public bool isInteracting;

        [Header("Helper bools", order = 2)]
        public bool shouldRefillHealth = false;
        public bool shouldRefillStamina = false;
        public bool shouldRefillFocus = false;
        public bool shouldRefillHealthBg = false;
        public bool shouldAddJumpForce = false;
        public bool isRestingAtBonfire = false;
        public bool isRemovingFog = false;

        [Header("Player Flags", order = 2)]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        [Header("Respawn Places", order = 2)]
        public GameObject quickMoveScreen;
        public GameObject currentSpawnPoint;

        private float _healthBgRefillTimer = 0.0f;
        private float _staminaRefillTimer = 0.0f;
        private float _focusRefillTimer = 0.0f;
        private float _addJumpForceTimer = 1.25f;

        private const string BonfireTag = "Bonfire";
        private const string InteractableTag = "Interactable";
        private const string FogWallTag = "Fog Wall";
        private const string ChestTag = "Chest";
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
            _playerLocomotion.HandleFalling(_playerLocomotion.moveDirection);
        }

        private void LateUpdate()
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

            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                if (!_inputHandler.inventoryFlag)
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
                _playerLocomotion.inAirTimer = _playerLocomotion.inAirTimer + Time.deltaTime;
            }
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
                                    interactableObject.Interact(this);
                                }
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
    }
}