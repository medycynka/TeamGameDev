using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for character (player or enemy) manager
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        [Header("Manager", order = 0)]
        [Header("Lock-on", order = 1)]
        public Transform lockOnTransform;
        
        [Header("Combat Colliders", order = 1)]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;
        
        [Header("Combat Flags", order = 1)]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isParrying;
        public bool isBlocking;

        [Header("Combat Colliders", order = 1)]
        public Transform characterTransform;
        
        [Header("Critical Damage for Animations", order = 1)]
        public float pendingCriticalDamage;
    }
}