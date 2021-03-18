using UnityEngine;
using SzymonPeszek.BaseClasses;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class representing enemy's attack action
    /// </summary>
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
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
        
        [Header("Combo Actions", order = 2)]
        public bool canCombo;
        public EnemyAttackAction comboAction;
    }
}