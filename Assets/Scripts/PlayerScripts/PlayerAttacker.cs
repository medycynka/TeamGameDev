using System;
using UnityEngine;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.Items.Weapons;
using SzymonPeszek.Misc;
using SzymonPeszek.Enums;
using SzymonPeszek.EnemyScripts;


namespace SzymonPeszek.PlayerScripts
{
    /// <summary>
    /// Class which manages player's attacks
    /// </summary>
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorManager _playerAnimatorManager;
        private InputHandler _inputHandler;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerInventory _playerInventory;
        private PlayerManager _playerManager;
        private PlayerStats _playerStats;
        private RaycastHit _hit;
        
        public LayerMask specialAttackLayer;
        public LayerMask backStabLayer;
        public LayerMask riposteLayer;
        

        [Header("Last Attack Name")]
        public string lastAttack;

        private void Awake()
        {
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            _weaponSlotManager = GetComponent<WeaponSlotManager>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerStats = GetComponentInParent<PlayerStats>();
            specialAttackLayer = (1 << LayerMask.NameToLayer("Back Stab") | 1 << LayerMask.NameToLayer("Riposte"));
            backStabLayer = 1 << LayerMask.NameToLayer("Back Stab");
            riposteLayer = 1 << LayerMask.NameToLayer("Riposte");
        }

        /// <summary>
        /// Perform combo attack
        /// </summary>
        /// <param name="weapon">Attacking weapon</param>
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (_inputHandler.comboFlag)
            {
                _playerAnimatorManager.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.CanDoComboName], false);

                if (lastAttack == weapon.ohLightAttack1)
                {
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.ohLightAttack2], true);
                    lastAttack = weapon.ohLightAttack2;
                }
                else if (lastAttack == weapon.ohLightAttack2)
                {
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.ohLightAttack3], true);
                } 
                else if(lastAttack == weapon.ohHeavyAttack1)
                {
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.ohHeavyAttack2], true);
                }
                else if (lastAttack == weapon.thLightAttack1)
                {
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.thLightAttack2], true);
                    lastAttack = weapon.thLightAttack2;
                }
                else if (lastAttack == weapon.thLightAttack2)
                {
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.thLightAttack3], true);
                }
                else if (lastAttack == weapon.thHeavyAttack1)
                {
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.thHeavyAttack2], true);
                    lastAttack = weapon.thHeavyAttack2;
                }
            }
        }

        /// <summary>
        /// Perform light attack
        /// </summary>
        /// <param name="weapon">Attacking weapon</param>
        private void HandleLightAttack(WeaponItem weapon)
        {
            _weaponSlotManager.attackingWeapon = weapon;

            if (_inputHandler.twoHandFlag)
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.thLightAttack1], true);
                lastAttack = weapon.thLightAttack1;
            }
            else
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.ohLightAttack1], true);
                lastAttack = weapon.ohLightAttack1;
            }
        }

        /// <summary>
        /// Perform heavy attack
        /// </summary>
        /// <param name="weapon">Attacking weapon</param>
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            _weaponSlotManager.attackingWeapon = weapon;

            if (_inputHandler.twoHandFlag)
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.thHeavyAttack1], true);
                lastAttack = weapon.thHeavyAttack1;
            }
            else
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[weapon.ohHeavyAttack1], true);
                lastAttack = weapon.ohHeavyAttack1;
            }
        }

        /// <summary>
        /// Handle right hand attack
        /// </summary>
        public void HandleRbAction()
        {
            switch (_playerInventory.rightWeapon.weaponType)
            {
                case WeaponType.Melee:
                    PerformRbMeleeAction();
                    break;
                case WeaponType.Casting:
                    PerformRbMagicAction(_playerInventory.rightWeapon);
                    break;
                case WeaponType.Shooting:
                    break;
            }
        }

        /// <summary>
        /// Handle left hand action (block, parry, etc.)
        /// </summary>
        public void HandleLtAction()
        {
            switch (_playerInventory.leftWeapon.meleeType)
            {
                case MeleeType.Shield:
                    PerformLtWeaponArt(_inputHandler.twoHandFlag);
                    break;
                case MeleeType.NotMelee:
                    break;
                default:
                    // Handle left hand attack
                    break;
            }
        }
        
        #region Attack Actions
        /// <summary>
        /// Perform right hand attack
        /// </summary>
        private void PerformRbMeleeAction()
        {
            if (_playerManager.canDoCombo)
            {
                _inputHandler.comboFlag = true;
                HandleWeaponCombo(_playerInventory.rightWeapon);
                _inputHandler.comboFlag = false;
            }
            else
            {
                if (_playerManager.isInteracting)
                {
                    return;
                }

                if (_playerManager.canDoCombo)
                {
                    return;
                }

                _playerAnimatorManager.anim.SetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsUsingRightHandName], true);
                HandleLightAttack(_playerInventory.rightWeapon);
            }
        }

        /// <summary>
        /// Perform left hand action
        /// </summary>
        private void PerformLtWeaponArt(bool isTwoHanding)
        {
            if (_playerManager.isInteracting)
            {
                return;
            }

            if (isTwoHanding)
            {
                // Handle two handle input
            }
            else
            {
                _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[_playerInventory.leftWeapon.weaponArt], true);
            }
        }

        /// <summary>
        /// Perform right hand magic attack
        /// </summary>
        /// <param name="weapon">Attacking weapon</param>
        private void PerformRbMagicAction(WeaponItem weapon)
        {
            if (_playerManager.isInteracting)
            {
                return;
            }
            
            if (_playerInventory.currentSpell != null && _playerStats.currentFocus > 0)
            {
                switch (weapon.castingType)
                {
                    case CastingType.Faith:
                        if (_playerInventory.currentSpell.spellType == CastingType.Faith)
                        {
                            _playerInventory.currentSpell.AttemptToCastSpell(_playerAnimatorManager, _playerStats);
                        }
                        
                        break;
                    case CastingType.Curse:
                        if (_playerInventory.currentSpell.spellType == CastingType.Curse)
                        {
                            _playerInventory.currentSpell.AttemptToCastSpell(_playerAnimatorManager, _playerStats);
                        }
                        break;
                    case CastingType.Destruction:
                        if (_playerInventory.currentSpell.spellType == CastingType.Destruction)
                        {
                            _playerInventory.currentSpell.AttemptToCastSpell(_playerAnimatorManager, _playerStats);
                        }
                        break;
                    case CastingType.NotCasting:
                        break;
                }
            }
        }
        
        /// <summary>
        /// Cast a magic spell
        /// </summary>
        private void SuccessfullyCastSpell()
        {
            _playerInventory.currentSpell.SuccessfullyCastSpell(_playerAnimatorManager, _playerStats);
        }
        
        /// <summary>
        /// Back stab enemy or riposte enemy's attack
        /// </summary>
        public void AttemptBackStabOrRiposte()
        {
            if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out _hit, 0.75f, backStabLayer))
            {
                EnemyManager enemyCharacterManager = _hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyCharacterManager != null)
                {
                    _playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;
                    Vector3 rotationDirection = _hit.transform.position - _playerStats.characterTransform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(_playerStats.characterTransform.rotation, tr, 500 * Time.deltaTime);
                    _playerStats.characterTransform.rotation = targetRotation;

                    float criticalHitDamage = _playerInventory.rightWeapon.criticalDamageMult * _weaponSlotManager.rightHandDamageCollider.currentWeaponDamage;

                    enemyCharacterManager.pendingCriticalDamage = criticalHitDamage;
                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.BackStabName], true);
                    enemyCharacterManager.HandleBackStabOrRiposte(true);
                }
            }
            else if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out _hit, 1.5f, riposteLayer))
            {
                EnemyManager enemyCharacterManager = _hit.transform.gameObject.GetComponentInParent<EnemyManager>();

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    enemyCharacterManager.isGettingRiposted = true;
                    _playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

                    Vector3 rotationDirection = _hit.transform.position - _playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(_playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    _playerManager.transform.rotation = targetRotation;

                    float criticalDamage = _playerInventory.rightWeapon.criticalDamageMult * _weaponSlotManager.rightHandDamageCollider.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    _playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.RiposteName], true);
                    enemyCharacterManager.HandleBackStabOrRiposte(false);
                }
            }
        }
        #endregion
    }

}