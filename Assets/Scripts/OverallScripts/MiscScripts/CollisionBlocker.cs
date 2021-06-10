﻿using UnityEngine;


namespace SzymonPeszek.Misc
{
    /// <summary>
    /// Class for blocking characters pushing each other
    /// </summary>
    public class CollisionBlocker : MonoBehaviour
    {
        public Collider characterCollider;
        public CapsuleCollider blockerCollider;

        void Start()
        {
            Physics.IgnoreCollision(characterCollider, blockerCollider, true);
        }
    }
}