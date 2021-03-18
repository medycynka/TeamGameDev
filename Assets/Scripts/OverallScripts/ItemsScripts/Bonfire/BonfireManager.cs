using UnityEngine;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.GameUI;
using SzymonPeszek.Environment.Areas;


namespace SzymonPeszek.Items.Bonfire
{
    /// <summary>
    /// Class for managing bonfire's actions
    /// </summary>
    public class BonfireManager : MonoBehaviour
    {
        [Header("Bonfire Manager", order = 0)]
        [Header("Activation properties", order = 1)]
        public bool isActivated;
        public bool showRestPopUp;
        public Light bonfireLight;
        public ParticleSystem bonfireParticleSystem;
        public GameObject bonfireLitScreen;

        [Header("UI objects", order = 1)]
        public UIManager uiManager;
        public GameObject playerUI;
        public GameObject uiWindow;
        public GameObject restUI;
        public GameObject quickMoveScreen;

        [Header("Player Scripts", order = 1)]
        public PlayerManager playerManager;

        [Header("Quick Move", order = 1)]
        public string locationName = "Test Field";
        public GameObject locationListScreen;
        public GameObject locationScreen;
        public int quickMoveID;
        public GameObject spawnPoint;

        [Header("Enemy Spawner", order = 1)]
        public bool isBossBonfire = false;
        public EnemySpawner enemySpawner;

        private void Start()
        {
            if (isActivated)
            {
                bonfireParticleSystem.Play();
                bonfireLight.enabled = true;
            }
            else
            {
                bonfireLight.enabled = false;
                bonfireParticleSystem.Stop();
            }
        }

        /// <summary>
        /// Activate rest UI
        /// </summary>
        public void ActivateRestUI()
        {
            playerUI.SetActive(false);
            uiWindow.SetActive(true);
            restUI.SetActive(true);
        }

        /// <summary>
        /// Close rest UI
        /// </summary>
        public void CloseRestUI()
        {
            restUI.SetActive(false);
            locationListScreen.SetActive(false);
            uiWindow.SetActive(false);
            playerUI.SetActive(true);
        }

        /// <summary>
        /// Activate quick move locations screen
        /// </summary>
        public void ActivateQuickMoveScreen()
        {
            locationListScreen.SetActive(false);
            restUI.SetActive(false);
            uiWindow.SetActive(false);
            playerUI.SetActive(true);
            quickMoveScreen.SetActive(true);
        }

        /// <summary>
        /// Close quick move locations screen
        /// </summary>
        public void CloseQuickMoveScreen()
        {
            quickMoveScreen.SetActive(false);
        }

        /// <summary>
        /// Respawn enemies in current area
        /// </summary>
        public void RespawnEnemies()
        {
            if (!isBossBonfire)
            {
                if (enemySpawner != null)
                {
                    enemySpawner.SpawnEnemies();
                }
            }
        }
    }
}