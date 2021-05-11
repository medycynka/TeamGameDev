using System.Collections;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Environment.Sounds;


namespace SzymonPeszek.Environment.Areas
{
    /// <summary>
    /// Class for managing area events
    /// </summary>
    public class AreaManager : LocationManager
    {
        [Header("Area Manager", order = 0)]
        private EnemySpawner _enemySpawner;
        private bool _insideReset = true;
        private AudioClip[] _footStepsOnExit;

        private void Awake()
        {
            _enemySpawner = GetComponentInChildren<EnemySpawner>();

            foreach(var bonfire in bonfiresInArea)
            {
                bonfire.enemySpawner = _enemySpawner;
            }
        }

        public void SetExitFootSteps(AudioClip[] exitFootStep)
        {
            _footStepsOnExit = exitFootStep;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                isInside = true;

                if (_insideReset)
                {
                    if (playerStats == null)
                    {
                        playerStats = other.GetComponent<PlayerStats>();
                    }

                    if (playerSoundManager == null)
                    {
                        playerSoundManager = other.GetComponent<AnimationSoundManager>();
                    }

                    playerSoundManager.ChangeBackGroundMusic(areaBgMusic);
                    // playerSoundManager.ChangeFootstepsSound(footSteps, this);

                    StartCoroutine(ShowAreaName());
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                if (isInside && _insideReset)
                {
                    _insideReset = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                playerStats = null;
                isInside = false;
                _insideReset = true;
                playerSoundManager.ChangeBackGroundMusic(null);
                playerSoundManager.currentMovingClips = _footStepsOnExit;
            }
        }

        private IEnumerator ShowAreaName()
        {
            locationScreenText.text = areaName;
            locationScreen.SetActive(true);

            yield return CoroutineYielder.waitFor1HalfSecond;

            locationScreenText.text = "";
            locationScreen.SetActive(false);
        }
    }
}