using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace SzymonPeszek.MainMenuUI
{
    /// <summary>
    /// Class which allows player to rotate character during character's creation
    /// </summary>
    public class CharacterMoveManager : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public Transform characterTransform;
        public float rotationSpeed = 15f;
        public float scrollSpeed = 0.05f;
        public float maxZoom = 10f;

        private Vector3 _beginScale;

        private void Awake()
        {
            _beginScale = characterTransform.localScale;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
            {
                return;
            }

            float rotX = eventData.delta.x * rotationSpeed * Mathf.Deg2Rad;
            characterTransform.Rotate(Vector3.up, -rotX);
        }
    }
}