using System.Collections.Generic;

namespace SzymonPeszek.Misc
{
    /// <summary>
    /// Class for storing animations names and their hash codes
    /// </summary>
    public static class StaticAnimatorIds
    {
        #region Animation names
        #region Shared Animations
        public const string VerticalName = "Vertical";
        public const string HorizontalName = "Horizontal";
        public const string IsInteractingName = "isInteracting";
        public const string CanDoComboName = "canDoCombo";
        public const string IsInAirName = "isInAir";
        public const string IsUsingRightHandName = "isUsingRightHand";
        public const string IsUsingLeftHandName = "isUsingLeftHand";
        public const string IsInvulnerableName = "isInvulnerable";
        public const string IsDeadName = "isDead";
        public const string IsBlockingName = "isBlocking";
        public const string CanRotateName = "canRotate";
        public const string EmptyName = "Empty";
        public const string LeftArmEmptyName = "Left Arm Empty";
        public const string RightArmEmptyName = "Right Arm Empty";
        public const string BothArmsEmptyName = "Both Arms Empty";
        public const string LeftArmIdleName = "Left_Arm_Idle_01";
        public const string BlockIdleName = "Block";
        public const string RightArmIdleName = "Right_Arm_Idle_01";
        public const string BothArmsIdleName = "TH_Idle_01";
        public const string StandUpName = "Stand Up";
        public const string SitName = "Sit Down";
        public const string PickUpName = "Pick_Up_Item";
        public const string EstusName = "Use Estus";
        public const string UseItemName = "Use Item";
        public const string RollName = "Rolling";
        public const string BackStepName = "Backstep";
        public const string JumpName = "Jump";
        public const string FallName = "Falling";
        public const string LandName = "Land";
        public const string Damage01Name = "Damage_01";
        public const string Death01Name = "Dead_01";
        public const string GetUpName = "Get Up";
        public const string SleepName = "Sleep";
        public const string FogRemoveName = "Remove Fog Wall";
        public const string BackStabName = "Back Stab";
        public const string BackStabbedName = "Back Stabbed";
        public const string LayDownName = "Laying Down";
        public const string LayDown2Name = "Laying Down 2";
        public const string RiposteName = "Riposte";
        public const string RipostedName = "Riposted";
        public const string ParryName = "Parry";
        public const string ParriedName = "Parried";
        public const string ChestOpeningName = "Open Chest";
        #endregion

        #region Player Attacks
        public const string OhLightAttack01 = "OH_Light_Attack_01";
        public const string OhLightAttack02 = "OH_Light_Attack_02";
        public const string OhLightAttack03 = "OH_Light_Attack_03";
        public const string OhLightAttack04 = "OH_Light_Attack_04";
        public const string OhLightAttack05 = "OH_Light_Attack_05";
        public const string OhLightAttack06 = "OH_Light_Attack_06";
        public const string OhHeavyAttack01 = "OH_Heavy_Attack_01";
        public const string OhHeavyAttack02 = "OH_Heavy_Attack_02";
        public const string OhHeavyAttack03 = "OH_Heavy_Attack_03";
        public const string OhHeavyAttack04 = "OH_Heavy_Attack_04";
        public const string OhHeavyAttack05 = "OH_Heavy_Attack_05";
        public const string ThLightAttack01 = "TH_Light_Attack_01";
        public const string ThLightAttack02 = "TH_Light_Attack_02";
        public const string ThLightAttack03 = "TH_Light_Attack_03";
        public const string ThHeavyAttack01 = "TH_Heavy_Attack_01";
        public const string ThHeavyAttack02 = "TH_Heavy_Attack_02";
        public const string LightPunch01 = "Light_Punch_01";
        public const string HeavyPunch01 = "Heavy_Punch_01";
        public const string OhCombo01 = "OH_Combo_01";
        public const string OhCombo02 = "OH_Combo_02";
        public const string OhHeavyCombo01 = "OH_Heavy_Combo_01";
        public const string HealSpell = "Heal Spell";
        public const string FireballSpell = "Fireball Cast";
        #endregion
        
        #region Enemy Attacks
        public const string EnemyMaceAttack01 = "OH_Mace_Attack_01";
        public const string EnemyMaceAttack02 = "OH_Mace_Attack_02";
        public const string EnemyMaceAttack03 = "OH_Mace_Attack_03";
        public const string EnemyStaffAttack01 = "Staff_Attack_01";
        public const string EnemyStaffAttack02 = "Staff_Attack_02";
        public const string EnemySpearAttack01 = "Spear_Attack_01";
        public const string EnemySpearAttack02 = "Spear_Attack_02";
        public const string EnemySwordAttack01 = "OH_Sword_Attack_01";
        public const string EnemySwordAttack02 = "OH_Sword_Attack_02";
        public const string EnemySwordAttack03 = "OH_Sword_Attack_03";
        public const string EnemySwordAttack04 = "OH_Sword_Attack_04";
        public const string EnemySwordAttack05 = "OH_Sword_Attack_05";
        public const string EnemySwordAttack06 = "OH_Sword_Attack_06";
        #endregion

        #region Items Animations
        public const string ChestOpenName = "Chest Opening";
        #endregion
        #endregion
        
        #region Player Animation Ids
        public static Dictionary<string, int> animationIds;
        #endregion
        
        #region Enemy Animation Ids
        public static Dictionary<string, int> enemyAnimationIds;
        #endregion

        #region Interactable Items Animations Id
        public static int chestOpenId;
        #endregion
    }
}