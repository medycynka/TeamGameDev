using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SzymonPeszek.SaveScripts;


namespace SzymonPeszek.MainMenuUI {
    /// <summary>
    /// Class which manages main menu actions
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Resolution[] _resolutionsOpts;

        [Header("Settings Manager Manager", order = 0)]
        [Header("Character Creator Components", order = 1)]
        public GameObject characterCreatorScreen;

        public float fadeOutTime = 2.5f;

        [Header("Settings Options", order = 1)]
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullScreenToggle;
        public TMP_Dropdown qualityDropdown;
        public Slider mouseSlider;
        public Slider volumeSlider;

        private float _startMusicVolume;
        private float _currentTime;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _resolutionsOpts = Screen.resolutions;
            SettingsHolder.qualityID = QualitySettings.GetQualityLevel();
            
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

            DataManager dataManager = SaveManager.LoadGame();

            if (dataManager != null)
            {
                SettingsHolder.resolutionID = dataManager.resolutionID;
                SettingsHolder.isFullscreen = dataManager.isFullscreen;
                SettingsHolder.qualityID = dataManager.qualityID;
                SettingsHolder.mouseSensibility = dataManager.mouseSensibility;
                SettingsHolder.soundVolume = dataManager.soundVolume;
                SettingsHolder.isCharacterCreated = dataManager.isCharacterCreated;
                SettingsHolder.playerName = dataManager.playerName;
                SettingsHolder.isMale = dataManager.isMale;
                SettingsHolder.partsID[2] = dataManager.partsID[2];
                SettingsHolder.partsID[5] = dataManager.partsID[5];
                SettingsHolder.partsID[6] = dataManager.partsID[6];
                SettingsHolder.partsID[7] = dataManager.partsID[7];
                SettingsHolder.partsID[8] = dataManager.partsID[8];
            }

            resolutionDropdown.value = SettingsHolder.resolutionID;
            fullScreenToggle.isOn = SettingsHolder.isFullscreen;
            qualityDropdown.value = SettingsHolder.qualityID;
            mouseSlider.value = SettingsHolder.mouseSensibility;
            volumeSlider.value = SettingsHolder.soundVolume;
            _audioSource.volume = SettingsHolder.soundVolume;
            _startMusicVolume = SettingsHolder.soundVolume;

            resolutionDropdown.RefreshShownValue();

            SettingsHolder.dataManager = dataManager;
        }

        /// <summary>
        /// Save settings data
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
        /// Load main level or open character creation screen on first play
        /// </summary>
        public void PlayGame()
        {
            if (SettingsHolder.isCharacterCreated)
            {
                SettingsHolder.firstStart = false;
                FadeOutMusic();
            }
            else
            {
                SettingsHolder.firstStart = true;
                characterCreatorScreen.SetActive(true);
            }
        }

        /// <summary>
        /// Fade out music during next scene loading
        /// </summary>
        public void FadeOutMusic()
        {
            _currentTime = 0.0f;
            
            StartCoroutine(SwitchToNextScene());
        }
        
        /// <summary>
        /// Exit game
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// Set game resolution
        /// </summary>
        /// <param name="resolutionId">Game resolution</param>
        public void SetResolution(int resolutionId)
        {
            Screen.SetResolution(_resolutionsOpts[resolutionId].width, _resolutionsOpts[resolutionId].height, Screen.fullScreen);
            SettingsHolder.resolutionID = resolutionId;
        }

        /// <summary>
        /// Set game window to fullscreen or revert to normal window
        /// </summary>
        /// <param name="isFullScreen">Should screen be set to fullscreen?</param>
        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            SettingsHolder.isFullscreen = isFullScreen;
        }

        /// <summary>
        /// Set game quality
        /// </summary>
        /// <param name="qualityId">Game quality</param>
        public void SetQuality(int qualityId)
        {
            QualitySettings.SetQualityLevel(qualityId);
            SettingsHolder.qualityID = qualityId;
        }

        /// <summary>
        /// Set mouse sensibility
        /// </summary>
        /// <param name="sensibility">Mouse sensibility</param>
        public void SetMouseSensibility(float sensibility)
        {
            SettingsHolder.mouseSensibility = sensibility;
        }

        /// <summary>
        /// Set music volume
        /// </summary>
        /// <param name="volume">Music Volume</param>
        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
            _startMusicVolume = volume;
            SettingsHolder.soundVolume = volume;
        }

        /// <summary>
        /// Coroutine whick loads next scene
        /// </summary>
        /// <returns>Coroutine's enumerator</returns>
        private IEnumerator SwitchToNextScene()
        {
            while (_currentTime <= fadeOutTime)
            {
                _audioSource.volume = Mathf.Lerp(_startMusicVolume, 0.0f, _currentTime / fadeOutTime);
                _currentTime += Time.deltaTime;

                yield return null;
            }
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}