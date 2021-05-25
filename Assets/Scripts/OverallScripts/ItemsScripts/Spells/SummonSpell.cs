using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using UnityEngine;


namespace SzymonPeszek.Items.Spells
{
    /// <summary>
    /// Class representing summon type spell
    /// </summary>
    [CreateAssetMenu(menuName = "Spells/Summon Spell")]
    public class SummonSpell : SpellItem
    {
        [Header("Summon properties")]
        public GameObject spawnPrefab;
        public float spawnRadius = 10f;
        public LayerMask spawnLayer;
        public int maxSpawnIterationCount = 32;

        private Vector3 _spawnPosition;
        private RaycastHit _hit;
        
        public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            
        }

        public override void EnemyAttemptToCastSpell(EnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
        {
            enemyAnimationManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[spellAnimation], true);
            _spawnPosition = GetSummonPosition(enemyStats.characterTransform.position);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, _spawnPosition + Vector3.up * 0.05f, Quaternion.identity);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, _spawnPosition, Quaternion.identity);
        }

        public override void EnemySuccessfullyCastSpell(EnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
        {
            spawnPrefab = Instantiate(spawnPrefab, _spawnPosition, enemyStats.characterTransform.rotation);
        }

        private Vector3 GetSummonPosition(Vector3 casterPosition)
        {
            for (int i = 0; i < maxSpawnIterationCount; i++)
            {
                Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
                if (Physics.Raycast(randomPoint + casterPosition, Vector3.down, out _hit, spawnRadius,
                    spawnLayer))
                {
                    return _hit.point;
                }
            }
            
            return casterPosition + Vector3.forward;
        }
    }
}