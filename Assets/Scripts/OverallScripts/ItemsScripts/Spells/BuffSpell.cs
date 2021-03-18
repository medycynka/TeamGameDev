using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Items.Spells
{
    /// <summary>
    /// Class representing buff type spell
    /// </summary>
    [CreateAssetMenu(menuName = "Spells/Buff Spell")]
    public class BuffSpell : SpellItem
    {
        public StatsBuffType buffType;
        public BuffRang buffRang;
        public float buffAmount;
        
        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(playerAnimatorManager, playerStats);
            
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[spellAnimation], true);
        }

        /// <summary>
        /// Successfully cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
            
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorManager.transform); 
            playerStats.BuffPlayer(buffType, buffRang, buffAmount);
        }
    }
}