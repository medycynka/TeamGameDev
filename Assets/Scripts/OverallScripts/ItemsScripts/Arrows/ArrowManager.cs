using System;
using System.Collections;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts;
using UnityEngine;


namespace SzymonPeszek.Items.Arrows
{
    public class ArrowManager : MonoBehaviour
    {
        public Rigidbody rb;
        public float minArrowSpeed = 5f;
        public float maxArrowSpeed = 25f;
        public float arrowDamage = 20f;
        public float lifeTime = 5f;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Transform arrowTransform;
        public float maxTravelDistanceSqr = 10000f;
        [HideInInspector] public Transform cameraTransform;
        [HideInInspector] public PlayerStats playerStats;

        private Vector3 _currentDistanceVector;
        private bool _hasCollided;
        private bool _isFired;
        private const string EnemyTag = "Enemy";
        private const string BossTag = "Boss";

        private void Update()
        {
            if (!_hasCollided && _isFired)
            {
                _currentDistanceVector = startPosition - arrowTransform.position;

                if (_currentDistanceVector.sqrMagnitude >= maxTravelDistanceSqr)
                {
                    Debug.Log("Arrow too far");
                    Destroy(gameObject);
                }
                
                arrowTransform.rotation = Quaternion.LookRotation(rb.velocity);
            }

            if (!_hasCollided && !_isFired)
            {
                arrowTransform.rotation = cameraTransform.rotation;
            }
        }
        
        public void Fire(float bowStretchTime)
        {
            startPosition = cameraTransform.position;
            arrowTransform.parent = null;
            
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = cameraTransform.forward *
                          Mathf.Lerp(minArrowSpeed, maxArrowSpeed, bowStretchTime.Remap(0f, 3f, 0f, 1f));
            
            _isFired = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_hasCollided)
            {
                Debug.Log("Arrow collided with " + other.collider.name);
                arrowTransform.parent = other.transform;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                _hasCollided = true;

                if (other.collider.CompareTag(EnemyTag) || other.collider.CompareTag(BossTag))
                {
                    other.gameObject.GetComponent<EnemyStats>().TakeDamage(arrowDamage);
                }

                if (other.collider.CompareTag("Passive"))
                {
                    PassiveEnemyStats passiveEnemyStats = other.gameObject.GetComponentInParent<PassiveEnemyStats>();

                    if (passiveEnemyStats != null && playerStats != null)
                    {
                        passiveEnemyStats.TakeDamage(playerStats.CalculateDamage(arrowDamage, true), playerStats);
                    }
                }

                Destroy(gameObject, lifeTime);
            }
        }
    }
}