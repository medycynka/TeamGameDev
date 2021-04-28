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
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileScript : SpellItem
    {
        public GameObject fireballPrefab;
        public float spellDamage;
        public float projectileSpeed;
        public float maxTravelDistanceSqr = 10000f;

        private const string EnvironmentName = "Environment";
        private const string EnemyName = "Enemy";
        private RaycastHit _hit;
        private SpellCollision _spellCollision;
        
        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="playerAnimatorManager">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
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

            Vector3 rayOrigin = playerStats.mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(rayOrigin, playerStats.mainCamera.transform.forward, out _hit, 100f, playerAnimatorManager.spellRayCastLayer))
            {
                Transform fireball = Instantiate(fireballPrefab,
                    playerAnimatorManager.spellProjectilesTransform.position,
                    playerAnimatorManager.spellProjectilesTransform.rotation).transform;
                fireball.LookAt(playerStats.IsLockOn() ? _hit.point + Vector3.up : _hit.point);
                _spellCollision = fireball.GetComponent<SpellCollision>();
                _spellCollision.startPosition = fireball.position;
                _spellCollision.projectileTransform = fireball;
                _spellCollision.damage = spellDamage;
                _spellCollision.maxTravelDistanceSqr = maxTravelDistanceSqr;
                fireball.GetComponent<Rigidbody>().AddForce(fireball.forward * projectileSpeed);
            }
            else
            {
                Transform fireball = Instantiate(fireballPrefab,
                    playerAnimatorManager.spellProjectilesTransform.position,
                    playerAnimatorManager.spellProjectilesTransform.rotation).transform;
                fireball.LookAt(rayOrigin + (playerStats.mainCamera.transform.forward * 100f));
                _spellCollision = fireball.GetComponent<SpellCollision>();
                _spellCollision.startPosition = fireball.position;
                _spellCollision.projectileTransform = fireball;
                _spellCollision.damage = spellDamage;
                _spellCollision.maxTravelDistanceSqr = maxTravelDistanceSqr;
                fireball.GetComponent<Rigidbody>().AddForce(fireball.forward * projectileSpeed);
            }
        }
    }
}