using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing pursue target state
    /// </summary>
    public class PursueTargetState : State
    {
        [Header("Pursue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public IdleState idleState;
        public CombatStanceState combatStanceState;
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
                if (enemyManager.shouldFollowTarget)
                {
                    if (enemyManager.isPreformingAction)
                    {
                        enemyAnimationManager.anim.SetFloat(
                            StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f,
                            Time.deltaTime);

                        return this;
                    }

                    //Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.enemyTransform.position;
                    float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
                        enemyManager.enemyTransform.position);
                    //float viewableAngle = Vector3.Angle(targetDirection, enemyManager.enemyTransform.forward);

                    if (distanceFromTarget > enemyManager.detectionRadius)
                    {
                        enemyManager.currentTarget = null;
                        enemyManager.shouldFollowTarget = false;

                        return idleState;
                    }

                    if (distanceFromTarget > enemyManager.maximumAttackRange)
                    {
                        enemyAnimationManager.anim.SetFloat(
                            StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 1, 0.1f,
                            Time.deltaTime);
                    }

                    HandleRotateTowardsTarget(enemyManager);

                    if (distanceFromTarget <= enemyManager.maximumAttackRange)
                    {
                        return combatStanceState;
                    }

                    return this;
                }

                return idleState;
            }
            
            return deathState;
        }
        
        /// <summary>
        /// Helper function for rotating character towards player
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPreformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.enemyTransform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Lerp(enemyManager.enemyTransform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh) -> Change to A*
            else
            {
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidBody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Lerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}