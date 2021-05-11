using UnityEngine;

namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for character (player or enemy) stat manager
    /// </summary>
    public class CharacterStats : MonoBehaviour
    {
        [Header("Stats", order = 0)]
        [Header("Player Transform", order = 2)]
        public Transform characterTransform;
        
        [Header("Health", order = 1)]
        public float healthLevel = 10f;
        public float maxHealth;
        public float currentHealth;

        [Header("Stamina", order = 1)]
        public float staminaLevel = 10f;
        public float maxStamina;
        public float currentStamina;
        
        [Header("Focus", order = 1)]
        public float focusLevel = 5f;
        public float maxFocus;
        public float currentFocus;

        [Header("Attributes", order = 1)]
        public float baseArmor = 5f;
        public float strength = 4f;      // 1 Strength = +1 attack damage and +2.5 max health
        public float agility = 1f;       // 1 agility = +2.5 stamina and +0.5 armor
        public float defence = 3f;       // +2.5 to defence
        public float bonusHealth = 2f;   // +10 to maxHealth
        public float bonusStamina = 2f;  // +10 to maxStamina
        public float bonusFocus;  // +10 to maxFocus
    }

}
