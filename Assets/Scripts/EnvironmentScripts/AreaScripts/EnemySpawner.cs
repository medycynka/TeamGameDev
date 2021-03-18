using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Environment.Areas
{
    /// <summary>
    /// Class for enemies spawning
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public Transform parentTransform;
        public List<GameObject> prefabsList;
        public List<Vector3> positionsList;

        private List<GameObject> _enemiesAlive;

        private void Start()
        {
            _enemiesAlive = new List<GameObject>();

            InitializeSpawnerList();    
        }

        /// <summary>
        /// Spawn enemies at given positions and random rotations
        /// </summary>
        public void SpawnEnemies()
        {
            StartCoroutine(RefreshSpawner());
        }

        /// <summary>
        /// Initialize spawning list
        /// </summary>
        private void InitializeSpawnerList()
        {
            for (int i = 0; i < prefabsList.Count; i++)
            {
                _enemiesAlive.Add(Instantiate(prefabsList[i], positionsList[i], Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), parentTransform));
            }
        }

        /// <summary>
        /// Coroutine for respawning enemies
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator RefreshSpawner()
        {
            yield return CoroutineYielder.waitFor1Second;

            for (int i = 0; i < prefabsList.Count; i++)
            {
                if (_enemiesAlive[i] == null)
                {
                    _enemiesAlive[i] = Instantiate(prefabsList[i], positionsList[i],
                        Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), parentTransform);
                }
            }
        }
    }
}