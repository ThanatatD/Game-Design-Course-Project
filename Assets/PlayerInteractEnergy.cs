using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat; // Health is in this namespace

namespace Mechanics
{
    public class PlayerInteractEnergy : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private int transferAmount = 20;      // amount to transfer
        [SerializeField] private string playerTag = "Player";  // tag for players (change if different)

        private Health _myHealth;      // this player's Health (found at Awake)
        private Health _otherPlayer;   // the Health of the nearby player we can transfer to/from

        private void Awake()
        {
            // Try to find the Health component in this object or its parents (flexible)
            _myHealth = GetComponent<Health>() ?? GetComponentInParent<Health>() ?? GetComponentInChildren<Health>();

            if (_myHealth == null)
                Debug.LogWarning($"{name} PlayerInteractEnergy: no Health component found on this player or parents/children.");
        }

        private void Update()
        {
            if (_otherPlayer != null && Input.GetKeyDown(interactKey))
            {
                TryTransferEnergy();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TrySetOtherFromCollider(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // in case Health is assigned later, try again
            if (_otherPlayer == null)
                TrySetOtherFromCollider(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // if exiting the collider that we assigned, clear
            Health h = collision.GetComponent<Health>() ?? collision.GetComponentInParent<Health>() ?? collision.GetComponentInChildren<Health>();
            if (h != null && h == _otherPlayer)
                _otherPlayer = null;
        }

        private void TrySetOtherFromCollider(Collider2D collision)
        {
            if (!collision.CompareTag(playerTag)) return; // only consider objects tagged "Player"
            if (collision.gameObject == gameObject) return; // ignore self-collisions

            // search for Health on the collider, then parent, then children
            Health h = collision.GetComponent<Health>() ?? collision.GetComponentInParent<Health>() ?? collision.GetComponentInChildren<Health>();
            if (h != null && h != _myHealth)
            {
                _otherPlayer = h;
                // optional debug
                // Debug.Log($"{name}: detected player {_otherPlayer.name} for energy transfer.");
            }
        }

        private void TryTransferEnergy()
        {
            if (_myHealth == null || _otherPlayer == null) return;

            int myEnergy = _myHealth.CurrentHealth;
            int otherEnergy = _otherPlayer.CurrentHealth;

            // who has more?
            if (myEnergy > otherEnergy && myEnergy >= transferAmount)
            {
                TransferEnergy(_myHealth, _otherPlayer, transferAmount);
            }
            else if (otherEnergy > myEnergy && otherEnergy >= transferAmount)
            {
                TransferEnergy(_otherPlayer, _myHealth, transferAmount);
            }
            else
            {
                Debug.Log("Not enough energy to transfer or energies equal.");
            }
        }

        private void TransferEnergy(Health giver, Health receiver, int amount)
        {
            // Ensure Health has ModifyHealth method (see below)
            giver.ModifyHealth(-amount);
            receiver.ModifyHealth(+amount);

            // Optional: feedback
            Debug.Log($"{giver.name} transferred {amount} energy to {receiver.name}");
        }
    }
}
