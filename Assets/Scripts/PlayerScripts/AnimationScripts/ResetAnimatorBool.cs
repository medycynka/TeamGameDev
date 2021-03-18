using UnityEngine;
using SzymonPeszek.Misc;


namespace SzymonPeszek.PlayerScripts.Animations
{
    /// <summary>
    /// Class for resetting Animator's bool parameters
    /// </summary>
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        [Header("Animator's Bool Reset", order = 0)] 
        [Header("Properties", order = 1)]
        public bool isEnemy;
        public string[] targetBools;
        public bool[] status;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isEnemy)
            {
                for (int i = 0; i < targetBools.Length; i++)
                {
                    animator.SetBool(StaticAnimatorIds.enemyAnimationIds[targetBools[i]], status[i]);
                }
            }
            else
            {
                for (int i = 0; i < targetBools.Length; i++)
                {
                    animator.SetBool(StaticAnimatorIds.animationIds[targetBools[i]], status[i]);
                }
            }

            //animator.SetBool(targetBool, status);
        }
    }
}