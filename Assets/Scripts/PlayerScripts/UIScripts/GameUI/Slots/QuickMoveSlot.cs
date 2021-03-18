using UnityEngine;
using TMPro;
using SzymonPeszek.Items.Bonfire;

namespace SzymonPeszek.GameUI.Slots
{ 
    /// <summary>
    /// Class representing location quick move slots
    /// </summary>
    public class QuickMoveSlot : MonoBehaviour
    {
        public GameObject bonfirePrefab;
        public TextMeshProUGUI locationName;

        /// <summary>
        /// Add location to this slot
        /// </summary>
        /// <param name="bonfire">Bonfire object representing location</param>
        public void AddSlot(GameObject bonfire)
        {
            bonfirePrefab = bonfire;
            locationName.text = bonfirePrefab.GetComponent<BonfireManager>().locationName;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Remove location from this slot
        /// </summary>
        public void ClearSlot()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Teleport player to location from this slot
        /// </summary>
        public void TeleportHere()
        {
            bonfirePrefab.GetComponent<BonfireInteraction>().QuickMove();
        }
    }
}