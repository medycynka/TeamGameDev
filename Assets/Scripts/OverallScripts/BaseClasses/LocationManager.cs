using UnityEngine;
using TMPro;
using SzymonPeszek.Items.Bonfire;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Environment.Sounds;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for area managers
    /// </summary>
    public class LocationManager : MonoBehaviour
    {
        [Header("Area Name", order = 1)]
        public string areaName = "";

        [Header("Location Screen Properties", order = 1)]
        public GameObject locationScreen;
        public TextMeshProUGUI locationScreenText;

        [Header("Bonfires in Area", order = 1)]
        public BonfireManager[] bonfiresInArea;

        [Header("Sound Manager", order = 1)]
        public AnimationSoundManager playerSoundManager;

        [Header("Area Sounds", order = 1)]
        public AudioClip areaBgMusic;
        public AudioClip[] footSteps;

        [Header("Bools", order = 1)]
        public bool isInside;
        public bool isPlayerDead;

        [Header("Player Stats", order = 1)]
        public PlayerStats playerStats;
        protected const string PlayerTag = "Player";
    }
}