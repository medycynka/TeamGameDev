using PolyPerfect;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using UnityEngine;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class for managing passive enemy's stats
    /// </summary>
    public class PassiveEnemyStats : CharacterStats
    {
        private Common_WanderScript _commonWander;

        private void Awake()
        {
            characterTransform = transform;
            _commonWander = GetComponent<Common_WanderScript>();
            maxHealth = healthLevel * 10f;
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage, PlayerStats playerStats)
        {
            currentHealth -= damage;

            if (currentHealth <= 0f)
            {
                _commonWander.Die();
            }
            else
            {
                _commonWander.StopCurrentCoroutines();
                
                if (_commonWander.useNavMesh)
                {
                    _commonWander.RunAwayFromPlayer(playerStats);
                }
                else
                {
                    _commonWander.NonNavMeshRunAwayFromPlayer(playerStats);
                }
            }
        }
    }
}