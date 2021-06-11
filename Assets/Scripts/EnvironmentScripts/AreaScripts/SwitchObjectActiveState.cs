using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SzymonPeszek.Environment
{
    public class SwitchObjectActiveState : MonoBehaviour
    {
        public string objectId;
        public bool currentActiveState = true;
        public GameObject objectToSwitch;

        public static Dictionary<string, SwitchObjectActiveState> activeObjectsMap;

        private void Awake()
        {
            activeObjectsMap ??= new Dictionary<string, SwitchObjectActiveState>();

            if (!activeObjectsMap.ContainsKey(objectId))
            {
                activeObjectsMap.Add(objectId, this);
            }
        }

        private void OnDestroy()
        {
            if (activeObjectsMap != null)
            {
                if (activeObjectsMap.ContainsKey(objectId))
                {
                    activeObjectsMap.Remove(objectId);
                }
            }
        }

        private void Start()
        {
            objectToSwitch.SetActive(currentActiveState);
        }

        public void SwitchActiveState()
        {
            currentActiveState = !currentActiveState;
            objectToSwitch.SetActive(currentActiveState);
        }
    }
}