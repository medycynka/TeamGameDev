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
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        [Header("State Properties", order = 1)] 
        [Range(-180, 0)] public float minViewableAngleToRotate = -65;
        [Range(0, 180)] public float maxViewableAngleToRotate = 65;

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
                    Vector3 targetDirection = enemyManager.currentTarget.characterTransform.position - enemyManager.characterTransform.position;
                    float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.characterTransform.position,
                        enemyManager.characterTransform.position);
                    float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.characterTransform.forward, Vector3.up);

                    HandleRotateTowardsTarget(enemyManager);

                    if (viewableAngle > maxViewableAngleToRotate || viewableAngle < minViewableAngleToRotate)
                    {
                        return rotateTowardsTargetState;
                    }
                    
                    if (enemyManager.isInteracting)
                    {
                        return this;
                    }
                    
                    if (enemyManager.isPreformingAction)
                    {
                        enemyAnimationManager.anim.SetFloat(
                            StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f,
                            Time.deltaTime);

                        return this;
                    }

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
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.characterTransform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Lerp(enemyManager.characterTransform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
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