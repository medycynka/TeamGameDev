using UnityEngine;
using UnityEngine.UI;


namespace SzymonPeszek.GameUI.StatBars
{
    /// <summary>
    /// Class representing focus bar
    /// </summary>
    public class FocusBar : MonoBehaviour
    {
        public Slider focusBarSlider;

        /// <summary>
        /// Set maximum focus value
        /// </summary>
        /// <param name="maxFocus">Maximum focus value</param>
        public void SetMaxFocus(float maxFocus)
        {
            focusBarSlider.maxValue = maxFocus;
            focusBarSlider.value = maxFocus;
        }

        /// <summary>
        /// Set current focus value
        /// </summary>
        /// <param name="currentFocus">Current focus value</param>
        public void SetCurrentFocus(float currentFocus)
        {
            focusBarSlider.value = currentFocus;
        }
    }
}