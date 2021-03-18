using System;
using System.Collections;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Environment.Areas
{
    /// <summary>
    /// Class representing fog wall object
    /// </summary>
    public class FogWallManager : Interactable
    {
        [Header("Fog Wall Manager", order = 1)]
        [Header("Fog Wall Components", order = 2)]
        public BoxCollider boxCollider;
        public ParticleSystem wallParticles;

        [Header("Bools", order = 2)]
        public bool canInteract = true;
        public bool shouldDestroy = false;

        private void Awake()
        {
            if (wallParticles)
            {
                wallParticles.Play();
            }
        }

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            PickUpItem(playerManager);
        }

        /// <summary>
        /// Remove fog wall or go through it with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        protected override void PickUpItem(PlayerManager playerManager)
        {
            boxCollider.enabled = false;

            base.PickUpItem(playerManager);

            StartCoroutine(shouldDestroy ? DestroyFog(playerManager) : RemoveFog(playerManager));
        }

        /// <summary>
        /// Coroutine for destroying fog wall
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator DestroyFog(PlayerManager playerManager)
        {
            playerManager.isRemovingFog = true;
            wallParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            yield return CoroutineYielder.waitFor2Seconds;

            playerManager.isRemovingFog = false;

            Destroy(gameObject);
        }

        /// <summary>
        /// Coroutine for temporary removing fog wall
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator RemoveFog(PlayerManager playerManager)
        {
            canInteract = false;
            playerManager.isRemovingFog = true;
            
            yield return CoroutineYielder.waitFor2Seconds;
            
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.FogRemoveName], true);

            yield return CoroutineYielder.waitFor2HalfSeconds;

            playerManager.isRemovingFog = false;
            boxCollider.enabled = true;
        }
    }
}