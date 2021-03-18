using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;


namespace SzymonPeszek.PlayerScripts.CameraManager
{
    /// <summary>
    /// Class for managing main camera
    /// </summary>
    public class CameraHandler : MonoBehaviour
    {
        [Header("Camera Handler", order = 0)]
        [Header("Player Components", order = 1)]
        public InputHandler inputHandler;
        public PlayerManager playerManager;

        [Header("Look At Targets", order = 1)]
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform _myTransform;
        private Vector3 _cameraTransformPosition;

        [Header("Masks", order = 1)]
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;

        [Header("Camera Basic Properties", order = 1)]
        public Vector3 cameraFollowVelocity = Vector3.zero;
        public float lookSpeed = 0.025f;
        public float followSpeed = 0.5f;
        public float pivotSpeed = 0.03f;
        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        [Header("Camera Detection Properties", order = 1)]
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffSet = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        [Header("Camera Lock-on Properties", order = 1)]
        public Transform currentLockOnTarget;
        public Transform nearestLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 20;
        public float lockOnSmoothFactor = 100f;
        public bool transitDuringLockOn;
        private const string EnvironmentTag = "Environment";
        private LayerMask _lockOnLayer;
        private RaycastHit _hit;
        private float _LockOnAngle = 120f;
        public Collider[] colliders;
        private List<CharacterManager> _availableTargets = new List<CharacterManager>();
        private int _collidersSize = 512;
        private int _collidersPrevSize = 512;
        
        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            _myTransform = transform;
            _defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << LayerMask.NameToLayer("UI") | 1 << LayerMask.NameToLayer("Camera") |
                             1 << LayerMask.NameToLayer("Controller") | 1 << LayerMask.NameToLayer("Pick Up") |
                             1 << LayerMask.NameToLayer("Spawner") | 1 << LayerMask.NameToLayer("Area Manager") |
                             1 << LayerMask.NameToLayer("Boss Area Manager") | 1 << LayerMask.NameToLayer("Back Stab") |
                             1 << LayerMask.NameToLayer("Spell") | 1 << LayerMask.NameToLayer("Riposte") |
                             1 << LayerMask.NameToLayer("Collider Blocker"));
            targetTransform = playerManager.transform;
            environmentLayer = 1 << LayerMask.NameToLayer(EnvironmentTag);
            _lockOnLayer = (1 << LayerMask.NameToLayer(EnvironmentTag) | 1 << LayerMask.NameToLayer("Enemy"));
            colliders = new Collider[_collidersSize];
        }

        /// <summary>
        /// Follow player's movement
        /// </summary>
        /// <param name="delta">Time stamp</param>
        public void FollowTarget(float delta)
        {
            var newTargetPosition = Vector3.SmoothDamp(_myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            _myTransform.position = newTargetPosition;

            HandleCameraCollisions(delta);
        }

        /// <summary>
        /// Handle camera rotation on mouse move
        /// </summary>
        /// <param name="delta">Time stamp</param>
        /// <param name="mouseXInput">Mouse movement on X-axis</param>
        /// <param name="mouseYInput">Mouse movement on Y-axis</param>
        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            float smooth = lockOnSmoothFactor;
            
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                _lookAngle += (mouseXInput * lookSpeed) / delta;
                _pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = _lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                _myTransform.rotation = targetRotation;

                rotation = Vector3.zero;

                if (transitDuringLockOn)
                {
                    smooth = 20f;
                    rotation.x = Mathf.Lerp(rotation.x, _pivotAngle, 5f);
                }
                else
                {
                    rotation.x = _pivotAngle;
                }

                _myTransform.rotation = Quaternion.Lerp(_myTransform.rotation, targetRotation, delta * smooth / 10f);
                targetRotation = Quaternion.Euler(rotation);

                if (transitDuringLockOn)
                {
                    cameraPivotTransform.localRotation = Quaternion.Lerp(cameraPivotTransform.localRotation, targetRotation, delta * smooth);
                }
                else
                {
                    cameraPivotTransform.localRotation = targetRotation;
                }
            }
            else
            {
                if (currentLockOnTarget == null)
                {
                    return;
                }
                
                Vector3 rotation = Vector3.zero;

                if (transitDuringLockOn)
                {
                    smooth = 20;
                    rotation.x = Mathf.Lerp(rotation.x, _pivotAngle, 5f);
                }

                Vector3 currentLockOnTargetPosition = currentLockOnTarget.position;
                Vector3 dir = currentLockOnTargetPosition - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                //transform.rotation = targetRotation;
                _myTransform.rotation = Quaternion.Lerp(_myTransform.rotation, targetRotation, delta * smooth / 10f);

                dir = currentLockOnTargetPosition - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);

                if (transitDuringLockOn)
                {
                    targetRotation = Quaternion.Lerp(cameraPivotTransform.rotation, targetRotation, delta * smooth);
                }
                
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        /// <summary>
        /// Prevent camera from penetrating walls, doors, etc.
        /// </summary>
        /// <param name="delta">Time stamp</param>
        private void HandleCameraCollisions(float delta)
        {
            _targetPosition = _defaultPosition;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out var hit, Mathf.Abs(_targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                _targetPosition = -(dis - cameraCollisionOffSet);
            }

            if (Mathf.Abs(_targetPosition) < minimumCollisionOffset)
            {
                _targetPosition = -minimumCollisionOffset;
            }

            _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
            cameraTransform.localPosition = _cameraTransformPosition;
        }

        /// <summary>
        /// Handle lock-on system
        /// </summary>
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            int collidersLenght = Physics.OverlapSphereNonAlloc(targetTransform.position, maximumLockOnDistance, colliders, _lockOnLayer);

            if (2 * collidersLenght < _collidersSize)
            {
                _collidersPrevSize = _collidersSize;
                _collidersSize /= 2;
            }
            else if (collidersLenght > _collidersSize)
            {
                _collidersPrevSize = _collidersSize;
                _collidersSize *= 2;
            }

            Vector3 currentTargetPosition = targetTransform.position;
            
            for (int i = 0; i < collidersLenght; i++)
            {
                if (colliders[i].TryGetComponent(out CharacterManager character))
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(currentTargetPosition, character.characterTransform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, playerManager.gameObject.transform.forward);

                    if (character.transform.root == targetTransform.transform.root || !(viewableAngle > -_LockOnAngle) || !(viewableAngle < _LockOnAngle) || !(distanceFromTarget <= maximumLockOnDistance))
                    {
                        continue;
                    }

                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out _hit))
                    {
                        if (_hit.transform.gameObject.CompareTag(EnvironmentTag) ||
                            _hit.transform.gameObject.layer == environmentLayer)
                        {
                            //Cannot lock onto target, object in the way
                        }
                        else
                        {
                            _availableTargets.Add(character);
                        }
                    }
                }
            }

            if (currentLockOnTarget != null)
            {
                return;
            }

            for (int i = 0; i < _availableTargets.Count; i++)
            {
                CharacterManager availableTarget = _availableTargets[i];
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget.transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTarget.lockOnTransform;
                }

                if (!inputHandler.lockOnFlag)
                {
                    continue;
                }

                Vector3 availableTargetPosition = availableTarget.characterTransform.position;
                Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargetPosition);
                Vector3 currentLockOnTargetPosition = currentLockOnTarget.position;
                float distanceFromLeftTarget = currentLockOnTargetPosition.x - availableTargetPosition.x;
                float distanceFromRightTarget = currentLockOnTargetPosition.x + availableTargetPosition.x;

                if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTarget.lockOnTransform;
                }

                if (!(relativeEnemyPosition.x < 0.00) || !(distanceFromRightTarget < shortestDistanceOfRightTarget))
                {
                    continue;
                }

                shortestDistanceOfRightTarget = distanceFromRightTarget;
                rightLockTarget = availableTarget.lockOnTransform;
            }

            if (_collidersPrevSize != _collidersSize)
            {
                colliders = new Collider[collidersLenght];
            }
        }

        /// <summary>
        /// Clear detected targets
        /// </summary>
        public void ClearLockOnTargets()
        {
            transitDuringLockOn = true;
            StartCoroutine(TransitionBetweenLockOn());
            
            _availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        /// <summary>
        /// Set camera's height
        /// </summary>
        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            cameraPivotTransform.localPosition = currentLockOnTarget != null
                ? Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity,
                    Time.deltaTime * 10f)
                : Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity,
                    Time.deltaTime * 10f);
        }

        private IEnumerator TransitionBetweenLockOn()
        {
            yield return CoroutineYielder.waitFor1Second;
            
            transitDuringLockOn = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maximumLockOnDistance);
        }
    }
}