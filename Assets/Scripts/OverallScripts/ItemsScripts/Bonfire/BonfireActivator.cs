using System.Collections;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Misc;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.Items.Bonfire
{
    /// <summary>
    /// Class for activating bonfire
    /// </summary>
    public class BonfireActivator : Interactable
    {
        private BonfireManager _bonfireManager;

        private void Awake()
        {
            _bonfireManager = GetComponent<BonfireManager>();
        }

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public override void Interact(PlayerManager playerManager)
        {
            ActivateFireplace(playerManager);
        }

        /// <summary>
        /// Activate bonfire
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        private void ActivateFireplace(PlayerManager playerManager)
        {
            base.PickUpItem(playerManager);

            _bonfireManager.bonfireParticleSystem.Play();
            _bonfireManager.bonfireLight.enabled = true;
            _bonfireManager.isActivated = true;

            StartCoroutine(DisplayScreen());
        }

        /// <summary>
        /// Coroutine for displaying bonfire lit screen
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator DisplayScreen()
        {
            _bonfireManager.bonfireLitScreen.SetActive(true);

            yield return CoroutineYielder.waitFor2Seconds;

            _bonfireManager.bonfireLitScreen.SetActive(false);
            _bonfireManager.showRestPopUp = true;
        }
    }

}