using UnityEngine;
using SzymonPeszek.Items.Bonfire;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    /// <summary>
    /// Class which manages resting at the bonfire 
    /// </summary>
    public class RestManager : MonoBehaviour
    {
        public BonfireInteraction bonfireInteraction;

        /// <summary>
        /// Get up from a bonfire
        /// </summary>
        public void GetUp()
        {
            bonfireInteraction.GetUp();
        }
    }
}