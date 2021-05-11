using System.Collections;
using UnityEngine;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.PlayerScripts
{
    /// <summary>
    /// Class which manages player movement
    /// </summary>
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Locomotion Manager", order = 0)]
        [Header("Camera", order = 1)]
        public CameraHandler cameraHandler;

        private PlayerManager _playerManager;
        private Transform _cameraObject;
        private InputHandler _inputHandler;
        private PlayerStats _playerStats;
        private CapsuleCollider _playerCollider;
        private FootIkManager _footIkManager;

        [Header("Move Direction", order = 1)]
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorManager playerAnimatorManager;

        [Header("Player Rigidbody", order = 1)]
        public new Rigidbody rigidbody;

        [Header("Ground & Air Detection Stats", order = 1)]
        [SerializeField]
        private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        private float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        private float groundDirectionRayDistance = 0.2f;
        private LayerMask _ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats", order = 1)]
        public float movementSpeed = 5;
        public float walkingSpeed = 1;
        public float sprintSpeed = 7;
        public float rotationSpeed = 16;
        public float fallingSpeed = 80;
        public float jumpMult = 10;

        [Header("Next Jump Cooldown", order = 1)]
        public float nextJump = 2.0f;

        [Header("Stamina Costs", order = 1)]
        public float rollStaminaCost = 10;
        public float sprintStaminaCost = 5;

        [Header("Fall Damage", order = 1)]
        public float fallDamage = 10f;
        
        private RaycastHit _hit;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _playerStats = GetComponent<PlayerStats>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            _playerCollider = GetComponent<CapsuleCollider>();
            _footIkManager = GetComponentInChildren<FootIkManager>();
            
            if (!(Camera.main is null))
            {
                _cameraObject = Camera.main.transform;
            }
            
            if (cameraHandler == null)
            {
                cameraHandler = _cameraObject.GetComponent<CameraHandler>();
            }
            
            myTransform = transform;
            playerAnimatorManager.Initialize();

            _playerManager.isGrounded = true;
            _ignoreForGroundCheck = ~(1 << LayerMask.NameToLayer("UI") | 1 << LayerMask.NameToLayer("Camera") |
                                      1 << LayerMask.NameToLayer("Controller") | 1 << LayerMask.NameToLayer("Pick Up") |
                                      1 << LayerMask.NameToLayer("Spawner") |
                                      1 << LayerMask.NameToLayer("Area Manager") |
                                      1 << LayerMask.NameToLayer("Boss Area Manager") |
                                      1 << LayerMask.NameToLayer("Back Stab") |
                                      1 << LayerMask.NameToLayer("Spell") | 1 << LayerMask.NameToLayer("Riposte") |
                                      1 << LayerMask.NameToLayer("Collider Blocker"));
        }

        #region Movement
        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        /// <summary>
        /// Rotate player
        /// </summary>
        /// <param name="delta">Time stamp</param>
        public void HandleRotation(float delta)
        {
            if (playerAnimatorManager.canRotate)
            {
                if (_inputHandler.lockOnFlag)
                {
                    if (_inputHandler.sprintFlag || _inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = cameraHandler.cameraTransform.forward * _inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * _inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation =
                            Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation =
                            Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDir = _cameraObject.forward * _inputHandler.vertical;
                    targetDir += _cameraObject.right * _inputHandler.horizontal;
                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                    {
                        targetDir = myTransform.forward;
                    }

                    float rs = rotationSpeed;
                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
                    myTransform.rotation = targetRotation;
                }
            }
        }
        
        /// <summary>
        /// Move player
        /// </summary>
        /// <param name="delta">Time stamp</param>
        public void HandleMovement(float delta)
        {
            if (_inputHandler.rollFlag)
            {
                return;
            }

            if (_playerManager.isInteracting)
            {
                return;
            }

            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (_playerStats.currentStamina > 0 && _inputHandler.sprintFlag && _inputHandler.moveAmount > 0.5)
            {
                _playerManager.isSprinting = true;
                moveDirection *= sprintSpeed;
            }
            else
            {
                if (_inputHandler.walkFlag)
                {
                    moveDirection *= walkingSpeed;
                    _playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    _playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;

            if (_inputHandler.lockOnFlag && _inputHandler.sprintFlag == false)
            {
                playerAnimatorManager.UpdateAnimatorValues(_inputHandler.vertical, _inputHandler.horizontal, _playerManager.isSprinting, _inputHandler.walkFlag);
            }
            else
            {
                playerAnimatorManager.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting, _inputHandler.walkFlag);
            }
        }

        /// <summary>
        /// Roll, Back step or sprint player
        /// </summary>
        /// <param name="delta">Time stamp</param>
        public void HandleRollingAndSprinting(float delta)
        {
            if (playerAnimatorManager.anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName]))
            {
                return;
            }

            if (_playerStats.currentStamina > 0)
            {
                if (_inputHandler.rollFlag)
                {
                    moveDirection = _cameraObject.forward * _inputHandler.vertical;
                    moveDirection += _cameraObject.right * _inputHandler.horizontal;

                    if (_inputHandler.moveAmount > 0)
                    {
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.RollName], true);
                        moveDirection.y = 0;
                        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = rollRotation;
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BackStepName], true);
                    }

                    _playerStats.TakeStaminaDamage(rollStaminaCost);
                }
            }
        }

        /// <summary>
        /// Handle falling and ground detection
        /// </summary>
        /// <param name="moveDir">Move direction</param>
        public void HandleFalling(Vector3 moveDir)
        {
            _playerManager.isGrounded = false;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out _hit, 0.4f))
            {
                moveDir = Vector3.zero;
            }

            if (_playerManager.isInAir)
            {
                rigidbody.velocity = Vector3.down * (fallingSpeed * 0.05f) + moveDir * (fallingSpeed * 0.01f);
            }

            Vector3 dir = moveDir;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            _targetPosition = myTransform.position;

            Debug.DrawRay(origin, Vector3.down * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            
            if (Physics.Raycast(origin, Vector3.down, out _hit, minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
            {
                _normalVector = _hit.normal;
                Vector3 tp = _hit.point;
                _playerManager.isGrounded = true;
                _targetPosition.y = tp.y;

                if (_playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        //Debug.Log("You were in the air for " + inAirTimer);
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.LandName], true);

                        if (inAirTimer > 2.5f)
                        {
                            _playerStats.TakeDamage(fallDamage * inAirTimer);
                        }
                        
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EmptyName], false);
                        inAirTimer = 0;
                    }

                    _playerManager.isInAir = false;
                    _footIkManager.enableFeetIk = true;
                }
            }
            else
            {
                if (_playerManager.isGrounded)
                {
                    _playerManager.isGrounded = false;
                }

                if (_playerManager.isInAir == false)
                {
                    _footIkManager.enableFeetIk = false;
                    
                    if (_playerManager.isInteracting == false)
                    {
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.FallName], true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    _playerManager.isInAir = true;
                }
            }

            if (_playerManager.isInteracting || _inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = _targetPosition;
            }

        }

        /// <summary>
        /// Handle player jumping
        /// </summary>
        /// <param name="delta">Time stamp</param>
        public void HandleJumping(float delta)
        {
            if (_playerManager.isInteracting)
            {
                return;
            }

            if (_inputHandler.jumpInput)
            {
                if (_inputHandler.moveAmount > 0)
                {
                    nextJump -= delta;

                    if (nextJump <= 0)
                    {
                        StartCoroutine(ResizeCollider());
                        rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpMult, rigidbody.velocity.z);
                        playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.JumpName], true);
                        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = jumpRotation;
                    }
                }
            }
        }

        /// <summary>
        /// Coroutine which resize player's collider during jump
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator ResizeCollider()
        {
            yield return CoroutineYielder.waitFor02Second;

            _playerCollider.center = Vector3.up * 1.25f;
            _playerCollider.height = 1f;

            yield return CoroutineYielder.waitFor05Second;

            _playerCollider.center = Vector3.up;
            _playerCollider.height = 1.5f;
        }
        #endregion
    }
}