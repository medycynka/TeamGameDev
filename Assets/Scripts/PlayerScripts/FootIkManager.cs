using UnityEngine;

namespace SzymonPeszek.PlayerScripts
{
    /// <summary>
    /// Class which manages foot IK placement
    /// </summary>
    public class FootIkManager : MonoBehaviour
    {
        private Animator _anim;
        private Vector3 _rightFootPosition;
        private Vector3 _leftFootPosition;
        private Vector3 _rightFootIkPosition;
        private Vector3 _leftFootIkPosition;
        private Quaternion _leftFootIkRotation;
        private Quaternion _rightFootIkRotation;
        private float _lastPelvisPositionY;
        private float _lastRightFootPositionY;
        private float _lastLeftFootPositionY;

        [Header("Foot IK Manager", order = 0)] 
        [Header("Feet Grounded Variables", order = 1)]
        public bool enableFeetIk = true;
        public LayerMask environmentLayer;
        [Range(0f, 1f)] public float weightPositionRight = 1f;
        [Range(0f, 1f)] public float weightRotationRight;
        [Range(0f, 1f)] public float weightPositionLeft = 1f;
        [Range(0f, 1f)] public float weightRotationLeft;
        public Vector3 offsetFoot;
        
        private const string EnvironmentTag = "Environment";
        private const AvatarIKGoal LeftFoot = AvatarIKGoal.LeftFoot;
        private const AvatarIKGoal RightFoot = AvatarIKGoal.RightFoot;
        private RaycastHit _hit;
        
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            environmentLayer = 1 << LayerMask.NameToLayer(EnvironmentTag);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (enableFeetIk)
            {
                #region Right Foot IK
                Vector3 footPos = _anim.GetIKPosition(RightFoot);

                if (Physics.Raycast(footPos + Vector3.up, Vector3.down, out _hit, 1.2f, environmentLayer))
                {
                    _anim.SetIKPositionWeight(RightFoot, weightPositionRight);
                    _anim.SetIKRotationWeight(RightFoot, weightRotationRight);
                    _anim.SetIKPosition(RightFoot, _hit.point + offsetFoot);

                    if (weightRotationRight > 0f)
                    {
                        Quaternion footRotation =
                            Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, _hit.normal),
                                _hit.normal);
                        _anim.SetIKRotation(RightFoot, footRotation);
                    }
                }
                else
                {
                    _anim.SetIKPositionWeight(RightFoot, 0f);
                    _anim.SetIKRotationWeight(RightFoot, 0f);
                }
                #endregion

                #region Left Foot IK
                footPos = _anim.GetIKPosition(LeftFoot);
                if (Physics.Raycast(footPos + Vector3.up, Vector3.down, out _hit, 1.2f, environmentLayer))
                {
                    _anim.SetIKPositionWeight(LeftFoot, weightPositionLeft);
                    _anim.SetIKRotationWeight(LeftFoot, weightRotationLeft);
                    _anim.SetIKPosition(LeftFoot, _hit.point + offsetFoot);

                    if (weightRotationLeft > 0f)
                    {
                        Quaternion footRotation =
                            Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, _hit.normal),
                                _hit.normal);
                        _anim.SetIKRotation(LeftFoot, footRotation);
                    }
                }
                else
                {
                    _anim.SetIKPositionWeight(LeftFoot, 0f);
                    _anim.SetIKRotationWeight(LeftFoot, 0f);
                }
                #endregion
            }
        }
    }
}