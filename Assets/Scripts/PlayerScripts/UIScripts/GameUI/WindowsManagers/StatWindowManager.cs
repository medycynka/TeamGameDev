using UnityEngine;
using TMPro;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    /// <summary>
    /// Class for upgrading player's stats during resting at the bonfire
    /// </summary>
    public class StatWindowManager : MonoBehaviour
    {
        public PlayerStats playerStats;
        public int levelToAdd;
        public int soulsCost;
        public float strengthToAdd;
        public float agilityToAdd;
        public float defenceToAdd;
        public float healthToAdd;
        public float staminaToAdd;

        public TextMeshProUGUI visibleLevel;
        public TextMeshProUGUI visibleSouls;
        public TextMeshProUGUI visibleStrength;
        public TextMeshProUGUI visibleAgility;
        public TextMeshProUGUI visibleDefence;
        public TextMeshProUGUI visibleHealth;
        public TextMeshProUGUI visibleStamina;

        private bool _shouldUpdateSouls = true;

        private void Start()
        {
            visibleLevel.color = Color.white;
            visibleSouls.color = Color.white;
            visibleStrength.color = Color.white;
            visibleAgility.color = Color.white;
            visibleDefence.color = Color.white;
            visibleHealth.color = Color.white;
            visibleStamina.color = Color.white;

            UpdateVisibleValues();
        }

        /// <summary>
        /// Update visible stats base on player's stats
        /// </summary>
        public void UpdateVisibleValues()
        {
            visibleLevel.text = playerStats.playerLevel.ToString();
            visibleSouls.color = Color.white;
            visibleSouls.text = soulsCost.ToString();
            visibleStrength.text = playerStats.strength.ToString();
            visibleAgility.text = playerStats.agility.ToString();
            visibleDefence.text = playerStats.defence.ToString();
            visibleHealth.text = playerStats.bonusHealth.ToString();
            visibleStamina.text = playerStats.bonusStamina.ToString();
        }

        /// <summary>
        /// Update visible level value
        /// </summary>
        /// <param name="update">Increase or decrease level</param>
        private void UpdateLevel(bool update)
        {
            if (update)
            {
                levelToAdd += 1;
            }
            else
            {
                levelToAdd -= 1;
                if (levelToAdd < 0)
                {
                    levelToAdd = 0;
                }
            }

            visibleLevel.text = (playerStats.playerLevel + levelToAdd).ToString();
        }

        /// <summary>
        /// Update visible soul costs base on current level
        /// </summary>
        private void UpdateSouls()
        {
            if (levelToAdd > 0)
            {
                soulsCost = playerStats.CalculateSoulsCost(playerStats.playerLevel + levelToAdd);

                visibleSouls.color = soulsCost > playerStats.soulsAmount ? Color.red : Color.white;
            }
            else
            {
                visibleSouls.color = Color.white;
                soulsCost = 0;
            }

            visibleSouls.text = soulsCost.ToString();
        }

        /// <summary>
        /// Update visible strength value
        /// </summary>
        /// <param name="update">Increase or decrease strength</param>
        public void UpdateStrength(bool update) 
        {
            if (update)
            {
                strengthToAdd += 1;
                if (playerStats.strength + strengthToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    strengthToAdd = 99 - playerStats.strength;
                }
            }
            else
            {
                strengthToAdd -= 1;
                if (strengthToAdd < 0)
                {
                    strengthToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleStrength.text = (playerStats.strength + strengthToAdd).ToString();
        }

        /// <summary>
        /// Update visible agility value
        /// </summary>
        /// <param name="update">Increase or decrease agility</param>
        public void UpdateAgility(bool update)
        {
            if (update)
            {
                agilityToAdd += 1;
                if (playerStats.agility + agilityToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    agilityToAdd = 99 - playerStats.agility;
                }
            }
            else
            {
                agilityToAdd -= 1;
                if (agilityToAdd < 0)
                {
                    agilityToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleAgility.text = (playerStats.agility + agilityToAdd).ToString();
        }

        /// <summary>
        /// Update visible defence value
        /// </summary>
        /// <param name="update">Increase or decrease defence</param>
        public void UpdateDefence(bool update)
        {
            if (update)
            {
                defenceToAdd += 1;
                if (playerStats.defence + defenceToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    defenceToAdd = 99 - playerStats.defence;
                }
            }
            else
            {
                defenceToAdd -= 1;
                if (defenceToAdd < 0)
                {
                    defenceToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleDefence.text = (playerStats.defence + defenceToAdd).ToString();
        }

        /// <summary>
        /// Update visible health value
        /// </summary>
        /// <param name="update">Increase or decrease health</param>
        public void UpdateHealth(bool update)
        {
            if (update)
            {
                healthToAdd += 1;
                if (playerStats.bonusHealth + healthToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    healthToAdd = 99 - playerStats.bonusHealth;
                }
            }
            else
            {
                healthToAdd -= 1;
                if (healthToAdd < 0)
                {
                    healthToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleHealth.text = (playerStats.bonusHealth + healthToAdd).ToString();
        }

        /// <summary>
        /// Update visible stamina value
        /// </summary>
        /// <param name="update">Increase or decrease stamina</param>
        public void UpdateStamina(bool update)
        {
            if (update)
            {
                staminaToAdd += 1;
                if (playerStats.bonusStamina + staminaToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    staminaToAdd = 99 - playerStats.bonusStamina;
                }
            }
            else
            {
                staminaToAdd -= 1;
                if (staminaToAdd < 0)
                {
                    staminaToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleStamina.text = (playerStats.bonusStamina + staminaToAdd).ToString();
        }

        /// <summary>
        /// Upgrade player if posses needed amount of souls
        /// </summary>
        public void UpgradePlayer()
        {
            if (soulsCost <= playerStats.soulsAmount)
            {
                playerStats.playerLevel += levelToAdd;
                playerStats.soulsAmount -= soulsCost;
                playerStats.strength += strengthToAdd;
                playerStats.agility += agilityToAdd;
                playerStats.defence += defenceToAdd;
                playerStats.bonusHealth += healthToAdd;
                playerStats.bonusStamina += staminaToAdd;

                playerStats.UpdatePlayerStats();

                ResetAddedStatsValues();
                UpdateVisibleValues();
            }
        }

        /// <summary>
        /// Reset added values
        /// </summary>
        public void ResetAddedStatsValues()
        {
            levelToAdd = 0;
            soulsCost = 0;
            strengthToAdd = 0;
            agilityToAdd = 0;
            defenceToAdd = 0;
            healthToAdd = 0;
            staminaToAdd = 0;
        }
    }

}