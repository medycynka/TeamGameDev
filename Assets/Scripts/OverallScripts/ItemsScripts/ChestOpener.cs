using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Items.Chests
{
    public class ChestOpener : Interactable
    {
        public Transform playerStandingPosition;
        public GameObject miscPickUpPrefab;
        public List<Item> chestPickUps;
        private Animator _animator;
        private ChestOpener _chestOpener;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            StaticAnimatorIds.chestOpenId = Animator.StringToHash(StaticAnimatorIds.ChestOpenName);
            _chestOpener = GetComponent<ChestOpener>();
            miscPickUpPrefab.GetComponent<MiscPickUp>().items = chestPickUps;
        }

        public override void Interact(PlayerManager playerManager)
        {
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            playerManager.OpenChestInteraction(playerStandingPosition);
            _animator.Play(StaticAnimatorIds.chestOpenId);
            StartCoroutine(SpawnItemInChest());
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return CoroutineYielder.waitFor4Second;
            
            Instantiate(miscPickUpPrefab, transform);
            Destroy(_chestOpener);
        }
    }
}