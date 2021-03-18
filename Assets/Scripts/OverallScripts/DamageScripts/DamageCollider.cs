using SzymonPeszek.BaseClasses;
using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.EnemyScripts;


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
                
                if (enemyStats != null && playerStats != null)
                {
                    playerStats.DealDamage(enemyStats, currentWeaponDamage);
                }
            }
        }
    }
}