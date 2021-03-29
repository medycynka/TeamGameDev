using UnityEngine;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Environment.Areas
{
    /// <summary>
    /// class for killing player after falling
    /// </summary>
    public class DeathJumpZone : MonoBehaviour
    {
        public bool isInside;
        public Transform dropPosition;
        
        private PlayerStats _playerStats;
        private bool _insideReset = true;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInside = true;

                if (_insideReset)
                {
                    if (_playerStats == null)
                    {
                        _playerStats = other.GetComponent<PlayerStats>();
                    }

                    _playerStats.isJumpDeath = true;
                    _playerStats.jumpDeathDropPosition = dropPosition.position;
                    _playerStats.TakeDamage(2000f);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (isInside && _insideReset)
            {
                _insideReset = false;
            }
        }
    }
}