using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;
using UnityEngine.AI;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class managing enemy behaviour
    /// </summary>
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotionManager _enemyLocomotionManager;
        private EnemyAnimationManager _enemyAnimationManager;
        private EnemyStats _enemyStats;
        private EnemyDrops _enemyDrops; 
        private List<Material> _characterMaterials;
        private int _edgeWidthId;
        private int _noiseScaleId;
        private int _fresnelPowerId;
        private int _glowId;
        private int _aliveId;
        private int _disolveId;

        [Header("Manager Properties", order = 1)] 
        [Header("Bools", order = 2)]
        public bool isNeutral;
        public bool isPassive;
        public bool isPreformingAction;
        public bool isInteracting;
        public bool isGettingRiposted;
        public bool shouldDrop = true;
        public bool isAlive = true;
        public bool deadFromBackStab;
        public bool deadFromRiposte;

        [Header("Current Target", order = 2)]
        public CharacterStats currentTarget;

        [Header("Current State from FSM", order = 2)]
        public State currentState;

        [Header("A.I Settings", order = 2)]
        public NavMeshAgent navmeshAgent;
        public NavMeshObstacle navMeshBlocker;
        public Rigidbody enemyRigidBody;
        public float rotationSpeed = 15;
        public float detectionRadius = 15;
        public float maximumAttackRange = 1.5f;
        public float maximumDetectionAngle = 75;
        public float minimumDetectionAngle = -75;
        public bool shouldFollowTarget;
        public bool canPatrol = true;
        [Range(1f, 15f)] public float maxRandomPatrolDistance = 5f;
        [Range(2, 32)] public int maxRandomPointSearchIterations = 16;
        
        [Header("Combat Flags", order = 2)]
        public bool canDoCombo;

        [Header("Recovery Timer", order = 2)]
        public float currentRecoveryTime;

        [Header("Death Disolve Effect", order = 2)]
        public float disolveEdgeWidth = 0.015f;
        public float disolveNoiseScale = 30.0f;
        public float disolveFresnelPower = 15.0f;
        public float currentDisolveTime;
        public float disolveDurationTime = 4.0f;
        public bool shouldGlow;
        public float objectDestructionDuration = 5.0f;

        [HideInInspector] public Transform enemyTransform;

        private void Awake()
        {
            characterTransform = transform;
            _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            _enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
            _enemyStats = GetComponent<EnemyStats>();
            _enemyDrops = GetComponent<EnemyDrops>();
            enemyRigidBody = GetComponent<Rigidbody>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            navmeshAgent.enabled = false;
            navMeshBlocker = GetComponent<NavMeshObstacle>();
            enemyTransform = GetComponent<Transform>();

            #region Get Model Materials
            _characterMaterials = new List<Material>();
            Renderer[] renders = GetComponentsInChildren<Renderer>();

            _edgeWidthId = Shader.PropertyToID("_EdgeWidth");
            _noiseScaleId = Shader.PropertyToID("_NoiceScale");
            _fresnelPowerId = Shader.PropertyToID("_FresnelPower");
            _glowId = Shader.PropertyToID("_ShouldBlink");
            _aliveId = Shader.PropertyToID("_IsAlive");
            _disolveId = Shader.PropertyToID("_DisolveValue");

            foreach (var r in renders)
            {
                Material mat = r.material;
                mat.SetFloat(_edgeWidthId, disolveEdgeWidth);
                mat.SetFloat(_noiseScaleId, disolveNoiseScale);
                mat.SetFloat(_fresnelPowerId, disolveFresnelPower);
                _characterMaterials.Add(mat);
            }
            #endregion
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }
        
        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isInteracting = _enemyAnimationManager.anim.GetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName]);
            canDoCombo = _enemyAnimationManager.anim.GetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanDoComboName]);
            _enemyAnimationManager.anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsDeadName], _enemyStats.currentHealth <= 0.0f);
            isNeutral = _enemyStats.currentHealth < _enemyStats.maxHealth;
        }

        private void LateUpdate()
        {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Use current state from FSM and save next state
        /// </summary>
        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, _enemyStats, _enemyAnimationManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        /// <summary>
        /// Update current state
        /// </summary>
        /// <param name="state">Next state</param>
        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        /// <summary>
        /// Manage recovery from actions
        /// </summary>
        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }

        /// <summary>
        /// Get back stabbed
        /// </summary>
        public void HandleBackStabOrRiposte(bool backStab)
        {
            if (backStab)
            {
                _enemyStats.TakeDamage(pendingCriticalDamage, true, false);
            }
            else
            {
                _enemyStats.TakeDamage(pendingCriticalDamage, false, true);
            }
        }

        /// <summary>
        /// Handle character's death
        /// </summary>
        public void HandleDeath()
        {
            isAlive = false;
            _enemyStats.currentHealth = 0;
            
            if (deadFromBackStab)
            {
                _enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.BackStabbedName], true);
            }
            else if(deadFromRiposte)
            {
                _enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.RipostedName], true);
            }
            else
            {
                _enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.Death01Name], true);
            }

            #region Disolve Effect
            foreach (var cM in _characterMaterials)
            {
                cM.SetInt(_aliveId, 0);
            }

            if (shouldGlow)
            {
                foreach (var cM in _characterMaterials)
                {
                    cM.SetInt(_glowId, 1);
                }
            }

            StartCoroutine(DisolveAfterDeath());
            #endregion

            _enemyStats.playerStats.soulsAmount += _enemyStats.soulsGiveAmount;
            _enemyStats.playerStats.uiManager.currentSoulsAmount.text = _enemyStats.playerStats.soulsAmount.ToString();

            if (shouldDrop)
            {
                _enemyDrops.DropPickUp();
                shouldDrop = false;
            }

            Destroy(_enemyStats.enemyObject, deadFromBackStab ? objectDestructionDuration + 1.5f : objectDestructionDuration);
        }

        /// <summary>
        /// Coroutine for managing disolve effect after death
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator DisolveAfterDeath()
        {
            if (deadFromBackStab)
            {
                yield return CoroutineYielder.waitFor1HalfSecond;
            }
            
            while(currentDisolveTime < disolveDurationTime)
            {
                foreach (var cM in _characterMaterials)
                {
                    cM.SetFloat(_disolveId, Mathf.Lerp(-0.1f, 1.0f, currentDisolveTime / disolveDurationTime));
                }
                
                currentDisolveTime += Time.deltaTime;

                yield return null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Quaternion leftRayRotation = Quaternion.AngleAxis(minimumDetectionAngle, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(maximumDetectionAngle, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(transform.position, leftRayDirection * detectionRadius);
            Gizmos.DrawRay(transform.position, rightRayDirection * detectionRadius);
        }
    }
}