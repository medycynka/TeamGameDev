﻿using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for animation manager
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        [Header("Animation Manager", order = 0)]
        [Header("Animator", order = 1)]
        public Animator anim;
        public bool canRotate;
        public bool isEnemy;

        /// <summary>
        /// Play target animation
        /// </summary>
        /// <param name="targetAnim">Animation's name</param>
        /// <param name="isInteracting">isInteracting bool parameter in animator</param>
        /// <param name="enableRotation">canRotate bool parameter in animator</param>
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool enableRotation = false)
        {
            //anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanRotateName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.CanRotateName], enableRotation);
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(isEnemy ? StaticAnimatorIds.enemyAnimationIds[targetAnim] : StaticAnimatorIds.animationIds[targetAnim], 0.2f);
        }
        
        /// <summary>
        /// Play target animation
        /// </summary>
        /// <param name="targetAnim">Animation's hash code</param>
        /// <param name="isInteracting"></param>
        /// <param name="enableRotation">canRotate bool parameter in animator</param>
        public void PlayTargetAnimation(int targetAnim, bool isInteracting, bool enableRotation = false)
        {
            //anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.CanRotateName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.CanRotateName], enableRotation);
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
        
        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsRotatingWithRootMotionName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsRotatingWithRootMotionName], true);
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
        
        public void PlayTargetAnimationWithRootRotation(int targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsRotatingWithRootMotionName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsRotatingWithRootMotionName], true);
            anim.SetBool(isEnemy ? StaticAnimatorIds.enemyAnimationIds[StaticAnimatorIds.IsInteractingName] : StaticAnimatorIds.animationIds[StaticAnimatorIds.IsInteractingName], isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        /// <summary>
        /// Get back stabbed or riposted
        /// </summary>
        public virtual void TakeCriticalDamageAnimationEvent()
        {
            
        }
    }

}