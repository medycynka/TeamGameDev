using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;
using SzymonPeszek.Misc;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace SzymonPeszek.EnemyScripts.States
{
    public class PatrolState : State
    {
        [Header("Patrol State", order = 0)]
        [Header("Possible After States", order = 1)]
        public PursueTargetState pursueTargetState;
        public DeathState deathState;

        [Header("Player Detection Layer", order = 1)]
        public LayerMask detectionLayer;

        private Collider[] detectPlayer = new Collider[2];
        private Vector3 _patrolPoint = Vector3.zero;
        private NavMeshHit _hit;
        
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                LookForPlayer(enemyManager);

                if (enemyManager.currentTarget != null)
                {
                    enemyManager.shouldFollowTarget = true;
                    
                    return pursueTargetState;
                }

                if (_patrolPoint == Vector3.zero)
                {
                    _patrolPoint = GetPatrolPoint(enemyManager, enemyManager.characterTransform.position);
                }

                if (_patrolPoint != Vector3.zero)
                {
                    float distanceFromTarget = Vector3.Distance(_patrolPoint, enemyManager.characterTransform.position);
                    
                    if (distanceFromTarget > 1f)
                    {
                        enemyAnimationManager.anim.SetFloat(
                            StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.VerticalName], 0.5f, 0.1f,
                            Time.deltaTime);

                        HandleRotateTowardsTarget(enemyManager);
                    }
                    else
                    {
                        _patrolPoint = Vector3.zero;

                        return this;
                    }
                }

                return this;
            }

            return deathState;
        }
        
        /// <summary>
        /// Check if player is in detection range
        /// </summary>
        /// <param name="enemyManager"></param>
        private void LookForPlayer(EnemyManager enemyManager)
        {
            int detectLength = Physics.OverlapSphereNonAlloc(enemyManager.characterTransform.position, enemyManager.detectionRadius, detectPlayer, detectionLayer);

            for (int i = 0; i < detectLength; i++)
            {
                CharacterStats characterStats = detectPlayer[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.characterTransform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.characterTransform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
        }
        
        /// <summary>
        /// Get new patrolling position on NavMesh
        /// </summary>
        /// <param name="enemyManager"></param>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        private Vector3 GetPatrolPoint(EnemyManager enemyManager, Vector3 currentPosition)
        {
            float randomRange = enemyManager.maxRandomPatrolDistance;
            
            for (int i = 0; i < enemyManager.maxRandomPointSearchIterations; i++)
            {
                Vector3 randomPatrolPoint = new Vector3(currentPosition.x + Random.Range(-randomRange, randomRange),
                    currentPosition.y + Random.Range(-1f, 1f),
                    currentPosition.z + Random.Range(-randomRange, randomRange));

                if (NavMesh.SamplePosition(randomPatrolPoint, out _hit, 1f, NavMesh.AllAreas))
                {
                    return _hit.position;
                }
            }
            
            return Vector3.zero;
        }
        
        /// <summary>
        /// Helper function for rotating character towards player
        /// </summary>
        /// <param name="enemyManager">Enemy manager</param>
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPreformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.characterTransform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = enemyManager.characterTransform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.characterTransform.rotation = Quaternion.Lerp(enemyManager.characterTransform.rotation,
                    targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh) -> Change to A*
            else
            {
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(_patrolPoint);
                enemyManager.enemyRigidBody.velocity = targetVelocity;
                enemyManager.characterTransform.rotation = Quaternion.Lerp(enemyManager.characterTransform.rotation,
                    enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}