using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.Misc;
using SzymonPeszek.SaveScripts;
using SzymonPeszek.Environment.Areas;
using Random = UnityEngine.Random;


namespace SzymonPeszek.Environment.Sounds
{
    /// <summary>
    /// Class for managing sounds
    /// </summary>
    public class AnimationSoundManager : MonoBehaviour
    {
        [Serializable]
        public struct EnvironmentFootstepSound
        {
            public string tagName;
            public AudioClip[] footsteps;
            public GameObject stepEffect;
        }

        [Header("Audio Clips", order = 0)] 
        [Header("Volume Variance", order = 1)]
        [Range(0, 0.5f)] public float volumeVariance = 0.1f;
        
        [Header("Multiple Clips For Random Pick", order = 1)]
        public EnvironmentFootstepSound[] environmentFootstepSounds;
        public AudioClip[] currentMovingClips;
        public AudioClip[] attackingClips;
        public AudioClip[] getDamageClips;
        public AudioClip[] faithSkillClips;
        public AudioClip[] curseSkillClips;
        public AudioClip[] destructionSkillClips;

        [Header("Unique Clips", order = 1)]
        public AudioClip rollClip;
        public AudioClip backStepClip;
        public AudioClip estusUse;
        public AudioClip soulUse;

        [Header("Current Background Music Clip", order = 1)]
        public AudioClip currentBackgroundMusic;
        //public AudioClip previouseBackgroundMusic;

        [Header("Music Fade In/Out", order = 1)]
        public float musicFadeIn = 2f;
        public float musicFadeOut = 2f;
        public bool fadingMusic;
        
        [Header("Feet transforms", order = 1)]
        public Transform leftFoot;
        public Transform rightFoot;
        
        private AudioSource _audioSource;
        private Animator _anim;
        private bool _playFootsteps = true;
        private string _currentEnvironmentTag;
        private RaycastHit _hit;
        private GameObject _terrainFinder;
        private Terrain _terrain;
        private TerrainData _terrainData;
        private Vector3 _terrainPos;
        private static int _surfaceIndex = 0;
        private string _whatTexture;
        private static GameObject _floor;
        private string _currentFoot = "right";
        private Transform _currentFootTransform;
        private LayerMask _environmentMask;
        private Dictionary<string, EnvironmentFootstepSound> _stepSounds;
        private List<GameObject> _stepEffects = new List<GameObject>();
        private float _stepDeletionTimer = 3f;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _anim = GetComponentInChildren<Animator>();
            _audioSource.loop = true;
            _audioSource.clip = currentBackgroundMusic;
            _audioSource.volume = SettingsHolder.soundVolume;
            _environmentMask = 1 << LayerMask.NameToLayer("Environment") | 1 << LayerMask.NameToLayer("Water");
            _audioSource.Play();

            _stepSounds = new Dictionary<string, EnvironmentFootstepSound>();

            if (environmentFootstepSounds.Length > 0)
            {
                for (int i = 0; i < environmentFootstepSounds.Length; i++)
                {
                    _stepSounds.Add(environmentFootstepSounds[i].tagName, environmentFootstepSounds[i]);
                }

                _currentEnvironmentTag = "Environment";
                currentMovingClips = _stepSounds[_currentEnvironmentTag].footsteps;
            }

            // Check if there is a terrain
            _terrainFinder = GameObject.FindGameObjectWithTag("Terrain");
            
            if (_terrainFinder != null) 
            {
                _terrain = Terrain.activeTerrain;
                _terrainData = _terrain.terrainData;
                _terrainPos = _terrain.transform.position;
            }
        }

        private void Update()
        {
            UpdateMovingClips();
            
            _stepDeletionTimer -= Time.deltaTime;

            if (_stepDeletionTimer <= 0)
            {
                if (_stepEffects.Count > 4)
                {
                    for (int i = _stepEffects.Count - 1; i >= 0; i--)
                    {
                        Destroy(_stepEffects[i]);
                        _stepEffects.RemoveAt(i);
                    }
                }

                _stepDeletionTimer = 3f;
            }
        }

        public void ChangeBackGroundMusic(AudioClip newBgMusic)
        {
            currentBackgroundMusic = newBgMusic;
            _audioSource.clip = currentBackgroundMusic;
            _audioSource.volume = SettingsHolder.soundVolume;
            _audioSource.Play();
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, AreaManager areaManager)
        {
            areaManager.SetExitFootSteps(currentMovingClips);
            
            if (newFootSteps.Length > 0)
            {
                currentMovingClips = newFootSteps;
            }
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, BossAreaManager areaManager)
        {
            areaManager.SetExitFootSteps(currentMovingClips);
            
            if (newFootSteps.Length > 0)
            {
                currentMovingClips = newFootSteps;
            }
        }

        #region Play For Animation
        public void PlayOnStep()
        {
            if (_currentFoot == "left")
            {
                FootStepLeft();
            }
            else
            {
                FootStepRight();
            }
            
            if (!_anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName]) &&
                !_anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName]))
            {
                if (currentMovingClips.Length > 0 && _playFootsteps)
                {
                    _audioSource.PlayOneShot(GetRandomClip(currentMovingClips),
                        Mathf.Clamp01(_audioSource.volume + Random.Range(-volumeVariance, volumeVariance)));
                    
                    if (_currentFoot == "left")
                    {
                        _stepEffects.Insert(0, Instantiate(_stepSounds[_currentEnvironmentTag].stepEffect,
                            leftFoot.position + new Vector3(0, 0.01f, 0), leftFoot.rotation));
                    } 
                    else
                    {
                        _stepEffects.Insert(0, Instantiate(_stepSounds[_currentEnvironmentTag].stepEffect,
                            rightFoot.position + new Vector3(0, 0.01f, 0), rightFoot.rotation));
                    }
                }
            }
        }

        public void PlayOnRoll()
        {
            if (rollClip != null)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(rollClip);
            }
        }

        public void PlayOnBackStep()
        {
            if (backStepClip != null)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(backStepClip);
            }
        }

        public void PlayOnAttack()
        {
            if (attackingClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(GetRandomClip(attackingClips));
            }
        }
        
        public void PlayOnFaith()
        {
            if (faithSkillClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(GetRandomClip(faithSkillClips));
            }
        }
        
        public void PlayOnCurse()
        {
            if (curseSkillClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(GetRandomClip(curseSkillClips));
            }
        }
        
        public void PlayOnDestruction()
        {
            if (destructionSkillClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(GetRandomClip(destructionSkillClips));
            }
        }

        public void PlayOnDamage()
        {
            if (getDamageClips.Length > 0)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(GetRandomClip(getDamageClips));
            }
        }

        public void PlayOnEstusUse()
        {
            if (estusUse != null)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(estusUse);
            }
        }

        public void PlayOnSoulUse()
        {
            if (soulUse != null)
            {
                StartCoroutine(StopStepSounds());
                _audioSource.PlayOneShot(soulUse);
            }
        }
        #endregion

        public void EnableFootStepsSound()
        {
            _playFootsteps = true;
        }
        
        public void DisableFootStepsSound()
        {
            _playFootsteps = false;
        }
        
        private static AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }

        private void UpdateMovingClips()
        {
            if (_terrainFinder != null) 
            {
                _surfaceIndex = GetMainTexture (transform.position);
                //If you added a grass texture, then a dirt, then a rock, you'd have grass=0, dirt=1, rock=2.
                _whatTexture = _terrainData.terrainLayers[_surfaceIndex].name;
            }
        }
        
        #region Detect ground type
        private float[] GetTextureMix(Vector3 worldPos)
        {
            if (_terrainFinder != null) 
            {
                int mapX = (int)(((worldPos.x - _terrainPos.x) / _terrainData.size.x) * _terrainData.alphamapWidth);
                int mapZ = (int)(((worldPos.z - _terrainPos.z) / _terrainData.size.z) * _terrainData.alphamapHeight);
                float[,,] splatmapData = _terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
                float[] cellMix = new float[splatmapData.GetUpperBound (2) + 1 ];
                
                for (int n = 0; n < cellMix.Length; n++) 
                {
                    cellMix[n] = splatmapData[0, 0, n];
                }
                
                return cellMix;
            }

            return null;
        }

        private int GetMainTexture(Vector3 worldPos)
        {
            if (_terrainFinder != null) 
            {
                float[] mix = GetTextureMix(worldPos);
                float maxMix = 0;
                int maxIndex = 0;
                
                for (int n = 0; n < mix.Length; n++)
                {
                    if (mix[n] > maxMix)
                    {
                        maxIndex = n;
                        maxMix = mix[n];
                    }
                }
                
                return maxIndex;
            }
            
            return 0;
        }

        private void FootStepLeft()
        {
            if (Physics.Raycast(new Ray(leftFoot.position + new Vector3(0, 1.5f, 0), Vector3.down), 
                out _hit, 2f, _environmentMask))
            {
                _floor = _hit.transform.gameObject;
            }

            _currentFoot = "left";

            if (_floor != null)
            {
                CheckTexture();
            }
        }

        private void FootStepRight()
        {
            if (Physics.Raycast(new Ray(rightFoot.position + new Vector3(0, 1.5f, 0), Vector3.down), 
                out _hit, 2f, _environmentMask))
            {
                _floor = _hit.transform.gameObject;
                _currentFootTransform = _hit.transform;
            }

            _currentFoot = "right";

            if (_floor != null)
            {
                CheckTexture();
            }
        }

        private void CheckTexture()
        {
            if (_floor.CompareTag(_currentEnvironmentTag))
            {
                return;
            }

            if (_floor.CompareTag("Terrain"))
            {
                for (int i = 0; i < environmentFootstepSounds.Length; i++)
                {
                    string groundTag = environmentFootstepSounds[i].tagName;

                    if (_whatTexture.Contains(groundTag) || _whatTexture.Contains(groundTag.ToLower()) ||
                        _whatTexture.Contains(groundTag.ToUpper()))
                    {
                        _currentEnvironmentTag = environmentFootstepSounds[i].tagName;
                        currentMovingClips = environmentFootstepSounds[i].footsteps;
                        
                        return;
                    }
                }
            }

            if (_stepSounds.ContainsKey(_floor.tag))
            {
                _currentEnvironmentTag = _stepSounds[_floor.tag].tagName;
                currentMovingClips = _stepSounds[_floor.tag].footsteps;
            }
        }
        #endregion

        private IEnumerator StopStepSounds()
        {
            DisableFootStepsSound();

            yield return CoroutineYielder.waitFor1Second;
            
            EnableFootStepsSound();
        }
    }
}