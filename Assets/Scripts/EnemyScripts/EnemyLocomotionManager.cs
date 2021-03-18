using UnityEngine;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class managing enemy movement
    /// </summary>
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager _enemyManager;
        private EnemyAnimationManager _enemyAnimationManager;

        [Header("Locomotion Manager", order = 0)] 
        [Header("Components", order = 1)]
        public LayerMask detectionLayer;

        [Header("A.I Movement Stats", order = 1)]
        public float stoppingDistance = 1.25f;
        public float rotationSpeed = 100;

        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            _enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }
    }
}