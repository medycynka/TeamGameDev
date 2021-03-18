using UnityEngine;
using UnityEngine.UI;


namespace SzymonPeszek.GameUI.StatBars
{
    /// <summary>
    /// Class representing stamina bar
    /// </summary>
    public class StaminaBar : MonoBehaviour
    {
        public Slider staminaBarSlider;

        /// <summary>
        /// Set maximum stamina value
        /// </summary>
        /// <param name="maxStamina">Maximum stamina value</param>
        public void SetMaxStamina(float maxStamina)
        {
            staminaBarSlider.maxValue = maxStamina;
            staminaBarSlider.value = maxStamina;
        }

        /// <summary>
        /// Set current stamina value
        /// </summary>
        /// <param name="currentStamina">Current stamina value</param>
        public void SetCurrentStamina(float currentStamina)
        {
            staminaBarSlider.value = currentStamina;
        }
    }
}
