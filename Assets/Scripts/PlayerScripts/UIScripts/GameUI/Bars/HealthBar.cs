using UnityEngine;
using UnityEngine.UI;


namespace SzymonPeszek.GameUI.StatBars
{
    /// <summary>
    /// Class representing health bar
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        public Slider healthBarSlider;
        public Slider backgroundSlider;

        /// <summary>
        /// Set maximum health value
        /// </summary>
        /// <param name="maxHealth">Maximum health value</param>
        public void SetMaxHealth(float maxHealth)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = maxHealth;
            backgroundSlider.maxValue = maxHealth;
            backgroundSlider.value = maxHealth;
        }

        /// <summary>
        /// Set current health value
        /// </summary>
        /// <param name="currentHealth">Current health value</param>
        public void SetCurrentHealth(float currentHealth)
        {
            healthBarSlider.value = currentHealth;
        }
    }

}