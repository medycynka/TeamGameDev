using SzymonPeszek.EnemyScripts;
using SzymonPeszek.EnemyScripts.Animations;
using UnityEngine;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for spell object
    /// </summary>
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public float focusPointCost;
        
        [Header("Spell Type")] 
        public CastingType spellType;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public virtual void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
        }

        /// <summary>
        /// Successfully cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public virtual void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            playerStats.TakeFocusDamage(focusPointCost);
        }
        
        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="enemyAnimationManager">Enemy animation manager</param>
        /// <param name="enemyStats">Enemy stats</param>
        public virtual void EnemyAttemptToCastSpell(EnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
        {
        }

        /// <summary>
        /// Successfully cast this spell
        /// </summary>
        /// <param name="enemyAnimationManager">Enemy animation manager</param>
        /// <param name="enemyStats">Enemy stats</param>
        public virtual void EnemySuccessfullyCastSpell(EnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
        {
        }
    }
}