using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;


namespace SzymonPeszek.Items.Weapons
{
    /// <summary>
    /// Class representing weapon item
    /// </summary>
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        [Header("Weapon Item", order = 1)]
        [Header("Weapon Prefab", order = 2)]
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Transformations for Left Hand", order = 2)]
        public Vector3 pivotPositionTransform;
        public Vector3 pivotRotationTransform;
        
        [Header("Transformations for Back Slot", order = 2)]
        public Vector3 backSlotPosition;
        public Vector3 backSlotRotation;
        public float backSlotScale = 1.0f;

        [Header("Weapon Stats", order = 2)] 
        public float baseAttack = 25f;
        public float lightAttackDamageMult = 1.0f;
        public float heavyAttackDamageMult = 1.5f;
        public float criticalDamageMult = 2.5f;
        public int weight;
        public int durability;

        [Header("Weapon Idle Animations", order = 2)]
        public string rightHandIdle;
        public string leftHandIdle;
        public string thIdle;

        [Header("One Handed Attack Animations", order = 2)]
        public string ohLightAttack1 = "OH_Light_Attack_01";
        public string ohLightAttack2 = "OH_Light_Attack_02";
        public string ohLightAttack3 = "OH_Light_Attack_03";
        public string ohHeavyAttack1 = "OH_Heavy_Attack_01";
        public string ohHeavyAttack2 = "OH_Heavy_Attack_02";
        public string thLightAttack1 = "TH_Light_Attack_01";
        public string thLightAttack2 = "TH_Light_Attack_02";
        public string thLightAttack3 = "TH_Light_Attack_03";
        public string thHeavyAttack1 = "TH_Heavy_Attack_01";
        public string thHeavyAttack2 = "TH_Heavy_Attack_02";
        
        [Header("Weapon Art")]
        public string weaponArt = "Parry";

        [Header("Stamina Costs", order = 2)]
        public int baseStamina;
        public float ohLightAttackMultiplier;
        public float ohHeavyAttackMultiplier;
        public float thLightAttackMultiplier;
        public float thHeavyAttackMultiplier;

        [Header("Weapon Specifier", order = 2)]
        public WeaponType weaponType;
        public MeleeType meleeType = MeleeType.NotMelee;
        public CastingType castingType = CastingType.NotCasting;
        public ShootingType shootingType = ShootingType.NotShooting;
    }

}
