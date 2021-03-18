using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattleDrakeStudios.ModularCharacters;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.PlayerScripts.Inventory
{
    /// <summary>
    /// Class for storing player current equipment
    /// </summary>
    public class CurrentEquipments : MonoBehaviour
    {
        private PlayerStats _playerStats;
        private ModularCharacterManager _modularCharacterManager;

        /// <summary>
        /// Helper class for quick managing player's equipment in equipment dictionary
        /// </summary>
        [Serializable]
        public class EquipmentPart
        {
            public int dictID { get; set; }
            public ModularBodyPart eqPart { get; set; }
            public int partID { get; set; }
            public float armorValue { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="did">Id in dictionary</param>
            /// <param name="mbp">Equipment represents by ModularBody part from FantasyModularCharacter pack</param>
            /// <param name="id">Part id</param>
            /// <param name="armor">Armor value</param>
            public EquipmentPart(int did, ModularBodyPart mbp, int id, float armor)
            {
                dictID = did;
                eqPart = mbp;
                partID = id;
                armorValue = armor;
            }
        }

        public Dictionary<ModularBodyPart, EquipmentPart> currentEq;

        private void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
            _modularCharacterManager = GetComponentInChildren<ModularCharacterManager>();
        }

        private void Start()
        {
            _modularCharacterManager.SwapGender(SettingsHolder.isMale ? Gender.Male : Gender.Female);

            InitializeCurrentEquipment();
            EquipPlayerWithCurrentItems();
            UpdateArmorValue();
        }

        /// <summary>
        /// Initialize current equipment
        /// </summary>
        public void InitializeCurrentEquipment()
        {
            currentEq = new Dictionary<ModularBodyPart, EquipmentPart>(){
                { ModularBodyPart.Helmet, new EquipmentPart(0, ModularBodyPart.Helmet, SettingsHolder.partsID[0], SettingsHolder.partsArmor[0]) },
                { ModularBodyPart.HeadAttachment, new EquipmentPart(1, ModularBodyPart.HeadAttachment, SettingsHolder.partsID[1], SettingsHolder.partsArmor[1]) },
                { ModularBodyPart.Head, new EquipmentPart(2, ModularBodyPart.Head, SettingsHolder.partsID[2], SettingsHolder.partsArmor[2]) },
                { ModularBodyPart.Hat, new EquipmentPart(3, ModularBodyPart.Hat, SettingsHolder.partsID[3], SettingsHolder.partsArmor[3]) },
                { ModularBodyPart.HeadCovering, new EquipmentPart(4, ModularBodyPart.HeadCovering, SettingsHolder.partsID[4], SettingsHolder.partsArmor[4]) },
                { ModularBodyPart.Hair, new EquipmentPart(5, ModularBodyPart.Hair, SettingsHolder.partsID[5], SettingsHolder.partsArmor[5]) },
                { ModularBodyPart.Eyebrow, new EquipmentPart(6, ModularBodyPart.Eyebrow, SettingsHolder.partsID[6], SettingsHolder.partsArmor[6]) },
                { ModularBodyPart.Ear, new EquipmentPart(7, ModularBodyPart.Ear, SettingsHolder.partsID[7], SettingsHolder.partsArmor[7]) },
                { ModularBodyPart.FacialHair, new EquipmentPart(8, ModularBodyPart.FacialHair, SettingsHolder.partsID[8], SettingsHolder.partsArmor[8]) },
                { ModularBodyPart.BackAttachment, new EquipmentPart(9, ModularBodyPart.BackAttachment, SettingsHolder.partsID[9], SettingsHolder.partsArmor[9]) },
                { ModularBodyPart.Torso, new EquipmentPart(10, ModularBodyPart.Torso, SettingsHolder.partsID[10], SettingsHolder.partsArmor[10]) },
                { ModularBodyPart.ShoulderAttachmentRight, new EquipmentPart(11, ModularBodyPart.ShoulderAttachmentRight, SettingsHolder.partsID[11], SettingsHolder.partsArmor[11]) },
                { ModularBodyPart.ShoulderAttachmentLeft, new EquipmentPart(12, ModularBodyPart.ShoulderAttachmentLeft, SettingsHolder.partsID[12], SettingsHolder.partsArmor[12]) },
                { ModularBodyPart.ArmUpperRight, new EquipmentPart(13, ModularBodyPart.ArmUpperRight, SettingsHolder.partsID[13], SettingsHolder.partsArmor[13]) },
                { ModularBodyPart.ArmUpperLeft, new EquipmentPart(14, ModularBodyPart.ArmUpperLeft, SettingsHolder.partsID[14], SettingsHolder.partsArmor[14]) },
                { ModularBodyPart.ElbowAttachmentRight, new EquipmentPart(15, ModularBodyPart.ElbowAttachmentRight, SettingsHolder.partsID[15], SettingsHolder.partsArmor[15]) },
                { ModularBodyPart.ElbowAttachmentLeft, new EquipmentPart(16, ModularBodyPart.ElbowAttachmentLeft, SettingsHolder.partsID[16], SettingsHolder.partsArmor[16]) },
                { ModularBodyPart.ArmLowerRight, new EquipmentPart(17, ModularBodyPart.ArmLowerRight, SettingsHolder.partsID[17], SettingsHolder.partsArmor[17]) },
                { ModularBodyPart.ArmLowerLeft, new EquipmentPart(18, ModularBodyPart.ArmLowerLeft, SettingsHolder.partsID[18], SettingsHolder.partsArmor[18]) },
                { ModularBodyPart.HandRight, new EquipmentPart(19, ModularBodyPart.HandRight, SettingsHolder.partsID[19], SettingsHolder.partsArmor[19]) },
                { ModularBodyPart.HandLeft, new EquipmentPart(20, ModularBodyPart.HandLeft, SettingsHolder.partsID[20], SettingsHolder.partsArmor[20]) },
                { ModularBodyPart.Hips, new EquipmentPart(21, ModularBodyPart.Hips, SettingsHolder.partsID[21], SettingsHolder.partsArmor[21]) },
                { ModularBodyPart.KneeAttachmentRight, new EquipmentPart(22, ModularBodyPart.KneeAttachmentRight, SettingsHolder.partsID[22], SettingsHolder.partsArmor[22]) },
                { ModularBodyPart.KneeAttachmentLeft, new EquipmentPart(23, ModularBodyPart.KneeAttachmentLeft, SettingsHolder.partsID[23], SettingsHolder.partsArmor[23]) },
                { ModularBodyPart.LegRight, new EquipmentPart(24, ModularBodyPart.LegRight, SettingsHolder.partsID[24], SettingsHolder.partsArmor[24]) },
                { ModularBodyPart.LegLeft, new EquipmentPart(25, ModularBodyPart.LegLeft, SettingsHolder.partsID[25], SettingsHolder.partsArmor[25]) }
            };
        }

        /// <summary>
        /// Equip player with current equipments
        /// </summary>
        public void EquipPlayerWithCurrentItems()
        {
            foreach(var kvp in currentEq)
            {
                if(kvp.Value.partID > -1)
                {
                    _modularCharacterManager.ActivatePart(kvp.Value.eqPart, kvp.Value.partID);
                }
            }
        }

        /// <summary>
        /// Save current equipment
        /// </summary>
        public void SaveCurrentEqIds()
        {
            foreach (var kvp in currentEq)
            {
                SettingsHolder.partsID[kvp.Value.dictID] = kvp.Value.partID;
                SettingsHolder.partsArmor[kvp.Value.dictID] = kvp.Value.armorValue;
            }
        }

        /// <summary>
        /// Updates player's current armor value
        /// </summary>
        public void UpdateArmorValue()
        {
            _playerStats.currentArmorValue = _playerStats.baseArmor + CalculateArmorOfCurrentEquipment() + 2.5f * _playerStats.defence + 0.5f * _playerStats.agility;
        }

        /// <summary>
        /// Calculate cumulative armor from current equipments
        /// </summary>
        /// <returns>Cumulative armor value</returns>
        private float CalculateArmorOfCurrentEquipment()
        {
            return currentEq.Sum(kvp => kvp.Value.armorValue);
        }
    }

}