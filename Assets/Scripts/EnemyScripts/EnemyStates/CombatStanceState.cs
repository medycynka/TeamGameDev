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
                if (enemyManager.isInteracting)
                {
                    return this;
                }
                
                HandleRotateTowardsTarget(enemyManager);
                
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.enemyTransform.position);
                
                if (enemyManager.isPreformingAction)
                {
                    enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);
                }

                if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
                {
                    return attackState;
                }
                
                if (distanceFromTarget > enemyManager.maximumAttackRange)
                {
                    return pursueTargetState;
                }

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
    }
}