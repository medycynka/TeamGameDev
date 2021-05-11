using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.SaveScripts;
using SzymonPeszek.GameUI.StatBars;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.GameUI;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Misc;
using SzymonPeszek.Enums;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.PlayerScripts.Controller;


namespace SzymonPeszek.PlayerScripts
{
    /// <summary>
    /// Class representing player stats like health, stamina, strength, etc.
    /// </summary>
    public class PlayerStats : CharacterStats
    {
        private PlayerManager _playerManager;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerAnimatorManager _playerAnimatorManager;
        private InputHandler _inputHandler;
        [HideInInspector] public Camera mainCamera;

        [Header("Player Properties", order = 1)]

        [Header("UI Components", order = 2)]
        public UIManager uiManager;
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusBar focusBar;
        public GameObject youDiedLogo;

        [Header("Death Drop", order = 2)]
        public Sprite deathDropIcon;
        public ConsumablePickUp soulDeathDrop;
        public Vector3 jumpDeathDropPosition;

        [Header("Unique Player Stats", order = 2)]
        public string playerName = SettingsHolder.playerName;
        public int playerLevel = 12;
        public float soulsAmount = 0;
        public float currentArmorValue;
        public float bonusBuffAttack = 1.0f;
        public float bonusBuffDefence = 1.0f;
        public float bonusBuffMagic = 1.0f;
        public float bonusBuffEndurance = 1.0f;

        [Header("Health & Stamina refill values", order = 2)]
        public float healthRefillAmount = 20f;
        public float healthBgRefillAmount = 20f;
        public float staminaRefillAmount = 20f;
        public float focusRefillAmount = 20f;

        [Header("Bools", order = 2)]
        public bool isPlayerAlive = true;
        public bool isJumpDeath;
        public bool isHavingQuest;

        public Dictionary<string, int> killCount = new Dictionary<string, int>();

        private EnemySpawner[] _enemiesSpawners;
        private RectTransform _hpBarTransform;
        private RectTransform _staminaBarTransform;
        private RectTransform _focusBarTransform;
        private ConsumablePickUp _currentDeathDrop;

        private void Awake()
        {
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            _playerManager = GetComponent<PlayerManager>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _inputHandler = GetComponent<InputHandler>();
            uiManager = FindObjectOfType<UIManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusBar = FindObjectOfType<FocusBar>();
            characterTransform = GetComponent<Transform>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            _enemiesSpawners = FindObjectsOfType<EnemySpawner>();
            youDiedLogo.SetActive(false);

            DataManager dataManager = SettingsHolder.dataManager;

            if (dataManager != null)
            {
                currentHealth = dataManager.currentHealth;
                currentStamina = dataManager.currentStamina;
                baseArmor = dataManager.baseArmor;
                strength = dataManager.strength;
                agility = dataManager.agility;
                defence = dataManager.defence;
                bonusHealth = dataManager.bonusHealth;
                bonusStamina = dataManager.bonusStamina;
                bonusFocus = dataManager.bonusFocus;
                playerLevel = dataManager.playerLevel;

                if (!dataManager.isFirstStart)
                {
                    soulsAmount = dataManager.soulsAmount;
                    
                    gameObject.transform.position = new Vector3(dataManager.spawnPointPosition[0], dataManager.spawnPointPosition[1], dataManager.spawnPointPosition[2]);
                    gameObject.transform.rotation = Quaternion.Euler(dataManager.spawnPointRotation[0], dataManager.spawnPointRotation[1], dataManager.spawnPointRotation[2]);
                    _playerManager.currentSpawnPoint.transform.position = new Vector3(dataManager.spawnPointPosition[0], dataManager.spawnPointPosition[1], dataManager.spawnPointPosition[2]);
                    _playerManager.currentSpawnPoint.transform.rotation = Quaternion.Euler(dataManager.spawnPointRotation[0], dataManager.spawnPointRotation[1], dataManager.spawnPointRotation[2]);
                }
                else
                {
                    SettingsHolder.firstStart = false;
                }
            }
            else
            {
                strength = SettingsHolder.currentStrength;
                agility = SettingsHolder.currentAgility;
                defence = SettingsHolder.currentDefence;
                bonusHealth = SettingsHolder.currentBonusHealth;
                bonusStamina = SettingsHolder.currentBonusStamina;
                bonusFocus = SettingsHolder.currentBonusFocus;
                playerLevel = SettingsHolder.currentLevel;
            }

            _hpBarTransform = healthBar.GetComponent<RectTransform>();
            _staminaBarTransform = staminaBar.GetComponent<RectTransform>();
            _focusBarTransform = focusBar.GetComponent<RectTransform>();
            
            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());
            UpdateFocusBar(SetMaxFocusFromFocusLevel());
        }

        /// <summary>
        /// Calculate stat bar size based on max stat value (health, stamina or focus)
        /// </summary>
        /// <param name="maxBarValue">Maximum stat value</param>
        /// <param name="statBarTransform">RectTransform of target stat bar</param>
        /// <param name="anchorX">Anchor X position</param>
        /// <param name="anchorY">Anchor Y position</param>
        private void SetStatBarSize(float maxBarValue, RectTransform statBarTransform, float anchorX, float anchorY)
        {
            float currentPixelWidth = 250f * (maxBarValue / 100f);
            float remappedPixelWidth = currentPixelWidth.Remap(100.0f, 1337.5f, 0.0f, 1.0f);
            float widthToSet = Mathf.Lerp(250f, Screen.width - Mathf.Lerp(60, 140, remappedPixelWidth), remappedPixelWidth);
            
            statBarTransform.anchoredPosition = new Vector2(anchorX + (widthToSet - 250f) / 2f, anchorY);
            statBarTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthToSet);
        }
        
        /// <summary>
        /// Calculate maximum health points amount
        /// </summary>
        /// <returns>Maximum health points amount</returns>
        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10 + bonusHealth * 10 + strength * 2.5f;

            return maxHealth;
        }

        /// <summary>
        /// Update health bar with new value
        /// </summary>
        /// <param name="newHealth">New health points amount</param>
        private void UpdateHealthBar(float newHealth)
        {
            maxHealth = newHealth;
            currentHealth = maxHealth;

            SetStatBarSize(maxHealth, _hpBarTransform,140f, -50f);
            
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);
        }

        /// <summary>
        /// Calculate maximum stamina points amount
        /// </summary>
        /// <returns>Maximum stamina points amount</returns>
        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10 + bonusStamina * 10 + agility * 2.5f;

            return maxStamina;
        }

        /// <summary>
        /// Update stamina bar with new value
        /// </summary>
        /// <param name="newStamina">New stamina points amount</param>
        private void UpdateStaminaBar(float newStamina)
        {
            maxStamina = newStamina;
            currentStamina = maxStamina;

            SetStatBarSize(maxStamina, _staminaBarTransform, 140f, -110f);
            
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);
        }
        
        /// <summary>
        /// Calculate maximum focus points amount
        /// </summary>
        /// <returns>Maximum focus points amount</returns>
        private float SetMaxFocusFromFocusLevel()
        {
            maxFocus = focusLevel * 10 + bonusFocus * 10;

            return maxFocus;
        }

        /// <summary>
        /// Update focus bar with new value
        /// </summary>
        /// <param name="newFocus">New focus points amount</param>
        private void UpdateFocusBar(float newFocus)
        {
            maxFocus = newFocus;
            currentFocus = maxFocus;

            SetStatBarSize(maxFocus, _focusBarTransform, 140f, -170f);
            
            focusBar.SetMaxFocus(maxFocus);
            focusBar.SetCurrentFocus(currentFocus);
        }

        /// <summary>
        /// Updates health bar, stamina bar and focus bar
        /// </summary>
        public void UpdatePlayerStats()
        {
            UpdateHealthBar(SetMaxHealthFromHealthLevel());
            UpdateStaminaBar(SetMaxStaminaFromStaminaLevel());
            UpdateFocusBar(SetMaxFocusFromFocusLevel());
        }
        
        /// <summary>
        /// Calculate damage received based on damage type and current modifiers
        /// </summary>
        /// <param name="damageType">Type of damage</param>
        /// <param name="damage">Damage amount</param>
        /// <returns>Calculated Damage</returns>
        protected override int CalculateDamage(DamageType damageType, float damage)
        {
            switch (damageType)
            {
                case DamageType.Physic:
                    float armorValue = Mathf.Clamp(currentArmorValue * bonusBuffDefence, 0, 999);
                    float defMod = Mathf.Clamp01(1 - Mathf.Lerp(armorValue, 999, armorValue / 999) / 999);
                    int damageMod = (int) (damage * defMod);
                    return damageMod;
                case DamageType.AbsolutePhysic:
                    return (int) damage;
                case DamageType.Magic:
                    // float magicDefMod = Mathf.Clamp01(1 - Mathf.Lerp(defence, 999, defence / 999) / 999); // Make magic defence stat
                    float magicDefMod = 1.0f;
                    int magicMod = (int) (damage * magicDefMod);
                    return magicMod;
                case DamageType.AbsoluteMagic:
                    return (int) damage;
                case DamageType.Fall:
                    return (int) damage;
                case DamageType.Other:
                    return (int) damage;
                default:
                    return (int) damage;
            }
        }

        /// <summary>
        /// Damage player
        /// </summary>
        /// <param name="damage">Damage dealt to the player</param>
        /// <param name="damageType">Type of damage</param>
        /// <param name="damageAnimation">Name of damage animation</param>
        /// <param name="isBackStabbed">Is damage from back stab?</param>
        /// <param name="isRiposted">Is damage from riposte?</param>
        public void TakeDamage(float damage, DamageType damageType, string damageAnimation = "Damage_01", 
            bool isBackStabbed = false, bool isRiposted = false)
        {
            if (isPlayerAlive && !_playerManager.isInvulnerable)
            {
                _playerManager.shouldRefillHealth = false;
                currentHealth -= CalculateDamage(damageType, damage);
                healthBar.SetCurrentHealth(currentHealth);

                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[damageAnimation], true);

                if (currentHealth <= 0)
                {
                    HandleDeathAndRespawn(isBackStabbed);
                }
            }
        }
        
        /// <summary>
        /// Handle getting paried
        /// </summary>
        public void GetParried()
        {
            _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.ParriedName], true);
        }

        /// <summary>
        /// Heal player to max health
        /// </summary>
        public void RefillHealth()
        {
            currentHealth += healthRefillAmount * Time.deltaTime;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.healthBarSlider.value += healthRefillAmount * Time.deltaTime;
            healthBar.backgroundSlider.value += healthRefillAmount * Time.deltaTime;
        }

        /// <summary>
        /// Heal player with given amount of health
        /// </summary>
        /// <param name="healAmount">Health points amount to heal</param>
        public void HealPlayer(float healAmount)
        {
            currentHealth += healAmount * bonusBuffMagic;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.healthBarSlider.value = currentHealth;
            healthBar.backgroundSlider.value = currentHealth;
        }

        /// <summary>
        /// Buffs player stats
        /// </summary>
        /// <param name="buffType">Type fo buff</param>
        /// <param name="buffRang">Rang of the buff</param>
        /// <param name="value">Buff value</param>
        public void BuffPlayer(StatsBuffType buffType, BuffRang buffRang, float value)
        {
            switch (buffType)
            {
                case StatsBuffType.Attack:
                    StopCoroutine(BuffAttack(buffRang, value));
                    StartCoroutine(BuffAttack(buffRang, value));
                    break;
                case StatsBuffType.Defence:
                    StopCoroutine(BuffDefence(buffRang, value));
                    StartCoroutine(BuffDefence(buffRang, value));
                    break;
                case StatsBuffType.MagicAttack:
                    StopCoroutine(BuffMagic(buffRang, value));
                    StartCoroutine(BuffMagic(buffRang, value));
                    break;
                case StatsBuffType.Endurance:
                    StopCoroutine(BuffEndurance(buffRang, value));
                    StartCoroutine(BuffEndurance(buffRang, value));
                    break;
            }
        }

        /// <summary>
        /// Drain stamina points during attack, sprint, etc
        /// </summary>
        /// <param name="drain">Amount of stamina points to drain</param>
        public void TakeStaminaDamage(float drain)
        {
            currentStamina = currentStamina - drain;

            if(currentStamina < 0)
            {
                currentStamina = 0;
            }

            staminaBar.SetCurrentStamina(currentStamina);
        }

        /// <summary>
        /// Refill stamina to max stamina
        /// </summary>
        public void RefillStamina()
        {
            currentStamina += staminaRefillAmount * Time.deltaTime;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            staminaBar.staminaBarSlider.value += staminaRefillAmount * Time.deltaTime;
        }

        /// <summary>
        /// Replenish player's stamina with given amount of stamina
        /// </summary>
        /// <param name="staminaAmount">Health points amount to heal</param>
        public void HealStamina(float staminaAmount)
        {
            currentStamina += staminaAmount * bonusBuffMagic;
            
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            staminaBar.staminaBarSlider.value = currentStamina;
        }
        
        /// <summary>
        /// Drain focus points during magic action
        /// </summary>
        /// <param name="drain">Amount of focus points to drain</param>
        public void TakeFocusDamage(float drain)
        {
            currentFocus = currentFocus - drain;

            if(currentFocus < 0)
            {
                currentFocus = 0;
            }

            focusBar.SetCurrentFocus(currentFocus);
        }

        /// <summary>
        /// Refill focus to max focus
        /// </summary>
        public void RefillFocus()
        {
            currentFocus += focusRefillAmount * Time.deltaTime;

            if (currentFocus > maxFocus)
            {
                currentFocus = maxFocus;
            }

            focusBar.focusBarSlider.value += focusRefillAmount * Time.deltaTime;
        }

        /// <summary>
        /// Deal damage to the enemy
        /// </summary>
        /// <param name="enemyStats">Enemy's stats</param>
        /// <param name="weaponDamage">Damage to deal</param>
        /// <param name="damageType">Damage Type</param>
        /// <param name="isPassive">Is it passive enemy</param>
        /// <param name="passiveEnemyStats">Passive enemy stats</param>
        public void DealDamage(EnemyStats enemyStats, float weaponDamage, DamageType damageType, bool isPassive = false, PassiveEnemyStats passiveEnemyStats = null)
        {
            if (isPassive && passiveEnemyStats != null)
            {
                passiveEnemyStats.TakeDamage(CalculateDamage(weaponDamage, false), this);
            }
            else
            {
                enemyStats.TakeDamage(CalculateDamage(weaponDamage, false), this, damageType);
            }
        }

        /// <summary>
        /// Calculate damage based on player's stats
        /// </summary>
        /// <param name="weaponDamage">Current weapon damage</param>
        /// <param name="isFromBow">Is current weapon a bow</param>
        /// <returns></returns>
        public float CalculateDamage(float weaponDamage, bool isFromBow)
        {
            if (isFromBow)
            {
                return Mathf.RoundToInt(
                    ((weaponDamage + _weaponSlotManager.attackingWeapon.baseAttack) *
                        _weaponSlotManager.attackingWeapon.heavyAttackDamageMult + strength * 0.125f) *
                    bonusBuffAttack);
            }

            return Mathf.RoundToInt((weaponDamage * _weaponSlotManager.attackingWeapon.lightAttackDamageMult +
                                    strength * 0.5f) * bonusBuffAttack);
        }

        /// <summary>
        /// Calculate soul costs for stats upgrade
        /// </summary>
        /// <param name="level">Player's level after upgrade</param>
        /// <returns>Soul cost of upgrade</returns>
        public int CalculateSoulsCost(int level)
        {
            return (int)(0.02f * level * level * level + 3.06f * level * level + 105.6f * level - 895f);
        }
        
        /// <summary>
        /// Is player locking-on an enemy?
        /// </summary>
        /// <returns></returns>
        public bool IsLockOn()
        {
            return _inputHandler.lockOnFlag;
        }

        /// <summary>
        /// Increase player kill count of specific enemy
        /// </summary>
        /// <param name="enemyName">Recently killed enemy's name</param>
        public void IncrementEnemyKillCount(string enemyName)
        {
            if (isHavingQuest)
            {
                if (killCount.ContainsKey(enemyName))
                {
                    killCount[enemyName]++;
                }
                else
                {
                    killCount.Add(enemyName, 1);
                }
            }
        }

        /// <summary>
        /// Check if player killed specific amount of certain enemies
        /// </summary>
        /// <param name="enemyName">Name of enemies to kill</param>
        /// <param name="killNeeded">Number of kills needed</param>
        /// <returns></returns>
        public bool IsKillCountFulfilled(string enemyName, int killNeeded)
        {
            if (killCount.ContainsKey(enemyName))
            {
                return killCount[enemyName] == killNeeded;
            }

            return false;
        }
        
        /// <summary>
        /// Respawn player after death
        /// </summary>
        /// <param name="isBackStabbed">Was player killed from back stab?</param>
        private void HandleDeathAndRespawn(bool isBackStabbed)
        {
            currentHealth = 0;
            _playerAnimatorManager.UpdateAnimatorValues(0, 0, false, false);
            _playerAnimatorManager.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName], true);
            
            if (isJumpDeath)
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.LayDownName], true);
            }
            else if(isBackStabbed)
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BackStabName], true);
            }
            else
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.Death01Name], true);
            }

            isPlayerAlive = false;

            StartCoroutine(Respawn());
        }

        /// <summary>
        /// Player respawn coroutine
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator Respawn()
        {
            youDiedLogo.SetActive(true);
            DropSouls(isJumpDeath ? jumpDeathDropPosition : transform.position);

            yield return CoroutineYielder.waitFor5Second;

            youDiedLogo.SetActive(false);
            _playerManager.quickMoveScreen.SetActive(true);
            _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.EmptyName], false);
            UpdateHealthBar(maxHealth);
            UpdateStaminaBar(maxStamina);
            characterTransform.position = _playerManager.currentSpawnPoint.transform.position;
            characterTransform.rotation = _playerManager.currentSpawnPoint.transform.rotation;
            // Respawn enemies and refresh boss health if alive
            RespawnEnemiesOnDead();

            yield return CoroutineYielder.waitFor5Second;
            
            isPlayerAlive = true;
            _playerAnimatorManager.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName], false);
            _playerManager.quickMoveScreen.SetActive(false);

            if (isJumpDeath)
            {
                isJumpDeath = false;
            }
        }
        
        /// <summary>
        /// Respawn enemies when player dies
        /// </summary>
        private void RespawnEnemiesOnDead()
        {
            foreach (var eS in _enemiesSpawners)
            {
                eS.SpawnEnemies();
            }
        }

        /// <summary>
        /// Drop owned souls after death
        /// </summary>
        /// <param name="dropPosition">Position of souls drop</param>
        private void DropSouls(Vector3 dropPosition)
        {
            if (soulsAmount > 0)
            {
                ConsumableItem deathDrop = ScriptableObject.CreateInstance<ConsumableItem>();
                deathDrop.soulAmount = soulsAmount;
                deathDrop.itemName = "Souls recovered";
                deathDrop.itemIcon = deathDropIcon;
                deathDrop.consumableType = ConsumableType.SoulItem;
                deathDrop.isDeathDrop = true;
                soulsAmount = 0;
                uiManager.UpdateSouls();
                soulDeathDrop.consumableItems = new []{ deathDrop };
                soulDeathDrop.interactableText = "Recover souls";

                if (_currentDeathDrop != null)
                {
                    Destroy(_currentDeathDrop.gameObject);
                    _currentDeathDrop = null;
                }
                
                _currentDeathDrop = Instantiate(soulDeathDrop, dropPosition, Quaternion.identity);
            }
        }

        /// <summary>
        /// Coroutine which buffs player attack
        /// </summary>
        /// <param name="buffRang">Rang of the buff</param>
        /// <param name="value">Buff's value</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator BuffAttack(BuffRang buffRang, float value)
        {
            bonusBuffAttack = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.waitFor20Second;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.waitFor40Second;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.waitFor60Second;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffAttack = 1.0f;
        }
        
        /// <summary>
        /// Coroutine which buffs player defence
        /// </summary>
        /// <param name="buffRang">Rang of the buff</param>
        /// <param name="value">Buff's value</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator BuffDefence(BuffRang buffRang, float value)
        {
            bonusBuffDefence = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.waitFor20Second;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.waitFor40Second;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.waitFor60Second;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffDefence = 1.0f;
        }
        
        /// <summary>
        /// Coroutine which buffs player magic attack
        /// </summary>
        /// <param name="buffRang">Rang of the buff</param>
        /// <param name="value">Buff's value</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator BuffMagic(BuffRang buffRang, float value)
        {
            bonusBuffMagic = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.waitFor20Second;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.waitFor40Second;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.waitFor60Second;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffAttack = 1.0f;
        }
        
        /// <summary>
        /// Coroutine which buffs player endurance
        /// </summary>
        /// <param name="buffRang">Rang of the buff</param>
        /// <param name="value">Buff's value</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator BuffEndurance(BuffRang buffRang, float value)
        {
            bonusBuffEndurance = value;

            switch (buffRang)
            {
                case BuffRang.Lesser:
                    yield return CoroutineYielder.waitFor20Second;
                    break;
                case BuffRang.Medium:
                    yield return CoroutineYielder.waitFor40Second;
                    break;
                case BuffRang.Grand:
                    yield return CoroutineYielder.waitFor60Second;
                    break;
                default:
                    yield return null;
                    break;
            }

            bonusBuffEndurance = 1.0f;
        }
    }
}