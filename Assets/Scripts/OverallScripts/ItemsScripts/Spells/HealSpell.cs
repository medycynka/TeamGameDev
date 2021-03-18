using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.Items.Spells
{
    /// <summary>
    /// Class representing healing type spell
    /// </summary>
    [CreateAssetMenu(menuName = "Spells/Heal Spell")]
    public class HealSpell : SpellItem
    {
        public float healAmount;

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
            playerStats.HealPlayer(healAmount);
        }
    }
}