using UnityEngine;
using SzymonPeszek.Damage;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class for managing enemy's weapon collider
    /// </summary>
    public class EnemyWeaponColliderManager : MonoBehaviour
    {
        [Header("Weapon Collider Manager", order = 0)]
        [Header("Collider", order = 1)]
        public DamageCollider damageCollider;

        private void Awake()
        {
            damageCollider = GetComponentInChildren<DamageCollider>();
        }

        /// <summary>
        /// Open collider on attack
        /// </summary>
        public void OpenDamageCollider()
        {
            damageCollider.EnableDamageCollider();
        }

        /// <summary>
        /// Close collider
        /// </summary>
        public void CloseDamageCollider()
        {
            damageCollider.DisableDamageCollider();
        }
    }
}