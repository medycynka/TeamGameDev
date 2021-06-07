using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Misc;
using UnityEngine;


namespace SzymonPeszek.EnemyScripts.States
{
    public class RotateTowardsTargetState : State
    {
        [Header("Rotate towards target State", order = 0)]
        [Header("Possible After States", order = 1)]
        public DeathState deathState;
        
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0);
                enemyAnimationManager.anim.SetFloat(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.HorizontalName], 0);

                Vector3 targetDirection = enemyManager.currentTarget.characterTransform.position - enemyManager.characterTransform.position;
                float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.characterTransform.forward, Vector3.up);

                if (viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
                {
                    enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                    return this;
                }
                if (viewableAngle <= -101 && viewableAngle >= -180 && !enemyManager.isInteracting)
                {
                    enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                    return this;
                }
                if (viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInteracting)
                {
                    enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                    return this;
                }
                if (viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
                {
                    enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                    return this;
                }

                return this;
            }
            else
            {
                return deathState;
            }
        }
    }
}