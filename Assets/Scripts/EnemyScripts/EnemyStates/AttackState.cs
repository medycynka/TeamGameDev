using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
{
    /// <summary>
    /// Class representing attack state 
    /// </summary>
    public class AttackState : State
    {
        [Header("Attack State", order = 0)]
        [Header("Possible After States", order = 1)]
        public IdleState idleState;
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        [Header("Enemy Attacks", order = 1)]
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        private bool _isComboing;

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
                if(enemyManager.currentTarget.currentHealth <= 0)
                {
                    return idleState;
                }
                
                if ((enemyManager.isInteracting || enemyManager.isGettingRiposted) && enemyManager.canDoCombo == false)
                {
                    return this;
                }
                if (enemyManager.isInteracting && enemyManager.canDoCombo)
                {
                    if (_isComboing)
                    {
                        enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentAttack.actionAnimation], true);
                        _isComboing = false;
                    }
                }
                
                if (enemyManager.isPreformingAction)
                {
                    return combatStanceState;
                }

                Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.enemyTransform.position;
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.enemyTransform.position);
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (currentAttack != null)
                {
                    if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                    {
                        return this;
                    }
                    
                    if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= currentAttack.maximumAttackAngle && viewableAngle >= currentAttack.minimumAttackAngle)
                        {
                            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                            {
                                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f, Time.deltaTime);
                                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.HorizontalName], 0, 0.1f, Time.deltaTime);
                                enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentAttack.actionAnimation], true);
                                enemyManager.isPreformingAction = true;

                                if (currentAttack.canCombo)
                                {
                                    currentAttack = currentAttack.comboAction;
                                    
                                    return this;
                                }

                                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                currentAttack = null;

                                return combatStanceState;
                            }
                        }
                    }
                }
                else
                {
                    GetNewAttack(enemyManager);
                }

                return combatStanceState;
            }
            
            return deathState;
        }

        /// <summary>
        /// Helper function for getting random attack from attacks list
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.enemyTransform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.enemyTransform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.enemyTransform.position);
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                        {
                            return;
                        }

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}