using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing combat state
    /// </summary>
    public class CombatStanceState : State
    {
        [Header("Combat Stance State", order = 0)]
        [Header("Possible After States", order = 1)]
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        private bool _randomDestinationSet;
        private float _verticalMovement;
        private float _horizontalMovement;

        /// <summary>
        /// Use state behaviour
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        /// <param name="enemyStats">Enemy stats</param>
        /// <param name="enemyAnimationManager">Enemy animation manager</param>
        /// <returns>This or next state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName],
                    _verticalMovement, 0.2f, Time.deltaTime);
                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.HorizontalName], 
                    _horizontalMovement, 0.2f, Time.deltaTime);
                attackState.hasPerformedAttack = false;
                
                if (enemyManager.isInteracting)
                {
                    enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0);
                    enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.HorizontalName], 0);
                    
                    return this;
                }
                
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.characterTransform.position);

                if (distanceFromTarget > enemyManager.maximumAttackRange)
                {
                    return pursueTargetState;
                }

                if (!_randomDestinationSet)
                {
                    _randomDestinationSet = true;
                    DecideCirclingAction(enemyAnimationManager);
                }
                
                HandleRotateTowardsTarget(enemyManager);

                if (enemyManager.currentRecoveryTime <= 0 && !enemyManager.isMagicCaster && attackState.currentAttack != null)
                {
                    _randomDestinationSet = false;

                    return attackState;
                }

                if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isMagicCaster &&
                    attackState.currentMagicAttack != null)
                {
                    _randomDestinationSet = false;

                    return attackState;
                }
                
                attackState.GetNewAttack(enemyManager);

                return this;
            }
            
            return deathState;
        }
        
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPreformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh) -> Change to A*
            else
            {
                Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidBody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void DecideCirclingAction(EnemyAnimationManager enemyAnimationManager)
        {
            // Add more circling functions (maybe based on percentage? like 35% to walk and 65% to run etc.)
            WalkAroundTarget(enemyAnimationManager);
        }
        
        private void WalkAroundTarget(EnemyAnimationManager enemyAnimationManager)
        {
            _verticalMovement = 0f;
            
            _horizontalMovement = Random.Range(-1, 1);
            
            if (_horizontalMovement <= 1 && _horizontalMovement > 0)
            {
                _horizontalMovement = 0.5f;
            }
            else if (_horizontalMovement >= -1 && _horizontalMovement < 0)
            {
                _horizontalMovement = -0.5f;
            }
        }
    }
}