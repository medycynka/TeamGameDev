using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;


namespace SzymonPeszek.PlayerScripts.Animations
{
    /// <summary>
    /// Class for managing player's animations
    /// </summary>
    public class PlayerAnimatorManager : AnimationManager
    {
        private PlayerManager _playerManager;
        private PlayerLocomotion _playerLocomotion;
        private PlayerStats _playerStats;

        public Transform spellProjectilesTransform;
        
        /// <summary>
        /// Initialize fields and Animator's hash values of all animations
        /// </summary>
        public void Initialize()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            _playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            _playerStats = GetComponentInParent<PlayerStats>();

            StaticAnimatorIds.animationIds = new Dictionary<string, int>
            {
                {StaticAnimatorIds.VerticalName, Animator.StringToHash(StaticAnimatorIds.VerticalName)},
                {StaticAnimatorIds.HorizontalName, Animator.StringToHash(StaticAnimatorIds.HorizontalName)},
                {StaticAnimatorIds.IsInteractingName, Animator.StringToHash(StaticAnimatorIds.IsInteractingName)},
                {StaticAnimatorIds.CanDoComboName, Animator.StringToHash(StaticAnimatorIds.CanDoComboName)},
                {StaticAnimatorIds.IsInvulnerableName, Animator.StringToHash(StaticAnimatorIds.IsInvulnerableName)},
                {StaticAnimatorIds.IsDeadName, Animator.StringToHash(StaticAnimatorIds.IsDeadName)},
                {StaticAnimatorIds.IsInAirName, Animator.StringToHash(StaticAnimatorIds.IsInAirName)},
                {StaticAnimatorIds.CanRotateName, Animator.StringToHash(StaticAnimatorIds.CanRotateName)},
                {StaticAnimatorIds.IsUsingLeftHandName, Animator.StringToHash(StaticAnimatorIds.IsUsingLeftHandName)},
                {StaticAnimatorIds.IsUsingRightHandName, Animator.StringToHash(StaticAnimatorIds.IsUsingRightHandName)},
                {StaticAnimatorIds.IsBlockingName, Animator.StringToHash(StaticAnimatorIds.IsBlockingName)},
                {StaticAnimatorIds.EmptyName, Animator.StringToHash(StaticAnimatorIds.EmptyName)},
                {StaticAnimatorIds.LeftArmEmptyName, Animator.StringToHash(StaticAnimatorIds.LeftArmEmptyName)},
                {StaticAnimatorIds.RightArmEmptyName, Animator.StringToHash(StaticAnimatorIds.RightArmEmptyName)},
                {StaticAnimatorIds.BothArmsEmptyName, Animator.StringToHash(StaticAnimatorIds.BothArmsEmptyName)},
                {StaticAnimatorIds.LeftArmIdleName, Animator.StringToHash(StaticAnimatorIds.LeftArmIdleName)},
                {StaticAnimatorIds.BlockIdleName, Animator.StringToHash(StaticAnimatorIds.BlockIdleName)},
                {StaticAnimatorIds.RightArmIdleName, Animator.StringToHash(StaticAnimatorIds.RightArmIdleName)},
                {StaticAnimatorIds.BothArmsIdleName, Animator.StringToHash(StaticAnimatorIds.BothArmsIdleName)},
                {StaticAnimatorIds.StandUpName, Animator.StringToHash(StaticAnimatorIds.StandUpName)},
                {StaticAnimatorIds.SitName, Animator.StringToHash(StaticAnimatorIds.SitName)},
                {StaticAnimatorIds.PickUpName, Animator.StringToHash(StaticAnimatorIds.PickUpName)},
                {StaticAnimatorIds.EstusName, Animator.StringToHash(StaticAnimatorIds.EstusName)},
                {StaticAnimatorIds.UseItemName, Animator.StringToHash(StaticAnimatorIds.UseItemName)},
                {StaticAnimatorIds.RollName, Animator.StringToHash(StaticAnimatorIds.RollName)},
                {StaticAnimatorIds.BackStepName, Animator.StringToHash(StaticAnimatorIds.BackStepName)},
                {StaticAnimatorIds.JumpName, Animator.StringToHash(StaticAnimatorIds.JumpName)},
                {StaticAnimatorIds.FallName, Animator.StringToHash(StaticAnimatorIds.FallName)},
                {StaticAnimatorIds.LandName, Animator.StringToHash(StaticAnimatorIds.LandName)},
                {StaticAnimatorIds.Damage01Name, Animator.StringToHash(StaticAnimatorIds.Damage01Name)},
                {StaticAnimatorIds.Death01Name, Animator.StringToHash(StaticAnimatorIds.Death01Name)},
                {StaticAnimatorIds.FogRemoveName, Animator.StringToHash(StaticAnimatorIds.FogRemoveName)},
                {StaticAnimatorIds.BackStabName, Animator.StringToHash(StaticAnimatorIds.BackStabName)},
                {StaticAnimatorIds.BackStabbedName, Animator.StringToHash(StaticAnimatorIds.BackStabbedName)},
                {StaticAnimatorIds.OhLightAttack01, Animator.StringToHash(StaticAnimatorIds.OhLightAttack01)},
                {StaticAnimatorIds.OhLightAttack02, Animator.StringToHash(StaticAnimatorIds.OhLightAttack02)},
                {StaticAnimatorIds.OhLightAttack03, Animator.StringToHash(StaticAnimatorIds.OhLightAttack03)},
                {StaticAnimatorIds.OhLightAttack04, Animator.StringToHash(StaticAnimatorIds.OhLightAttack04)},
                {StaticAnimatorIds.OhLightAttack05, Animator.StringToHash(StaticAnimatorIds.OhLightAttack05)},
                {StaticAnimatorIds.OhLightAttack06, Animator.StringToHash(StaticAnimatorIds.OhLightAttack06)},
                {StaticAnimatorIds.OhHeavyAttack01, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack01)},
                {StaticAnimatorIds.OhHeavyAttack02, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack02)},
                {StaticAnimatorIds.OhHeavyAttack03, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack03)},
                {StaticAnimatorIds.OhHeavyAttack04, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack04)},
                {StaticAnimatorIds.OhHeavyAttack05, Animator.StringToHash(StaticAnimatorIds.OhHeavyAttack05)},
                {StaticAnimatorIds.ThLightAttack01, Animator.StringToHash(StaticAnimatorIds.ThLightAttack01)},
                {StaticAnimatorIds.ThLightAttack02, Animator.StringToHash(StaticAnimatorIds.ThLightAttack02)},
                {StaticAnimatorIds.ThLightAttack03, Animator.StringToHash(StaticAnimatorIds.ThLightAttack03)},
                {StaticAnimatorIds.ThHeavyAttack01, Animator.StringToHash(StaticAnimatorIds.ThHeavyAttack01)},
                {StaticAnimatorIds.ThHeavyAttack02, Animator.StringToHash(StaticAnimatorIds.ThHeavyAttack02)},
                {StaticAnimatorIds.LightPunch01, Animator.StringToHash(StaticAnimatorIds.LightPunch01)},
                {StaticAnimatorIds.HeavyPunch01, Animator.StringToHash(StaticAnimatorIds.HeavyPunch01)},
                {StaticAnimatorIds.OhCombo01, Animator.StringToHash(StaticAnimatorIds.OhCombo01)},
                {StaticAnimatorIds.OhCombo02, Animator.StringToHash(StaticAnimatorIds.OhCombo02)},
                {StaticAnimatorIds.OhHeavyCombo01, Animator.StringToHash(StaticAnimatorIds.OhHeavyCombo01)},
                {StaticAnimatorIds.HealSpell, Animator.StringToHash(StaticAnimatorIds.HealSpell)},
                {StaticAnimatorIds.FireballSpell, Animator.StringToHash(StaticAnimatorIds.FireballSpell)},
                {StaticAnimatorIds.ChestOpeningName, Animator.StringToHash(StaticAnimatorIds.ChestOpeningName)},
                {StaticAnimatorIds.LayDown2Name, Animator.StringToHash(StaticAnimatorIds.LayDown2Name)},
                {StaticAnimatorIds.RiposteName, Animator.StringToHash(StaticAnimatorIds.RiposteName)},
                {StaticAnimatorIds.RipostedName, Animator.StringToHash(StaticAnimatorIds.RipostedName)},
                {StaticAnimatorIds.ParryName, Animator.StringToHash(StaticAnimatorIds.ParryName)},
                {StaticAnimatorIds.ParriedName, Animator.StringToHash(StaticAnimatorIds.ParriedName)}
            };
            
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName], false);
        }

        private void OnAnimatorMove()
        {
            if (!_playerManager.isInteracting || _playerManager.isJumping)
            {
                return;
            }

            float delta = Time.deltaTime;
            _playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _playerLocomotion.rigidbody.velocity = velocity;
        }
        
        /// <summary>
        /// Update animator vertical and horizontal parameter
        /// </summary>
        /// <param name="verticalMovement">Movement in vertical axis</param>
        /// <param name="horizontalMovement">Movement in horizontal axis</param>
        /// <param name="isSprinting">Is player sprinting?</param>
        /// <param name="isWalking">Is player walking?</param>
        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting, bool isWalking)
        {
            #region Vertical
            float v;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting && isWalking)
            {
                isWalking = false;
            }
            
            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            else if(isWalking)
            {
                v = 0.5f;
                h = horizontalMovement;
            }

            anim.SetFloat(StaticAnimatorIds.animationIds[StaticAnimatorIds.VerticalName], v, 0.1f, Time.deltaTime);
            anim.SetFloat(StaticAnimatorIds.animationIds[StaticAnimatorIds.HorizontalName], h, 0.1f, Time.deltaTime);
        }

        /// <summary>
        /// Enable rotation
        /// </summary>
        public void CanRotate()
        {
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanRotateName], true);
        }

        /// <summary>
        /// Disable rotation
        /// </summary>
        public void StopRotation()
        {
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanRotateName], false);
        }

        /// <summary>
        /// Enable attack combo
        /// </summary>
        public void EnableCombo()
        {
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanDoComboName], true);
        }

        /// <summary>
        /// Disable attack combo
        /// </summary>
        public void DisableCombo()
        {
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanDoComboName], false);
        }

        /// <summary>
        /// Enable invulnerability
        /// </summary>
        public void EnableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInvulnerableName], true);
        }
        
        /// <summary>
        /// Disable invulnerability
        /// </summary>
        public void DisableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInvulnerableName], false);
        }
        
        /// <summary>
        /// Enable parrying
        /// </summary>
        public void EnableIsParrying()
        {
            _playerManager.isParrying = true;
        }

        /// <summary>
        /// Disable parrying
        /// </summary>
        public void DisableIsParrying()
        {
            _playerManager.isParrying = false;
        }

        /// <summary>
        /// Enable ability to be riposted
        /// </summary>
        public void EnableCanBeRiposted()
        {
            _playerManager.canBeRiposted = true;
        }

        /// <summary>
        /// Disable ability to be riposted
        /// </summary>
        public void DisableCanBeRiposted()
        {
            _playerManager.canBeRiposted = false;
        }
        
        /// <summary>
        /// Enable ability to be riposted
        /// </summary>
        public void EnableIsJumping()
        {
            _playerManager.isJumping = true;
            _playerLocomotion.rigidbody.useGravity = false;
        }

        /// <summary>
        /// Disable ability to be riposted
        /// </summary>
        public void DisableIsJumping()
        {
            _playerManager.isJumping = false;
            _playerLocomotion.rigidbody.useGravity = true;
        }

        /// <summary>
        /// Handle being back stabbed or riposted
        /// </summary>
        public override void TakeCriticalDamageAnimationEvent()
        {
            _playerStats.TakeDamage(_playerManager.pendingCriticalDamage, false, true);
            _playerManager.pendingCriticalDamage = 0.0f;
        }
    }
}
