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
    }
}