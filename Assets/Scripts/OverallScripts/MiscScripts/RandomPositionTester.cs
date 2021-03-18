using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Environment.Areas;
using UnityEngine;


namespace SzymonPeszek.Misc
{
    /// <summary>
    /// Class for placing objects at random position
    /// </summary>
    public class RandomPositionTester : MonoBehaviour
    {
        public Bounds bounds;
        public GameObject testPrefab;
        public bool useBounds;
        [Range(1, 25000)] public int objectsAmount = 1;
        public Terrain worldTerrain;
        public LayerMask terrainLayer;
        private Vector3 _centerPoint;
        private List<GameObject> _createdObjects = new List<GameObject>();
        public Vector3[] points;
        public EnemySpawner enemySpawner;

        public void CreateObjectAtRandomPosition()
        {
            points = useBounds
                ? ExtensionMethods.GetRandomPointsOnSurfaceInBoundsNonRefFast(bounds, terrainLayer, objectsAmount)
                : ExtensionMethods.GetRandomPointsOnSurfaceNonRef(worldTerrain.transform.position,
                    worldTerrain.terrainData.size, terrainLayer, objectsAmount);

            for (int i = 0; i < objectsAmount; i++)
            {
                _createdObjects.Add(Instantiate(testPrefab, points[i], Quaternion.identity));
            }
        }

        public void DeleteCreatedObjects()
        {
            if (_createdObjects.Count > 0)
            {
                for (int i = 0; i < _createdObjects.Count; i++)
                {
                    DestroyImmediate(_createdObjects[i]);
                }

                _createdObjects.Clear();
            }
        }

        public void SavePointsToSpawner()
        {
            if (enemySpawner != null)
            {
                enemySpawner.positionsList = new List<Vector3>();

                for (int i = 0; i < _createdObjects.Count; i++)
                {
                    enemySpawner.positionsList.Add(_createdObjects[i].transform.position);
                }
            }
        }
    }
}