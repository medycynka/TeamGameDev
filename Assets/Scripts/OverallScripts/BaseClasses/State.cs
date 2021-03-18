using UnityEngine;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class representing states for finite state machine
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// Use state behaviour
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        /// <param name="enemyStats">Enemy stats</param>
        /// <param name="enemyAnimationManager">Enemy animation manager</param>
        /// <returns>This or next state</returns>
        public abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager);
    }

}