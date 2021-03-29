using SzymonPeszek.PlayerScripts.Inventory;
using UnityEngine;

namespace SzymonPeszek.Misc.ColliderManagers
{
    public class BlockManager : MonoBehaviour
    {
        public BlockingCollider blockingCollider;
        
        private PlayerInventory _playerInventory;

        private void Awake()
        {
            _playerInventory = GetComponentInParent<PlayerInventory>();
        }

        public void OpenBlockingCollider(bool isTwoHanded)
        {
            blockingCollider.SetColliderDamageAbsorption(isTwoHanded
                ? _playerInventory.rightWeapon
                : _playerInventory.leftWeapon);

            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}