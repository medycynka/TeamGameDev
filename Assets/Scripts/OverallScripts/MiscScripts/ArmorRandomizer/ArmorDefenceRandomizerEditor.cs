#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


namespace SzymonPeszek.Misc
{
    [CustomEditor(typeof(ArmorDefenceRandomizer))]
    public class ArmorDefenceRandomizerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ArmorDefenceRandomizer targetScript = (ArmorDefenceRandomizer) target;

            if (GUILayout.Button("Randomize armor"))
            {
                DateTime start = DateTime.Now;
                targetScript.RandomizeArmorValues();
                DateTime stop = DateTime.Now;
                    
                Debug.Log("Took " + (stop - start).Milliseconds + " ms");
            }
        }
    }
}
#endif