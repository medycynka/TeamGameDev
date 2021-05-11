using System;
using System.Security.Cryptography;
using SzymonPeszek.EnemyScripts;
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

        private Vector3 _currentDistanceVector;
        private bool _hasCollided;
        private const string EnemyTag = "Enemy";
        private const string DestructibleTag = "Destructible";

        private void Update()
        {
            _currentDistanceVector = startPosition - projectileTransform.position;
            
            if (_currentDistanceVector.sqrMagnitude >= 10000f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasCollided)
            {
                _hasCollided = true;
                
                if (collisionFx)
                {
                    collisionFx = Instantiate(collisionFx, transform.position, Quaternion.identity);
                    Destroy(collisionFx, 1.5f);
                }

                if (other.CompareTag(EnemyTag))
                {
                    other.GetComponent<EnemyStats>().TakeDamage(damage, false, false);
                }
                else if (other.CompareTag(DestructibleTag))
                {
                    Destroy(other.gameObject);
                }
                
                Destroy(gameObject, 0.25f);
            }
        }
    }
}