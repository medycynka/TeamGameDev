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
        [Header("Combo Actions", order = 2)]
        public bool canCombo;
        public EnemyAttackAction comboAction;
    }
}