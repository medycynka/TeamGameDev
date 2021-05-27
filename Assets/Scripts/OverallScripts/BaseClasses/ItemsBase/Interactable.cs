using UnityEngine;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Controller;
using SzymonPeszek.GameUI;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for interactable objects
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        [Header("Interactable Object Properties", order = 0)]
        public float radius = 0.6f;
        public string interactableText = "Pick up";

        public PlayerInventory playerInventory;
        public PlayerLocomotion playerLocomotion;
        public PlayerAnimatorManager playerAnimatorManager;
        public UIManager uIManager;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        /// <summary>
        /// Interact with object
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        public virtual void Interact(PlayerManager playerManager)
        {
            if (!playerInventory)
            {
                playerInventory = playerManager.GetComponent<PlayerInventory>();
            }
            if (!playerLocomotion)
            {
                playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            }
            if (!playerAnimatorManager)
            {
                playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();
            }
            if (!uIManager)
            {
                uIManager = playerManager.GetComponent<InputHandler>().uiManager;
            }
        }

        /// <summary>
        /// Pick up interactable item
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        protected virtual void PickUpItem(PlayerManager playerManager)
        {
            if (!playerInventory)
            {
                playerInventory = playerManager.GetComponent<PlayerInventory>();
            }
            if (!playerLocomotion)
            {
                playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            }
            if (!playerAnimatorManager)
            {
                playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();
            }
            if (!uIManager)
            {
                uIManager = playerManager.GetComponent<InputHandler>().uiManager;
            }

            playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
            playerAnimatorManager.PlayTargetAnimation(StaticAnimatorIds.animationIds[StaticAnimatorIds.PickUpName], true); //Plays the animation of looting the item
        }
    }

}
