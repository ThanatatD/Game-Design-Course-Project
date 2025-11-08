using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat; // For Energy component
using Managers;

namespace Controllers
{
    public class EnergyGain : MonoBehaviour
    {
        [SerializeField] private float _energyValue = 100f; // default energy gain
        private Animator _anim;
        private bool _isCollected;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !_isCollected)
            {
                // Try to get Energy component from player
                Energy playerEnergy = collision.GetComponent<Energy>();
                if (playerEnergy != null)
                {
                    playerEnergy.ModifyEnergy(_energyValue);
                }

                // Play sound
                SoundManager.Instance.PlaySound(5);

                // Play collected animation
                if (_anim != null)
                    _anim.Play("Collected");

                _isCollected = true;

                // Destroy after animation
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
