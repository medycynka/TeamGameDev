﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Environment.Areas;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Enums;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class for managing enemy's stats
    /// </summary>
    public class EnemyStats : CharacterStats
    {
        private EnemyManager _enemyManager;

        [Header("Enemy Properties", order = 1)]
        [Header("Animator", order = 2)]
        public EnemyAnimationManager animator;

        [Header("Health Bar", order = 2)]
        public GameObject healthBar;
        public Image healthBarFill;
        public TextMeshProUGUI damageValue;

        [Header("Enemy's name", order = 2)]
        public string enemyName;
        
        [Header("Attack properties", order = 2)]
        public float enemyAttack = 25f;

        [Header("Souls & souls target", order = 2)]
        public float soulsGiveAmount;
        public PlayerStats playerStats;
        public GameObject enemyObject;

        [Header("Is it Boss", order = 2)]
        public bool isBoss;
        public BossAreaManager bossAreaManager;
        public Slider bossHpSlider;

        private Transform _mainCameraTransform;

        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            animator = GetComponentInChildren<EnemyAnimationManager>();
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            characterTransform = GetComponent<Transform>();

            if (!(Camera.main is null))
            {
                _mainCameraTransform = Camera.main.transform;
            }
        }

        void Start()
        {
            InitializeHealth();
        }

        private void LateUpdate()
        {
            if (!isBoss)
            {
                healthBar.transform.LookAt(_mainCameraTransform);
                healthBar.transform.Rotate(0, 180, 0);
            }
        }
        
        /// <summary>
        /// Calculate maximum health value
        /// </summary>
        /// <returns>Maximum health value</returns>
        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        /// <summary>
        /// Initialize character's health
        /// </summary>
        public void InitializeHealth()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            
            if (isBoss)
            {
                if (bossHpSlider == null)
                {
                    bossHpSlider = bossAreaManager.bossHpBar.GetComponentInChildren<Slider>();
                }
            }
            else
            {
                if (healthBarFill == null)
                {
                    healthBarFill = healthBar.GetComponentInChildren<Image>();
                }
                
                healthBarFill.fillAmount = 1f;
                healthBar.SetActive(false);
            }
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
                    float armorValue = Mathf.Clamp(defence * 2.5f + baseArmor, 0, 999);
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
        /// Take damage from player
        /// </summary>
        /// <param name="damage">Damage amount</param>
        /// <param name="damageType">Type of damage</param>
        /// <param name="damageAnimation">Name of damage animation</param>
        /// <param name="isBackStabbed">Is it from back stab?</param>
        /// <param name="isRiposted">Is it from riposte?</param>
        public void TakeDamage(float damage, DamageType damageType, string damageAnimation = "Damage_01", bool isBackStabbed = false, bool isRiposted = false)
        {
            if (_enemyManager.isAlive)
            {
                if (isBoss)
                {
                    currentHealth -= CalculateDamage(damageType, damage);
                    bossHpSlider.value = currentHealth;

                    if (currentHealth > 0)
                    {
                        animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[damageAnimation], true);
                    }
                    else
                    {
                        _enemyManager.deadFromBackStab = isBackStabbed;
                        bossAreaManager.bossHpBar.SetActive(false);
                    }
                }
                else
                {
                    StartCoroutine(UpdateEnemyHealthBar(CalculateDamage(damageType, damage), isBackStabbed, isRiposted));
                }
            }
        }

        public void GetParried()
        {
            animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.ParriedName], true);
        }

        /// <summary>
        /// Deal damage to the player
        /// </summary>
        /// <param name="playerStat">Player stats</param>
        /// <param name="damageAnimation">Name of damage animation</param>
        public void DealDamage(PlayerStats playerStat, string damageAnimation = "Damage_01")
        {
            playerStat.TakeDamage(Mathf.RoundToInt(enemyAttack), DamageType.Physic, damageAnimation);
        }

        /// <summary>
        /// Coroutine for updating enemy's health bar
        /// </summary>
        /// <param name="damage">Damage get form player</param>
        /// <param name="isBackStabbed">Is it from back stab?</param>
        /// <param name="isRiposted">Is it from riposte?</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator UpdateEnemyHealthBar(float damage, bool isBackStabbed, bool isRiposted)
        {
            _enemyManager.deadFromBackStab = (isBackStabbed && currentHealth - damage <= 0.0f);
            _enemyManager.deadFromRiposte = (isRiposted && currentHealth - damage <= 0.0f);
            healthBar.SetActive(true);
            currentHealth -= damage;
            healthBarFill.fillAmount = currentHealth / maxHealth;
            damageValue.text = damage.ToString();

            if (currentHealth > 0)
            {
                if (isBackStabbed)
                {
                    animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.BackStabbedName],
                        true);
                }
                else if (isRiposted)
                {
                    animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.RipostedName],
                        true);
                    _enemyManager.isGettingRiposted = false;
                }
                else
                {
                    animator.PlayTargetAnimation(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.Damage01Name],
                        true);
                }
            }

            yield return CoroutineYielder.waitFor3HalfSeconds;

            healthBar.SetActive(false);
        }
    }

}
