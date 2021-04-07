using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.Environment.Sounds;
using SzymonPeszek.Misc;
using SzymonPeszek.EnemyScripts;


namespace SzymonPeszek.Environment.Areas
{
    /// <summary>
    /// Class for managing boss area events
    /// </summary>
    public class BossAreaManager : LocationManager
    {
        [Header("Area Manager", order = 0)]
        private AudioClip[] _footStepsOnExit;
        public AudioClip bgMusicBossDefeat;
        private bool _insideReset = true;

        [Header("Fog Walls")]
        public FogWallManager[] fogWalls;

        [Header("Boss Prefab")]
        public string bossName = "";
        public GameObject bossPrefab;
        public Vector3 startPosition;
        public Quaternion startRotation;
        public Transform parentTransform;
        public GameObject bossHpBar;
        public bool isBossAlive = true;

        private EnemyStats _bossStats;
        private Slider _bossHpSlider;
        private TextMeshProUGUI _bossNameText;

        private void Start()
        {
            if (isBossAlive)
            {
                bossPrefab = Instantiate(bossPrefab, startPosition, startRotation, parentTransform);
                bonfiresInArea[0].gameObject.SetActive(false);
                _bossStats = bossPrefab.GetComponent<EnemyStats>();
                _bossStats.bossAreaManager = this;
                bossHpBar.SetActive(false);
                _bossHpSlider = bossHpBar.GetComponentInChildren<Slider>();
                _bossNameText = bossHpBar.GetComponentInChildren<TextMeshProUGUI>();
            }
            else
            {
                if (bossPrefab != null)
                {
                    Destroy(bossPrefab.gameObject);
                }

                foreach (var fogWall in fogWalls)
                {
                    Destroy(fogWall.gameObject);
                }

                bonfiresInArea[0].gameObject.SetActive(true);
                bonfiresInArea[0].bonfireParticleSystem.Play();
                bonfiresInArea[0].bonfireLight.enabled = true;
            }
        }

        public void SetExitFootSteps(AudioClip[] footStep)
        {
            _footStepsOnExit = footStep;
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

                    if (isBossAlive)
                    {
                        _bossHpSlider.minValue = 0;
                        _bossHpSlider.maxValue = _bossStats.maxHealth;
                        _bossHpSlider.value = _bossStats.maxHealth;
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
                bossHpBar.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (playerStats != null)
            {
                if (playerStats.currentHealth <= 0)
                {
                    StartCoroutine(HealBoss());
                }
            }

            if (isBossAlive)
            {
                if (_bossStats.currentHealth <= 0)
                {
                    for (var i = 0; i < fogWalls.Length; i++)
                    {
                        Destroy(fogWalls[i].gameObject);
                    }

                    isBossAlive = false;
                    bonfiresInArea[0].gameObject.SetActive(true);
                    if (playerSoundManager != null)
                    {
                        playerSoundManager.ChangeBackGroundMusic(bgMusicBossDefeat);
                    }

                    areaBgMusic = bgMusicBossDefeat;
                }
            }
        }

        private IEnumerator ShowAreaName()
        {
            locationScreenText.text = areaName;
            locationScreen.SetActive(true);

            if (isBossAlive)
            {
                _bossNameText.text = bossName;
                bossHpBar.SetActive(true);
            }

            yield return CoroutineYielder.waitFor1HalfSecond;

            locationScreenText.text = "";
            locationScreen.SetActive(false);
        }

        private IEnumerator HealBoss()
        {
            bossHpBar.SetActive(false);
            fogWalls[0].canInteract = true;

            yield return CoroutineYielder.waitFor1Second;

            _bossStats.InitializeHealth();

            yield return CoroutineYielder.waitFor5Second;

            bossPrefab.transform.position = startPosition;
        }
    }
}