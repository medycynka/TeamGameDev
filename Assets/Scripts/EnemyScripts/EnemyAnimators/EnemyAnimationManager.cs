using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;


namespace SzymonPeszek.EnemyScripts.Animations
{
    /// <summary>
    /// Class for managing enemy's animations
    /// </summary>
    public class EnemyAnimationManager : AnimationManager
    {
        private EnemyManager _enemyManager;
        private EnemyStats _enemyStats;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
            _enemyStats = GetComponentInParent<EnemyStats>();

            if (StaticAnimatorIds.enemyAnimationIds == null)
            {
                StaticAnimatorIds.enemyAnimationIds = new Dictionary<string, int>
                {
                    {StaticAnimatorIds.VerticalName, Animator.StringToHash(StaticAnimatorIds.VerticalName)},
                    {StaticAnimatorIds.HorizontalName, Animator.StringToHash(StaticAnimatorIds.HorizontalName)},
                    {StaticAnimatorIds.IsInteractingName, Animator.StringToHash(StaticAnimatorIds.IsInteractingName)},
                    {StaticAnimatorIds.CanDoComboName, Animator.StringToHash(StaticAnimatorIds.CanDoComboName)},
                    {StaticAnimatorIds.IsInvulnerableName, Animator.StringToHash(StaticAnimatorIds.IsInvulnerableName)},
                    {StaticAnimatorIds.IsDeadName, Animator.StringToHash(StaticAnimatorIds.IsDeadName)},
                    {StaticAnimatorIds.IsInAirName, Animator.StringToHash(StaticAnimatorIds.IsInAirName)},
                    {StaticAnimatorIds.CanRotateName, Animator.StringToHash(StaticAnimatorIds.CanRotateName)},
                    {StaticAnimatorIds.EmptyName, Animator.StringToHash(StaticAnimatorIds.EmptyName)},
                    {StaticAnimatorIds.Damage01Name, Animator.StringToHash(StaticAnimatorIds.Damage01Name)},
                    {StaticAnimatorIds.Death01Name, Animator.StringToHash(StaticAnimatorIds.Death01Name)},
                    {StaticAnimatorIds.GetUpName, Animator.StringToHash(StaticAnimatorIds.GetUpName)},
                    {StaticAnimatorIds.SleepName, Animator.StringToHash(StaticAnimatorIds.SleepName)},
                    {StaticAnimatorIds.BackStabName, Animator.StringToHash(StaticAnimatorIds.BackStabName)},
                    {StaticAnimatorIds.BackStabbedName, Animator.StringToHash(StaticAnimatorIds.BackStabbedName)},
                    {StaticAnimatorIds.LayDownName, Animator.StringToHash(StaticAnimatorIds.LayDownName)},
                    {StaticAnimatorIds.EnemyMaceAttack01, Animator.StringToHash(StaticAnimatorIds.EnemyMaceAttack01)},
                    {StaticAnimatorIds.EnemyMaceAttack02, Animator.StringToHash(StaticAnimatorIds.EnemyMaceAttack02)},
                    {StaticAnimatorIds.EnemyMaceAttack03, Animator.StringToHash(StaticAnimatorIds.EnemyMaceAttack03)},
                    {StaticAnimatorIds.EnemyStaffAttack01, Animator.StringToHash(StaticAnimatorIds.EnemyStaffAttack01)},
                    {StaticAnimatorIds.EnemyStaffAttack02, Animator.StringToHash(StaticAnimatorIds.EnemyStaffAttack02)},
                    {StaticAnimatorIds.EnemySpearAttack01, Animator.StringToHash(StaticAnimatorIds.EnemySpearAttack01)},
                    {StaticAnimatorIds.EnemySpearAttack02, Animator.StringToHash(StaticAnimatorIds.EnemySpearAttack02)},
                    {StaticAnimatorIds.EnemySwordAttack01, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack01)},
                    {StaticAnimatorIds.EnemySwordAttack02, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack02)},
                    {StaticAnimatorIds.EnemySwordAttack03, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack03)},
                    {StaticAnimatorIds.EnemySwordAttack04, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack04)},
                    {StaticAnimatorIds.EnemySwordAttack05, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack05)},
                    {StaticAnimatorIds.EnemySwordAttack06, Animator.StringToHash(StaticAnimatorIds.EnemySwordAttack06)},
                    {StaticAnimatorIds.LayDown2Name, Animator.StringToHash(StaticAnimatorIds.LayDown2Name)},
                    {StaticAnimatorIds.RiposteName, Animator.StringToHash(StaticAnimatorIds.RiposteName)},
                    {StaticAnimatorIds.RipostedName, Animator.StringToHash(StaticAnimatorIds.RipostedName)},
                    {StaticAnimatorIds.ParryName, Animator.StringToHash(StaticAnimatorIds.ParryName)},
                    {StaticAnimatorIds.ParriedName, Animator.StringToHash(StaticAnimatorIds.ParriedName)}
                };
            }

            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsDeadName], false);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            
            if (delta != 0.0f)
            {
                Vector3 velocity = deltaPosition / delta;
                _enemyManager.enemyRigidBody.velocity = velocity;
            }
            else
            {
                _enemyManager.enemyRigidBody.velocity = Vector3.zero;
            }
        }
        
        public override void TakeCriticalDamageAnimationEvent()
        {
            _enemyStats.TakeDamage(_enemyManager.pendingCriticalDamage, false, true);
            _enemyManager.pendingCriticalDamage = 0.0f;
        }
        
        /// <summary>
        /// Enable rotation
        /// </summary>
        public void CanRotate()
        {
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanRotateName], true);
        }

        /// <summary>
        /// Disable rotation
        /// </summary>
        public void StopRotation()
        {
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanRotateName], false);
        }

        /// <summary>
        /// Enable attack combo
        /// </summary>
        public void EnableCombo()
        {
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanDoComboName], true);
        }

        /// <summary>
        /// Disable attack combo
        /// </summary>
        public void DisableCombo()
        {
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanDoComboName], false);
        }

        /// <summary>
        /// Enable invulnerability
        /// </summary>
        public void EnableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInvulnerableName], true);
        }
        
        /// <summary>
        /// Disable invulnerability
        /// </summary>
        public void DisableIsInvulnerable()
        {
            anim.SetBool(StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInvulnerableName], false);
        }
        
        /// <summary>
        /// Enable parrying
        /// </summary>
        public void EnableIsParrying()
        {
            _enemyManager.isParrying = true;
        }

        /// <summary>
        /// Disable parrying
        /// </summary>
        public void DisableIsParrying()
        {
            _enemyManager.isParrying = false;
        }

        /// <summary>
        /// Enable ability to be riposted
        /// </summary>
        public void EnableCanBeRiposted()
        {
            _enemyManager.canBeRiposted = true;
        }

        /// <summary>
        /// Disable ability to be riposted
        /// </summary>
        public void DisableCanBeRiposted()
        {
            _enemyManager.canBeRiposted = false;
        }
    }
}
