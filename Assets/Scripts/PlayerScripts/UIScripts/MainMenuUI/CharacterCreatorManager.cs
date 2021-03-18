using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleDrakeStudios.ModularCharacters;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.MainMenuUI
{
    public class CharacterCreatorManager : MonoBehaviour
    {
        /// <summary>
        /// Class which manages character creation in main menu 
        /// </summary>
        private MainMenuManager _mainMenuManager;
        
        [Header("Character Creator Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public GameObject creatorScreen;
        public ModularCharacterManager modularCharacterManager;
        public TMP_InputField nameInputField;
        public Slider headSlider;
        public Slider hairSlider;
        public Slider eyebrowSlider;
        public Slider earSlider;
        public Slider facialHairSlider;

        [Header("Character Stats Components", order = 1)]
        public int currentLevel = 1;
        public int pointsToSpend = 11;
        public float startStrength = 1f;
        public float startAgility;
        public float startDefence;
        public float startBonusHealth;
        public float startBonusStamina;
        public float startBonusFocus;
        public TextMeshProUGUI pointsToSpendText;
        public TextMeshProUGUI strengthText;
        public TextMeshProUGUI agilityText;
        public TextMeshProUGUI defenceText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI focusText;

        private void Start()
        {
            _mainMenuManager = GetComponentInParent<MainMenuManager>();
            
            strengthText.text = startStrength.ToString();
            agilityText.text = startAgility.ToString();
            defenceText.text = startDefence.ToString();
            healthText.text = startBonusHealth.ToString();
            staminaText.text = startBonusStamina.ToString();
            focusText.text = startBonusFocus.ToString();
            pointsToSpendText.text = pointsToSpend.ToString();
        }

        /// <summary>
        /// Load main level if character is created
        /// </summary>
        public void PlayGame()
        {
            if (currentLevel == 12 && pointsToSpend == 0)
            {
                SaveStartingStats();
                SettingsHolder.isCharacterCreated = true;
                SaveManager.SaveMainMenu();
                creatorScreen.SetActive(false);

                _mainMenuManager.FadeOutMusic();
            }
        }

        /// <summary>
        /// Save created character's stats
        /// </summary>
        private void SaveStartingStats()
        {
            SettingsHolder.currentLevel = currentLevel;
            SettingsHolder.currentStrength = startStrength;
            SettingsHolder.currentAgility = startAgility;
            SettingsHolder.currentDefence = startDefence;
            SettingsHolder.currentBonusHealth = startBonusHealth;
            SettingsHolder.currentBonusStamina = startBonusStamina;
            SettingsHolder.currentBonusFocus = startBonusFocus;
        }

        #region Character Appearance
        /// <summary>
        /// Set character's gender to male
        /// </summary>
        public void SetMaleGender()
        {
            if (!SettingsHolder.isMale)
            {
                SettingsHolder.isMale = true;
                modularCharacterManager.SwapGender(Gender.Male);

                headSlider.value = 0;
                hairSlider.value = -1;
                eyebrowSlider.maxValue = 9;
                eyebrowSlider.value = -1;
                earSlider.value = -1;
                facialHairSlider.value = -1;
            }
        }

        /// <summary>
        /// Set character's gender to female
        /// </summary>
        public void SetFemaleGender()
        {
            if (SettingsHolder.isMale)
            {
                SettingsHolder.isMale = false;
                modularCharacterManager.SwapGender(Gender.Female);

                headSlider.value = 0;
                hairSlider.value = -1;
                eyebrowSlider.maxValue = 6;
                eyebrowSlider.value = -1;
                earSlider.value = -1;
                facialHairSlider.value = -1;
            }
        }

        /// <summary>
        /// Set character's name
        /// </summary>
        /// <param name="newName">Player name</param>
        public void SetPlayerName(string newName)
        {
            SettingsHolder.playerName = newName;
        }

        /// <summary>
        /// Set character's head
        /// </summary>
        /// <param name="partID">Head's part id</param>
        public void SetHead(float partID)
        {
            SettingsHolder.partsID[2] = (int)partID;
            modularCharacterManager.ActivatePart(ModularBodyPart.Head, (int)partID);
        }
        
        /// <summary>
        /// Set character's hairs
        /// </summary>
        /// <param name="partID">Hairs part id</param>
        public void SetHair(float partID)
        {
            SettingsHolder.partsID[5] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.Hair, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.Hair);
            }
        }

        /// <summary>
        /// Set character's eyebrows
        /// </summary>
        /// <param name="partID">Eyebrows part id</param>
        public void SetEyebrow(float partID)
        {
            SettingsHolder.partsID[6] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.Eyebrow, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.Eyebrow);
            }
        }

        /// <summary>
        /// Set character's ears
        /// </summary>
        /// <param name="partID">Ears part id</param>
        public void SetEar(float partID)
        {
            SettingsHolder.partsID[7] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.Ear, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.Ear);
            }
        }

        /// <summary>
        /// Set character's facial hair
        /// </summary>
        /// <param name="partID">Facial hairs part id</param>
        public void SetFacialHair(float partID)
        {
            SettingsHolder.partsID[8] = (int)partID;

            if (partID > -1)
            {
                modularCharacterManager.ActivatePart(ModularBodyPart.FacialHair, (int)partID);
            }
            else
            {
                modularCharacterManager.DeactivatePart(ModularBodyPart.FacialHair);
            }
        }

        /// <summary>
        /// Resets character's look
        /// </summary>
        public void ResetCharacterLook()
        {
            SetMaleGender();
            //SetPlayerName("");
            nameInputField.text = "";
            //SetHead(0);
            headSlider.value = 0;
            //SetHair(-1);
            hairSlider.value = -1;
            //SetEyebrow(-1);
            eyebrowSlider.value = -1;
            //SetEar(-1);
            earSlider.value = -1;
            //SetFacialHair(-1);
            facialHairSlider.value = -1;
        }
        #endregion

        #region Character Stats
        /// <summary>
        /// Increase player's strength
        /// </summary>
        public void IncreaseStrength()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startStrength += 1f;
                pointsToSpend--;

                strengthText.text = startStrength.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Decrease player's strength
        /// </summary>
        public void DecreaseStrength()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startStrength > 0)
            {
                currentLevel--;
                startStrength -= 1f;
                pointsToSpend++;
                
                strengthText.text = startStrength.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Increase player's agility
        /// </summary>
        public void IncreaseAgility()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startAgility += 1f;
                pointsToSpend--;
                
                agilityText.text = startAgility.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Decrease player's agility
        /// </summary>
        public void DecreaseAgility()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startAgility > 0)
            {
                currentLevel--;
                startAgility -= 1f;
                pointsToSpend++;
                
                agilityText.text = startAgility.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Increase player's defence
        /// </summary>
        public void IncreaseDefence()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startDefence += 1f;
                pointsToSpend--;
                
                defenceText.text = startDefence.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Decrease player's defence
        /// </summary>
        public void DecreaseDefence()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startDefence > 0)
            {
                currentLevel--;
                startDefence -= 1f;
                pointsToSpend++;
                
                defenceText.text = startDefence.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Increase player's health
        /// </summary>
        public void IncreaseHealth()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startBonusHealth += 1f;
                pointsToSpend--;
                
                healthText.text = startBonusHealth.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Decrease player's health
        /// </summary>
        public void DecreaseHealth()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startBonusHealth > 0)
            {
                currentLevel--;
                startBonusHealth -= 1f;
                pointsToSpend++;
                
                healthText.text = startBonusHealth.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Increase player's stamina
        /// </summary>
        public void IncreaseStamina()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startBonusStamina += 1f;
                pointsToSpend--;
                
                staminaText.text = startBonusStamina.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Decrease player's stamina
        /// </summary>
        public void DecreaseStamina()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startBonusStamina > 0)
            {
                currentLevel--;
                startBonusStamina -= 1f;
                pointsToSpend++;
                
                staminaText.text = startBonusStamina.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Increase player's focus
        /// </summary>
        public void IncreaseFocus()
        {
            if (pointsToSpend > 0 && currentLevel < 12)
            {
                currentLevel++;
                startBonusFocus += 1f;
                pointsToSpend--;
                
                focusText.text = startBonusFocus.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }
        
        /// <summary>
        /// Decrease player's focus
        /// </summary>
        public void DecreaseFocus()
        {
            if (currentLevel > 1 && pointsToSpend < 12 && startBonusFocus > 0)
            {
                currentLevel--;
                startBonusFocus -= 1f;
                pointsToSpend++;
                
                focusText.text = startBonusFocus.ToString();
                pointsToSpendText.text = pointsToSpend.ToString();
            }
        }

        /// <summary>
        /// Resets Beginning Stats
        /// </summary>
        public void ResetStats()
        {
            currentLevel = 1;
            pointsToSpend = 11;
            startStrength = 1f;
            startAgility = 0f;
            startDefence = 0f;
            startBonusHealth = 0f;
            startBonusStamina = 0f;
            startBonusFocus = 0f;
            
            strengthText.text = startStrength.ToString();
            agilityText.text = startAgility.ToString();
            defenceText.text = startDefence.ToString();
            healthText.text = startBonusHealth.ToString();
            staminaText.text = startBonusStamina.ToString();
            focusText.text = startBonusFocus.ToString();
            pointsToSpendText.text = pointsToSpend.ToString();
        }
        #endregion
    }
}