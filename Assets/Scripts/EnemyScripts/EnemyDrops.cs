using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Items.Consumable;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Items.Equipment;
using SzymonPeszek.Items;


namespace SzymonPeszek.EnemyScripts
{
    /// <summary>
    /// Class managing enemy's drop items and corresponding drop chances
    /// </summary>
    public class EnemyDrops : MonoBehaviour
    {
        [Header("Death Drop", order = 0)]
        [Header("Drop chances", order = 1)]
        public float consumableChance = 0.45f;
        public float weaponChance = 0.25f;
        public float equipmentChance = 0.2f;

        [Header("Drops pool", order = 1)]
        public ConsumableItem[] consumableDropPool;
        public WeaponItem[] weaponDropPool;
        public EquipmentItem[] equipmentDropPool;

        [SerializeField] private List<Item> dropList;
        [SerializeField] private MiscPickUp deathDrop;
        [SerializeField] private float dropChance;

        private void Start()
        {
            dropList = new List<Item>();
            dropChance = Random.Range(0.0f, 1.0f);
            deathDrop.items.Clear();

            if (dropChance < consumableChance)
            {
                // Drop consumable
                foreach (var drop in consumableDropPool)
                {
                    dropList.Add(drop);
                }
            }
            else if (dropChance - consumableChance < weaponChance)
            {
                // Drop weapon
                foreach (var drop in weaponDropPool)
                {
                    dropList.Add(drop);
                }
            }
            else if (dropChance - consumableChance - weaponChance < equipmentChance)
            {
                // Drop equipment
                foreach (var drop in equipmentDropPool)
                {
                    dropList.Add(drop);
                }
            }
            else
            {
                // Drop all
                foreach (var drop in consumableDropPool)
                {
                    dropList.Add(drop);
                }

                foreach (var drop in weaponDropPool)
                {
                    dropList.Add(drop);
                }

                foreach (var drop in equipmentDropPool)
                {
                    dropList.Add(drop);
                }
            }
        }

        public void DropPickUp()
        {
            deathDrop.items = dropList;
            Instantiate(deathDrop, transform.position, Quaternion.identity);
            deathDrop.items.Clear();
        }
    }
}