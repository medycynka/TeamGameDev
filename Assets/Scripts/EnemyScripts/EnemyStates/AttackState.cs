using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.PlayerScripts;


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
        public RotateTowardsTargetState rotateTowardsTargetState;
        public PursueTargetState pursueTargetState;
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        [Header("Enemy Attacks", order = 1)]
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;
        public EnemyMagicAction[] enemyMagicAttacks;
        public EnemyMagicAction currentMagicAttack;
        
        public bool hasPerformedAttack;

        private bool _willDoComboOnNextAttack;

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
                // if(enemyManager.currentTarget.currentHealth <= 0)
                // {
                //     return idleState;
                // }
                //
                // if ((enemyManager.isInteracting || enemyManager.isGettingRiposted) && !enemyManager.canDoCombo)
                // {
                //     return this;
                // }
                //
                // if (enemyManager.isInteracting && enemyManager.canDoCombo && !enemyManager.isMagicCaster)
                // {
                //     if (_willDoComboOnNextAttack)
                //     {
                //         enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentAttack.actionAnimation], true);
                //         _willDoComboOnNextAttack = false;
                //     }
                // }
                //
                // Vector3 targetDirection = enemyManager.currentTarget.characterTransform.position - enemyManager.characterTransform.position;
                // float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.characterTransform.position, enemyManager.characterTransform.position);
                // float viewableAngle = Vector3.Angle(targetDirection, enemyManager.characterTransform.forward);
                //
                // HandleRotateTowardsTarget(enemyManager);
                //
                // if (enemyManager.isPreformingAction)
                // {
                //     return combatStanceState;
                // }
                //
                // if (enemyManager.isMagicCaster)
                // {
                //     if (currentMagicAttack != null)
                //     {
                //         if (distanceFromTarget < currentMagicAttack.minimumDistanceNeededToAttack)
                //         {
                //             return this;
                //         }
                //
                //         if (distanceFromTarget < currentMagicAttack.maximumDistanceNeededToAttack)
                //         {
                //             if (viewableAngle <= currentMagicAttack.maximumAttackAngle &&
                //                 viewableAngle >= currentMagicAttack.minimumAttackAngle)
                //             {
                //                 if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                //                 {
                //                     enemyAnimationManager.anim.SetFloat(
                //                         StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f,
                //                         Time.deltaTime);
                //                     enemyAnimationManager.anim.SetFloat(
                //                         StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.HorizontalName], 0, 0.1f,
                //                         Time.deltaTime);
                //                     enemyAnimationManager.currentSpell = currentMagicAttack.spell;
                //                     currentMagicAttack.spell.EnemyAttemptToCastSpell(enemyAnimationManager, enemyStats);
                //                     
                //                     enemyManager.isPreformingAction = true;
                //                     enemyManager.currentRecoveryTime = currentMagicAttack.recoveryTime;
                //                     currentMagicAttack = null;
                //
                //                     return combatStanceState;
                //                 }
                //             }
                //         }
                //     }
                //     else
                //     {
                //         GetNewAttack(enemyManager);
                //     }
                // }
                // else
                // {
                //     if (currentAttack != null)
                //     {
                //         if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                //         {
                //             return this;
                //         }
                //
                //         if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                //         {
                //             if (viewableAngle <= currentAttack.maximumAttackAngle &&
                //                 viewableAngle >= currentAttack.minimumAttackAngle)
                //             {
                //                 if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                //                 {
                //                     enemyAnimationManager.anim.SetFloat(
                //                         StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0, 0.1f,
                //                         Time.deltaTime);
                //                     enemyAnimationManager.anim.SetFloat(
                //                         StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.HorizontalName], 0, 0.1f,
                //                         Time.deltaTime);
                //                     enemyAnimationManager.PlayTargetAnimation(
                //                         StaticAnimatorIds.enemyAnimationIds[currentAttack.actionAnimation], true);
                //                     enemyManager.isPreformingAction = true;
                //
                //                     RollForComboChance(enemyManager);
                //
                //                     if (currentAttack.canCombo && _willDoComboOnNextAttack)
                //                     {
                //                         currentAttack = currentAttack.comboAction;
                //
                //                         return this;
                //                     }
                //
                //                     enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                //                     currentAttack = null;
                //
                //                     return combatStanceState;
                //                 }
                //             }
                //         }
                //     }
                //     else
                //     {
                //         GetNewAttack(enemyManager);
                //     }
                // }
                //
                // return combatStanceState;
                RotateTowardsTargetWhilstAttacking(enemyManager);
                
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.characterTransform.position,
                    enemyManager.characterTransform.position);

                if (distanceFromTarget > enemyManager.maximumAttackRange)
                {
                    return pursueTargetState;
                }

                if (!enemyManager.isMagicCaster && _willDoComboOnNextAttack && enemyManager.canDoCombo)
                {
                    AttackTargetWithCombo(enemyAnimationManager, enemyManager);
                }

                if (!hasPerformedAttack)
                {
                    if (enemyManager.isMagicCaster)
                    {
                        MagicAttackTarget(enemyAnimationManager, enemyManager);
                    }
                    else
                    {
                        AttackTarget(enemyAnimationManager, enemyManager);
                        RollForComboChance(enemyManager);
                    }
                }

                if (_willDoComboOnNextAttack && hasPerformedAttack)
                {
                    return this;
                }

                return rotateTowardsTargetState;
            }
            
            return deathState;
        }

        /// <summary>
        /// Helper function for getting random attack from attacks list
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        public void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.characterTransform.position - enemyManager.characterTransform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.characterTransform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.characterTransform.position, enemyManager.characterTransform.position);
            int maxScore = 0;

            if (enemyManager.isMagicCaster)
            {
                for (int i = 0; i < enemyMagicAttacks.Length; i++)
                {
                    if (distanceFromTarget <= enemyMagicAttacks[i].maximumDistanceNeededToAttack &&
                        distanceFromTarget >= enemyMagicAttacks[i].minimumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= enemyMagicAttacks[i].maximumAttackAngle &&
                            viewableAngle >= enemyMagicAttacks[i].minimumAttackAngle)
                        {
                            maxScore += enemyMagicAttacks[i].attackScore;
                        }
                    }
                }

                int randomValue = Random.Range(0, maxScore);
                int temporaryScore = 0;

                for (int i = 0; i < enemyMagicAttacks.Length; i++)
                {
                    if (distanceFromTarget <= enemyMagicAttacks[i].maximumDistanceNeededToAttack &&
                        distanceFromTarget >= enemyMagicAttacks[i].minimumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= enemyMagicAttacks[i].maximumAttackAngle &&
                            viewableAngle >= enemyMagicAttacks[i].minimumAttackAngle)
                        {
                            if (currentMagicAttack != null)
                            {
                                return;
                            }

                            temporaryScore += enemyMagicAttacks[i].attackScore;

                            if (temporaryScore > randomValue)
                            {
                                currentMagicAttack = enemyMagicAttacks[i];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < enemyAttacks.Length; i++)
                {
                    EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                    if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                        distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                            viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

                    if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                        distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                            viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

        private void AttackTarget(EnemyAnimationManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentAttack.actionAnimation], true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }
        
        private void MagicAttackTarget(EnemyAnimationManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentMagicAttack.actionAnimation], true);
            enemyManager.currentRecoveryTime = currentMagicAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimationManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            _willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentAttack.actionAnimation], true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }
        
        private void AttackTargetWithMagicCombo(EnemyAnimationManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            _willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[currentMagicAttack.actionAnimation], true);
            enemyManager.currentRecoveryTime = currentMagicAttack.recoveryTime;
            currentMagicAttack = null;
        }
        
        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Rotate manually
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.characterTransform.position - enemyManager.characterTransform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = enemyManager.characterTransform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.characterTransform.rotation = Quaternion.Slerp(enemyManager.characterTransform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                _willDoComboOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                _willDoComboOnNextAttack = false;
                currentAttack = null;
            }
        }
    }
}