using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.PlayerScripts.Inventory;
using UnityEngine;


namespace SzymonPeszek.Environment.Areas
{
    public class TeleportManager : Interactable
    {
        [Header("Teleport properties", order = 1)]
        public GameObject teleportScreen;
        public Transform teleportDestination;

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            Teleport(playerManager);
        }

        /// <summary>
        /// Teleport player
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        protected void Teleport(PlayerManager playerManager)
        {
            playerManager.StopPlayer();
            StartCoroutine(TeleportPlayer(playerManager));
        }
        private IEnumerator TeleportPlayer(PlayerManager playerManager)
        {
            playerManager.isRemovingFog = true;
            teleportScreen.SetActive(true);
            playerManager.characterTransform.position = teleportDestination.position;
            
            yield return CoroutineYielder.waitFor2Seconds;
            
            teleportScreen.SetActive(false);
            playerManager.isRemovingFog = false;
        }
    }
}