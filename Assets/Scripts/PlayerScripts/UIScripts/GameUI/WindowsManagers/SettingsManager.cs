using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;
using SzymonPeszek.PlayerScripts.CameraManager;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    /// <summary>
    /// Class for managing settings during game
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        private readonly List<AudioSource> _audioSources = new List<AudioSource>();
        private Resolution[] _resolutionsOpts;

        [Header("Settings Manager", order = 0)]
        public PlayerManager playerManager;
        public CameraHandler cameraHandler;
        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToggle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        private void Start()
        {
            _audioSources.Add(playerManager.GetComponent<AudioSource>());

            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy.GetComponent<AudioSource>() != null)
                {
                    _audioSources.Add(enemy.GetComponent<AudioSource>());
                }
            }

            _resolutionsOpts = Screen.resolutions;
            List<string> resList = new List<string>();

            for (int i = 0; i < _resolutionsOpts.Length; i++)
            {
                resList.Add(_resolutionsOpts[i].width + "x" + _resolutionsOpts[i].height);

                if (_resolutionsOpts[i].width == Screen.width && _resolutionsOpts[i].height == Screen.height)
                {
                    SettingsHolder.resolutionID = i;
                }
            }

            resolutionDropdown.AddOptions(resList.Distinct().ToList());

            LoadSettings();
        }

        /// <summary>
        /// Loads settings properties
        /// </summary>
        public void LoadSettings()
        {
            resolutionDropdown.value = SettingsHolder.resolutionID;
            resolutionDropdown.RefreshShownValue();
            
            fullScreenToggle.isOn = SettingsHolder.isFullscreen;
            
            qualityDropdown.value = SettingsHolder.qualityID;
            qualityDropdown.RefreshShownValue();
            
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;
        }

        /// <summary>
        /// Save settings properties
        /// </summary>
        public void SaveSettings()
        {
            SettingsHolder.resolutionID = resolutionDropdown.value;
            SettingsHolder.isFullscreen = fullScreenToggle.isOn;
            SettingsHolder.qualityID = qualityDropdown.value;
            SettingsHolder.mouseSensibility = mouseSlider.value;
            SettingsHolder.soundVolume = volumeSlider.value;
        }

        /// <summary>
        /// Set screen resolution
        /// </summary>
        /// <param name="resolutionId">Screen resolution</param>
        public void SetResolution(int resolutionId)
        {
            Screen.SetResolution(_resolutionsOpts[resolutionId].width, _resolutionsOpts[resolutionId].height, Screen.fullScreen);
        }

        /// <summary>
        /// Set or unset fullscreen
        /// </summary>
        /// <param name="isFullScreen">Set or unset?</param>
        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        /// <summary>
        /// Set game's graphic quality
        /// </summary>
        /// <param name="qualityId">Graphic quality</param>
        public void SetQuality(int qualityId)
        {
            QualitySettings.SetQualityLevel(qualityId);
        }

        /// <summary>
        /// Set mouse sensibility
        /// </summary>
        /// <param name="sensibility">Mouse sensibility</param>
        public void SetMouseSensibility(float sensibility)
        {
            cameraHandler.lookSpeed = (sensibility / 1000f);
        }

        /// <summary>
        /// Set sound volume
        /// </summary>
        /// <param name="volume">Sound volume</param>
        public void SetVolume(float volume)
        {
            SetAllSounds(volume);
        }

        /// <summary>
        /// Save game's data and exit the game
        /// </summary>
        public void SaveAndExit()
        {
            SaveSettings();
            SaveManager.SaveGame(playerManager, playerManager.GetComponent<PlayerStats>(), playerManager.GetComponent<PlayerInventory>());
            Application.Quit();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// Set all sounds volume
        /// </summary>
        /// <param name="volume">Sounds volume</param>
        private void SetAllSounds(float volume)
        {
            for (var i = 0; i < _audioSources.Count; i++)
            {
                _audioSources[i].volume = volume;
            }
        }
    }
}