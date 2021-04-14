using SzymonPeszek.BaseClasses;
using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.Enums;
using SzymonPeszek.Misc.ColliderManagers;


namespace SzymonPeszek.Damage
{
    /// <summary>
    /// Class for dealing damage from attacks
    /// </summary>
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage Collider", order = 0)]
        [SerializeField] private Collider damageCollider;
        [SerializeField] private CharacterManager characterManager;

        [Header("Weapon Damage", order = 1)]
        public float currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        /// <summary>
        /// Enable object's collider
        /// </summary>
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        /// <summary>
        /// Disable object's collider
        /// </summary>
        public void DisableDamageCollider()
        {
            if (damageCollider != null)
            {
                damageCollider.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (characterManager == null)
            {
                characterManager = GetComponentInParent<CharacterManager>();
            }
            
            if (collision.CompareTag("Player"))
            {
                EnemyStats enemyStats = GetComponentInParent<EnemyStats>();
                PlayerManager playerManager = collision.GetComponentInParent<PlayerManager>();

                if (playerManager.isParrying)
                {
                    enemyStats.GetParried();
                    
                    return;
                }
                
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                
                if(playerManager.isBlocking)
                {
                    BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

                    if (shield != null)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage -
                                                         (currentWeaponDamage *
                                                          shield.blockingPhysicalDamageAbsorption) / 100;

                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), DamageType.Physic,
                                "Block_Guard");
                            
                            return;
                        }
                    }
                }

                if (playerStats != null && enemyStats != null)
                {
                    enemyStats.DealDamage(playerStats);
                }
            }

            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
                PlayerStats playerStats = GetComponentInParent<PlayerStats>();
                EnemyManager enemyManager = collision.GetComponentInParent<EnemyManager>();

                if (enemyManager.isParrying)
                {
                    playerStats.GetParried();
                    
                    return;
                }

                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                
                if(enemyManager.isBlocking)
                {
                    BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

                    if (shield != null)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage -
                                                         (currentWeaponDamage *
                                                          shield.blockingPhysicalDamageAbsorption) / 100;

                        if (playerStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), DamageType.Physic,
                                "Block_Guard");
                            
                            return;
                        }
                    }
                }
                
                if (enemyStats != null && playerStats != null)
                {
                    playerStats.DealDamage(enemyStats, currentWeaponDamage, DamageType.Physic);
                }
            }

            if (collision.CompareTag("Passive"))
            {
                PlayerStats playerStats = GetComponentInParent<PlayerStats>();
                PassiveEnemyStats passiveEnemyStats = collision.GetComponentInParent<PassiveEnemyStats>();

                if (passiveEnemyStats != null && playerStats != null)
                {
                    playerStats.DealDamage(null, currentWeaponDamage, DamageType.Physic, true, passiveEnemyStats);
                }
            }
        }
    }
}