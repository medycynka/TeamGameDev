using UnityEngine;
using UnityEngine.UI;
using BattleDrakeStudios.ModularCharacters;

namespace SzymonPeszek.MainMenuUI
{
    /// <summary>
    /// Class represents button for color picking
    /// </summary>
    public class ColorButtonPicker : MonoBehaviour
    {
        public ModularCharacterManager characterManager;
        public ColorPickerManager colorPicker;
        public string colorProperty;
        public Image buttonImage;

        private void Awake()
        {
            Color tempColor = characterManager.CharacterMaterial.GetColor(colorProperty);
            buttonImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1.0f);
        }

        /// <summary>
        /// Initialize color picker with current (property) color
        /// </summary>
        public void InitializeColorPickerWindow()
        {
            colorPicker.InitializeContent(colorProperty, this);
        }
    }
}