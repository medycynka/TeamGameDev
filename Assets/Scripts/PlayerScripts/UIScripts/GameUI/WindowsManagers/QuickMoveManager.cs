using UnityEngine;
using SzymonPeszek.GameUI.Slots;
using SzymonPeszek.Items.Bonfire;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    /// <summary>
    /// Class for moving between bonfires
    /// </summary>
    public class QuickMoveManager : MonoBehaviour
    {
        public GameObject backgroundScreen;
        public GameObject buttonPrefab;
        public BonfireManager[] bonfireList;
        private QuickMoveSlot[] _quickMoveSlots;

        /// <summary>
        /// Update activated bonfires
        /// </summary>
        public void UpdateActiveBonfireList()
        {
            int id = 0;
            _quickMoveSlots = backgroundScreen.GetComponentsInChildren<QuickMoveSlot>();

            for (var i = 0; i < _quickMoveSlots.Length; i++)
            {
                _quickMoveSlots[i].ClearSlot();
            }

            for (var i = 0; i < bonfireList.Length; i++)
            {
                if (bonfireList[i].isActivated)
                {
                    bonfireList[i].quickMoveID = id;
                    Instantiate(buttonPrefab, backgroundScreen.transform);

                    buttonPrefab.GetComponent<QuickMoveSlot>().AddSlot(bonfireList[i].gameObject);
                }

                id++;
            }
        }
    }
}