using System;
using System.Security.Cryptography;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using UnityEngine;


namespace SzymonPeszek.Items.Spells.Helpers
{
    /// <summary>
    /// Class for detecting spell projectiles collision
    /// </summary>
    public class SpellCollision : MonoBehaviour
    {
        public GameObject collisionFx;
        
        [HideInInspector] public float damage;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Transform projectileTransform;
        [HideInInspector] public float maxTravelDistanceSqr = 10000f;
        [HideInInspector] public PlayerStats playerStats;
        [HideInInspector] public string fromWho;

        private Vector3 _currentDistanceVector;
        private bool _hasCollided;
        private bool _firstCollision;
        private const string PlayerTag = "Player";
        private const string EnemyTag = "Enemy";
        private const string DestructibleTag = "Destructible";

        private void FixedUpdate()
        {
            _currentDistanceVector = startPosition - projectileTransform.position;
            
            if (_currentDistanceVector.sqrMagnitude >= maxTravelDistanceSqr)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasCollided)
            {
                if (!_firstCollision && other.CompareTag(fromWho))
                {
                    _firstCollision = true;
                    
                    return;
                }
                
                _hasCollided = true;
                
                if (collisionFx)
                {
                    collisionFx = Instantiate(collisionFx, transform.position, Quaternion.identity);
                    Destroy(collisionFx, 1.5f);
                }

                if (other.CompareTag(PlayerTag))
                {
                    other.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.Magic);
                }
                else if (other.CompareTag(EnemyTag))
                {
                    other.GetComponent<EnemyStats>().TakeDamage(damage, playerStats, DamageType.Magic);
                }
                else if (other.CompareTag(DestructibleTag))
                {
                    Destroy(other.gameObject);
                }
                
                Destroy(gameObject, 0.15f);
            }
        }
    }
}