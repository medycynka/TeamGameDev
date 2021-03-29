using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing idle state
    /// </summary>
    public class IdleState : State
    {
        [Header("Idle State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PatrolState patrolState;
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;

        private Collider[] detectPlayer = new Collider[2];

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
                if (enemyManager.isPassive)
                {
                    return patrolState;
                }
                
                if (!enemyManager.shouldFollowTarget)
                {
                    enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);
                }

                if (!enemyManager.isNeutral)
                {
                    LookForPlayer(enemyManager);
                }

                if (enemyManager.currentTarget != null)
                {
                    enemyManager.shouldFollowTarget = true;
                    
                    return pursueTargetState;
                }

                if (enemyStats.isBoss || !enemyManager.canPatrol)
                {
                    return this;
                }
                
                return patrolState;
            }
            
            return deathState;
        }

        /// <summary>
        /// Check if player is in detection range
        /// </summary>
        /// <param name="enemyManager"></param>
        private void LookForPlayer(EnemyManager enemyManager)
        {
            int detectLength = Physics.OverlapSphereNonAlloc(enemyManager.characterTransform.position, enemyManager.detectionRadius, detectPlayer, detectionLayer);

            for (int i = 0; i < detectLength; i++)
            {
                CharacterStats characterStats = detectPlayer[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.characterTransform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.characterTransform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
        }
    }
}