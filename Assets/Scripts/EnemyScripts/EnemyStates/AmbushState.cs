using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing ambush state
    /// </summary>
    public class AmbushState : State
    {
        [Header("Pursue Target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Ambush Settings", order = 1)]
        public bool isSleeping;
        public float detectionRadius = 2.5f;
        public string sleepAnimation;
        public string wakeAnimation;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;
        
        private Collider[] _detectPlayer = new Collider[2];

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
                if (isSleeping && enemyManager.isInteracting == false)
                {
                    enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[sleepAnimation], true);
                }

                #region Handle Target Detection
                int detectLength = Physics.OverlapSphereNonAlloc(enemyManager.characterTransform.position, detectionRadius, _detectPlayer, detectionLayer);

                for (int i = 0; i < detectLength; i++)
                {
                    CharacterStats characterStats = _detectPlayer[i].transform.GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        Vector3 targetsDirection = characterStats.characterTransform.position - enemyStats.characterTransform.position;
                        float viewableAngle = Vector3.Angle(targetsDirection, enemyStats.characterTransform.forward);

                        if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                        {
                            enemyManager.currentTarget = characterStats;
                            isSleeping = false;
                            enemyManager.navMeshBlocker.enabled = false;
                            enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[wakeAnimation], true);
                        }
                    }
                }
                #endregion

                #region Handle State Change
                if (enemyManager.currentTarget != null)
                {
                    enemyManager.shouldFollowTarget = true;
                    
                    return pursueTargetState;
                }
                
                return this;
                #endregion
            }
            
            return deathState;
        }
    }
}