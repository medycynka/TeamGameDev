using System.Collections;
using UnityEngine;
using SzymonPeszek.Misc;
using SzymonPeszek.SaveScripts;
using SzymonPeszek.Environment.Areas;


namespace SzymonPeszek.Environment.Sounds
{
    /// <summary>
    /// Class for managing sounds
    /// </summary>
    public class AnimationSoundManager : MonoBehaviour
    {
        [Header("Audio Clips", order = 0)]
        [Header("Multiple Clips For Random Pick", order = 1)]
        public AudioClip[] movingClips;
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

        [Header("Music Fade In/Out")]
        public float musicFadeIn = 2f;
        public float musicFadeOut = 2f;
        public bool fadingMusic;
        
        private AudioSource _audioSource;
        private Animator _anim;
        private bool _playFootsteps = true;
        private float _currTime;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _anim = GetComponentInChildren<Animator>();
            _audioSource.loop = true;
            _audioSource.clip = currentBackgroundMusic;
            _audioSource.volume = SettingsHolder.soundVolume;
            //previouseBackgroundMusic = currentBackgroundMusic;
            _audioSource.Play();
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
            areaManager.SetExitFootSteps(movingClips);
            
            if (newFootSteps.Length > 0)
            {
                movingClips = newFootSteps;
            }
        }

        public void ChangeFootstepsSound(AudioClip[] newFootSteps, BossAreaManager areaManager)
        {
            areaManager.SetExitFootSteps(movingClips);
            
            if (newFootSteps.Length > 0)
            {
                movingClips = newFootSteps;
            }
        }

        #region Play For Animation
        public void PlayOnStep()
        {
            if (movingClips.Length > 0 && _playFootsteps)
            {
                if (!_anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName]) &&
                    !_anim.GetBool(StaticAnimatorIds.animationIds[StaticAnimatorIds.IsDeadName]))
                {
                    _audioSource.PlayOneShot(GetRandomClip(movingClips));
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
            if (movingClips.Length > 0)
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

        private IEnumerator StopStepSounds()
        {
            DisableFootStepsSound();

            yield return CoroutineYielder.waitFor1Second;
            
            EnableFootStepsSound();
        }
    }
}