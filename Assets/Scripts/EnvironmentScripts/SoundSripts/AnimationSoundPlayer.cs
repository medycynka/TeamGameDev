using System.Collections;
using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Environment.Sounds 
{
    /// <summary>
    /// Helper class for playing footsteps, attack sound, etc during animation
    /// </summary>
    public class AnimationSoundPlayer : MonoBehaviour
    {
        private AnimationSoundManager _animationSoundManager;

        public bool canPlayStep = true;

        private void Awake()
        {
            _animationSoundManager = GetComponentInParent<AnimationSoundManager>();
        }

        public void StepSound()
        {
            if (canPlayStep)
            {
                _animationSoundManager.PlayOnStep();
            }
        }

        public void RollSound()
        {
            _animationSoundManager.PlayOnRoll();
        }

        public void BackStep()
        {
            _animationSoundManager.PlayOnBackStep();
        }

        public void AttackSound()
        {
            _animationSoundManager.PlayOnAttack();
        }

        public void FaithSound()
        {
            _animationSoundManager.PlayOnFaith();
        }
        
        public void CurseSound()
        {
            _animationSoundManager.PlayOnCurse();
        }
        
        public void DestructionSound()
        {
            _animationSoundManager.PlayOnDestruction();
        }

        public void DamageSound()
        {
            _animationSoundManager.PlayOnDamage();
        }

        public void EstusSound()
        {
            _animationSoundManager.PlayOnEstusUse();
        }

        public void SoulSound()
        {
            _animationSoundManager.PlayOnSoulUse();
        }

        public void EnableSteps()
        {
            _animationSoundManager.EnableFootStepsSound();
        }

	    public void EnableStepsAfterTime()
        {
            StartCoroutine(EnableStepsAfterSecond());
        }
        
        public void DisableSteps()
        {
            _animationSoundManager.DisableFootStepsSound();
        }

	    private IEnumerator EnableStepsAfterSecond()
        {
            yield return CoroutineYielder.waitFor1Second;

		    _animationSoundManager.EnableFootStepsSound();
	    }
    }
}