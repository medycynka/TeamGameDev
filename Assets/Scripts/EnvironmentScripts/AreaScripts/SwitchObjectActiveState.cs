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

        private void Awake()
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