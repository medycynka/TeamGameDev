using System.Collections;
using UnityEngine;
using TMPro;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.GameUI.WindowsManagers;
using SzymonPeszek.Misc;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.Items.Bonfire
{
    /// <summary>
    /// Class allowing resting at the bonfire
    /// </summary>
    public class BonfireInteraction : Interactable
    {
        private BonfireManager _bonfireManager;
        private PlayerStats _playerStats;
        private TextMeshProUGUI _locationNameScree;

        private void Awake()
        {
            _bonfireManager = GetComponent<BonfireManager>();
            _locationNameScree = _bonfireManager.locationScreen.GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            RestAtBonfire(playerManager);
        }

        /// <summary>
        /// Rest at bonfire
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        private void RestAtBonfire(PlayerManager playerManager)
        {
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();
            _playerStats = playerManager.GetComponent<PlayerStats>();
            _bonfireManager.restUI.GetComponent<RestManager>().bonfireInteraction = this;

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.SitName], true);
            _bonfireManager.playerManager.isRestingAtBonfire = true;

            _playerStats.HealPlayer(_playerStats.maxHealth);
            _playerStats.HealStamina(_playerStats.maxStamina);
            _bonfireManager.ActivateRestUI();
            playerManager.currentSpawnPoint = _bonfireManager.spawnPoint;
            _bonfireManager.RespawnEnemies();

            SaveManager.SaveGame(playerManager, _playerStats, playerManager.GetComponent<PlayerInventory>());
        }

        /// <summary>
        /// Stop resting and get up
        /// </summary>
        public void GetUp()
        {
            if (playerAnimatorManager == null)
            {
                playerAnimatorManager = _bonfireManager.playerManager.GetComponentInChildren<PlayerAnimatorManager>();
            }

            _bonfireManager.uiManager.UpdateSouls();
            _bonfireManager.CloseRestUI();
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.StandUpName], true);
            _bonfireManager.playerManager.isRestingAtBonfire = false;

            if(_playerStats == null)
            {
                _playerStats = _bonfireManager.playerManager.GetComponent<PlayerStats>();
            }
        }

        /// <summary>
        /// Move to this bonfire
        /// </summary>
        public void QuickMove()
        {
            if(playerAnimatorManager == null)
            {
                playerAnimatorManager = _bonfireManager.playerManager.GetComponentInChildren<PlayerAnimatorManager>();
            }

            StartCoroutine(TeleportToNextBonfire());
        }

        /// <summary>
        /// Coroutine for teleporting player to this bonfire
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator TeleportToNextBonfire()
        {
            _bonfireManager.ActivateQuickMoveScreen();
            _bonfireManager.uiManager.UpdateSouls();
            _playerStats.characterTransform.position = _bonfireManager.spawnPoint.transform.position;
            _playerStats.characterTransform.rotation = _bonfireManager.spawnPoint.transform.rotation;

            yield return CoroutineYielder.waitFor5Second;

            _bonfireManager.CloseQuickMoveScreen();
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.StandUpName], true);
            _bonfireManager.locationScreen.SetActive(true);
            _locationNameScree.text = _bonfireManager.locationName;

            yield return CoroutineYielder.waitFor1HalfSecond;

            _bonfireManager.locationScreen.SetActive(false);
            _bonfireManager.playerManager.isRestingAtBonfire = false;
            _bonfireManager.playerManager.currentSpawnPoint = _bonfireManager.spawnPoint;
        }
    }
}