using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Spells.Helpers;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Items.Spells
{
    /// <summary>
    /// Class representing fire ball spell
    /// </summary>
    [CreateAssetMenu(menuName = "Spells/Fireball Spell")]
    public class FireballScript : SpellItem
    {
        public GameObject fireballPrefab;
        public float spellDamage;
        public float projectileSpeed;
        public float maxTravelDistanceSqr = 10000f;

        private const string EnvironmentName = "Environment";
        private const string EnemyName = "Enemy";
        private LayerMask _raycastDetectionLayer;
        private Camera _mainCamera;
        private RaycastHit _hit;
        private SpellCollision _spellCollision;
        
        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(playerAnimatorManager, playerStats);

            _raycastDetectionLayer = (1 << LayerMask.NameToLayer(EnvironmentName) | 1 << LayerMask.NameToLayer(EnemyName));
            _mainCamera = Camera.main;
            
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[spellAnimation], true);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.spellProjectilesTransform);
        }

        /// <summary>
        /// Successfully cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimatorManager, playerStats);

            Vector3 rayOrigin = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(rayOrigin, _mainCamera.transform.forward, out _hit, 100f, _raycastDetectionLayer))
            {
                GameObject fireball = Instantiate(fireballPrefab, playerAnimatorManager.spellProjectilesTransform.position, playerAnimatorManager.spellProjectilesTransform.rotation);
                fireball.transform.LookAt(_hit.point);
                _spellCollision = fireball.GetComponent<SpellCollision>();
                _spellCollision.startPosition = fireball.transform.position;
                _spellCollision.projectileTransform = fireball.transform;
                _spellCollision.damage = spellDamage;
                _spellCollision.maxTravelDistanceSqr = maxTravelDistanceSqr;
                fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * projectileSpeed);
            }
            else
            {
                GameObject fireball = Instantiate(fireballPrefab, playerAnimatorManager.spellProjectilesTransform.position, playerAnimatorManager.spellProjectilesTransform.rotation);
                fireball.transform.LookAt(rayOrigin + (_mainCamera.transform.forward * 100f));
                _spellCollision = fireball.GetComponent<SpellCollision>();
                _spellCollision.startPosition = fireball.transform.position;
                _spellCollision.projectileTransform = fireball.transform;
                _spellCollision.damage = spellDamage;
                _spellCollision.maxTravelDistanceSqr = maxTravelDistanceSqr;
                fireball.GetComponent<Rigidbody>().AddForce(fireball.transform.forward * projectileSpeed);
            }
        }
    }
}