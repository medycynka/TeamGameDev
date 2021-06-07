#if UNITY_EDITOR
using System.Collections.Generic;
using SzymonPeszek.Items.Equipment;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


namespace SzymonPeszek.Misc
{
    public class ArmorDefenceRandomizer : MonoBehaviour
    {
        [Header("Helmets")]
        public List<EquipmentItem> helmets;
        [Min(1)] public float minHelmetDefence = 1;
        [Min(5)] public float maxHelmetDefence = 5;
        
        [Header("Chests")]
        public List<EquipmentItem> chests;
        [Min(1)] public float minChestDefence = 1;
        [Min(10)] public float maxChestDefence = 10;
        
        [Header("Shoulders")]
        public List<EquipmentItem> shoulders;
        [Min(1)] public float minShoulderDefence = 1;
        [Min(5)] public float maxShoulderDefence = 1;
        
        [Header("Arms")]
        public List<EquipmentItem> arms;
        [Min(1)] public float minArmDefence = 1;
        [Min(5)] public float maxArmDefence = 5;
        
        [Header("Legs")]
        public List<EquipmentItem> legs;
        [Min(1)] public float minLegDefence = 1;
        [Min(7.5f)] public float maxLegDefence = 7.5f;
        
        [Header("Feet")]
        public List<EquipmentItem> feet;
        [Min(1)] public float minFootDefence = 1;
        [Min(5)] public float maxFootDefence = 5;

        public void RandomizeArmorValues()
        {
            foreach (var helmet in helmets)
            {
                helmet.armor = Mathf.RoundToInt(Random.Range(minHelmetDefence, maxHelmetDefence));
                EditorUtility.SetDirty(helmet);
            }
            
            foreach (var chest in chests)
            {
                chest.armor = Mathf.RoundToInt(Random.Range(minChestDefence, maxChestDefence));
                EditorUtility.SetDirty(chest);
            }
            
            foreach (var shoulder in shoulders)
            {
                shoulder.armor = Mathf.RoundToInt(Random.Range(minShoulderDefence, maxShoulderDefence));
                EditorUtility.SetDirty(shoulder);
            }
            
            foreach (var arm in arms)
            {
                arm.armor = Mathf.RoundToInt(Random.Range(minArmDefence, maxArmDefence));
                EditorUtility.SetDirty(arm);
            }
            
            foreach (var leg in legs)
            {
                leg.armor = Mathf.RoundToInt(Random.Range(minLegDefence, maxLegDefence));
                EditorUtility.SetDirty(leg);
            }
            
            foreach (var foot in feet)
            {
                foot.armor = Mathf.RoundToInt(Random.Range(minFootDefence, maxFootDefence));
                EditorUtility.SetDirty(foot);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif