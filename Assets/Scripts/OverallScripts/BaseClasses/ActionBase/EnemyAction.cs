using UnityEngine;

namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for enemy action, like attacking
    /// </summary>
    public class EnemyAction : ScriptableObject
    {
        [Header("Attack Action", order = 0)]
        [Header("Attack Animation Name", order = 1)]
        public string actionAnimation;
        
        [Header("Attack Properties", order = 1)]
        [Header("Attack Score", order = 2)]
        public int attackScore = 3;

        [Header("Recovery Time", order = 2)]
        public float recoveryTime = 2;

        [Header("Attack Angle", order = 2)]
        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        [Header("Attack Distance", order = 2)]
        public float minimumDistanceNeededToAttack = 0.5f;
        public float maximumDistanceNeededToAttack = 3;
    }
}