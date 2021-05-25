using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using UnityEngine;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class representing enemy's magic attack action
    /// </summary>
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Magic Action")]
    public class EnemyMagicAction : EnemyAction
    {
        [Header("Magic properties", order = 2)]
        public SpellItem spell;
    }
}