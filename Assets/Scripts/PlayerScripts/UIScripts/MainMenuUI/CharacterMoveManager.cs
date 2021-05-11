using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace SzymonPeszek.MainMenuUI
{
    /// <summary>
    /// Class which allows player to rotate character during character's creation
    /// </summary>
    public class CharacterMoveManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
    {
        public Transform characterTransform;
        [Range(5f, 25f)]public float rotationSpeed = 15f;
        [Range(0.0005f, 0.025f)] public float moveSpeed = 0.005f;
        [Range(0.01f, 0.1f)] public float scrollSpeed = 0.025f;
        [Range(1f, 5f)] public float maxZoom = 2.5f;

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
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                float rotX = eventData.delta.x * rotationSpeed * Mathf.Deg2Rad;
                characterTransform.Rotate(Vector3.up, -rotX);
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Vector3 currentPos = characterTransform.position;
                float moveX = eventData.delta.x * moveSpeed;
                float moveY = eventData.delta.y * moveSpeed;
                characterTransform.position = new Vector3(currentPos.x + moveX, currentPos.y + moveY, currentPos.z);
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            Vector3 delta = Vector3.one * (eventData.scrollDelta.y * scrollSpeed);
            Vector3 zoom = characterTransform.localScale + delta;

            zoom = Vector3.Max(_beginScale, zoom);
            zoom = Vector3.Min(_beginScale * maxZoom, zoom);
            
            characterTransform.localScale = zoom;
        }
    }
}