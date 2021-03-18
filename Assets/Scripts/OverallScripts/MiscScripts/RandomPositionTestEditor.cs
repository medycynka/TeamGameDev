#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


namespace SzymonPeszek.Misc
{
    [CustomEditor(typeof(RandomPositionTester))]
    public class RandomPositionTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RandomPositionTester testerScript = (RandomPositionTester) target;

            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Place objects at random position"))
                {
                    //DateTime start = DateTime.Now;
                    testerScript.CreateObjectAtRandomPosition();
                    //DateTime stop = DateTime.Now;
                    
                    //Debug.Log("Took " + (stop - start).Milliseconds + " ms");
                }

                if (GUILayout.Button("Delete created objects"))
                {
                    testerScript.DeleteCreatedObjects();
                }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Save positions"))
            {
                testerScript.SavePointsToSpawner();
            }
        }
    }
}
#endif