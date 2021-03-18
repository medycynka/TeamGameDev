using UnityEngine;
using UnityEngine.UI;
using BattleDrakeStudios.ModularCharacters;
using TMPro;

namespace SzymonPeszek.MainMenuUI
{
    /// <summary>
    /// Class for picking color of character's parts
    /// </summary>
    public class ColorPickerManager : MonoBehaviour
    {
        [Header("Color Picker Manager", order = 0)]
        [Header("Color Picker Components", order = 1)]
        public ModularCharacterManager modularCharacterManager;
        public string colorProperty = "";

        [Header("Color Properties", order = 1)]
        public Color partColor;
        public Image shownColor;
        public ColorButtonPicker colorButtonPicker;
        public float colorR = 1.0f;
        public float colorG = 1.0f;
        public float colorB = 1.0f;
        public float colorA = 1.0f;
        private Color _baseColor;

        [Header("Sliders", order = 1)]
        public Slider rSlider;
        public Slider gSlider;
        public Slider bSlider;
        public Slider aSlider;

        [Header("Values Placeholders", order = 1)]
        public TextMeshProUGUI rPlaceholder;
        public TextMeshProUGUI gPlaceholder;
        public TextMeshProUGUI bPlaceholder;
        public TextMeshProUGUI aPlaceholder;

        /// <summary>
        /// Initialize class's properties
        /// </summary>
        /// <param name="s">Color property name</param>
        /// <param name="cBp">ColorButtonPicker component</param>
        public void InitializeContent(string s, ColorButtonPicker cBp)
        {
            colorProperty = s;
            colorButtonPicker = cBp;
            partColor = modularCharacterManager.CharacterMaterial.GetColor(colorProperty);
            shownColor.color = partColor;
            colorR = partColor.r;
            colorG = partColor.g;
            colorB = partColor.b;
            colorA = partColor.a;
            rSlider.value = colorR;
            gSlider.value = colorG;
            bSlider.value = colorB;
            aSlider.value = colorA;
            _baseColor = partColor;
            colorButtonPicker.buttonImage.color = partColor;
        }

        /// <summary>
        /// Set red value of rgba color
        /// </summary>
        /// <param name="r">Red value of current color</param>
        public void SetColorR(float r)
        {
            colorR = r;

            int i = (int)(255 * r);
            rPlaceholder.text = i.ToString();

            SetColor();
        }

        /// <summary>
        /// Get red color value from text field
        /// </summary>
        /// <param name="s">Red color value</param>
        public void GetColorRValue(string s)
        {
            if (s == "")
            {
                colorR = 0.0f;
            }
            else
            {
                colorR = float.Parse(s) / 255.0f;
            }

            rSlider.value = colorR;

            SetColor();
        }

        /// <summary>
        /// Set green value of rgba color
        /// </summary>
        /// <param name="g">Green value of current color</param>
        public void SetColorG(float g)
        {
            colorG = g;

            int i = (int)(255 * g);
            gPlaceholder.text = i.ToString();

            SetColor();
        }

        /// <summary>
        /// Get green color value from text field
        /// </summary>
        /// <param name="s">Green color value</param>
        public void GetColorGValue(string s)
        {
            if (s == "")
            {
                colorG = 0.0f;
            }
            else
            {
                colorG = float.Parse(s) / 255.0f;
            }

            gSlider.value = colorG;

            SetColor();
        }

        /// <summary>
        /// Set blue value of rgba color
        /// </summary>
        /// <param name="b">Blue value of current color</param>
        public void SetColorB(float b)
        {
            colorB = b;

            int i = (int)(255 * b);
            bPlaceholder.text = i.ToString();

            SetColor();
        }

        /// <summary>
        /// Get blue color value from text field
        /// </summary>
        /// <param name="s">Blue color value</param>
        public void GetColorBValue(string s)
        {
            if (s == "")
            {
                colorB = 0.0f;
            }
            else
            {
                colorB = float.Parse(s) / 255.0f;
            }

            bSlider.value = colorB;

            SetColor();
        }

        /// <summary>
        /// Set alpha value of rgba color
        /// </summary>
        /// <param name="a">Alpha value of current color</param>
        public void SetColorA(float a)
        {
            colorA = a;

            int i = (int)(255 * a);
            aPlaceholder.text = i.ToString();

            SetColor();
        }

        /// <summary>
        /// Get alpha value from text field
        /// </summary>
        /// <param name="s">Alpha value</param>
        public void GetColorAValue(string s)
        {
            if (s == "")
            {
                colorA = 0.0f;
            }
            else
            {
                colorA = float.Parse(s) / 255.0f;
            }

            aSlider.value = colorA;

            SetColor();
        }

        /// <summary>
        /// Save created color
        /// </summary>
        public void SaveColor()
        {
            colorButtonPicker.buttonImage.color = new Color(partColor.r, partColor.g, partColor.b, 1.0f);
        }

        /// <summary>
        /// Reset color to previous value
        /// </summary>
        public void ResetColor()
        {
            partColor = _baseColor;
            colorButtonPicker.buttonImage.color = new Color(partColor.r, partColor.g, partColor.b, 1.0f);
            modularCharacterManager.CharacterMaterial.SetColor(colorProperty, partColor);
        }

        /// <summary>
        /// Sets color for colorProperty part
        /// </summary>
        private void SetColor()
        {
            partColor = new Color(colorR, colorG, colorB, colorA);
            shownColor.color = partColor;
            colorButtonPicker.buttonImage.color = partColor;
            modularCharacterManager.CharacterMaterial.SetColor(colorProperty, partColor);
        }
    }
}