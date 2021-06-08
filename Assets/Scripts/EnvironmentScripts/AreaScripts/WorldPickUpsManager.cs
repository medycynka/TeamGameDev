using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.SaveScripts;
using UnityEngine;


namespace SzymonPeszek.Environment.Areas
{
    public class WorldPickUpsManager : MonoBehaviour
    {
        public static WorldPickUpsManager instance => _instance;

        public List<PickupIdentifier> pickUpsList;
        
        private static WorldPickUpsManager _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            } 
            else 
            {
                _instance = this;
            }
        }

        private void Start()
        {
            DataManager dataManager = SettingsHolder.dataManager;
            if (dataManager != null)
            {
                for (int i = 0; i < dataManager.pickUps.Length; i++)
                {
                    if (pickUpsList.Any(p => p.pickUpId == dataManager.pickUps[i].pickUpId))
                    {
                        PickupIdentifier tmp = pickUpsList.First(p => p.pickUpId == dataManager.pickUps[i].pickUpId);
                        if (dataManager.pickUps[i].isCollected)
                        {
                            Destroy(tmp.pickUp);
                        }
                    }
                }
            }
        }

        public PickUpsActivation[] SavePickUps()
        {
            PickUpsActivation[] returner = new PickUpsActivation[pickUpsList.Count];

            for (int i = 0; i < returner.Length; i++)
            {
                returner[i] = new PickUpsActivation
                {
                    pickUpId = pickUpsList[i].pickUpId,
                    isCollected = pickUpsList[i] == null
                };
            }

            return returner;
        }
    }

    [Serializable]
    public class PickupIdentifier
    {
        public string pickUpId;
        public GameObject pickUp;
    }
}