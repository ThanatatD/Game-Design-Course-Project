// using Combat;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// namespace UIs
// {
//     public class FilledHeartsImages : MonoBehaviour
//     {
//         [SerializeField] Health _playerHealth;
//         Image[] _filledHearts;

//         private void Awake()
//         {
//             _filledHearts = GetComponentsInChildren<Image>();
//         }
//         private void OnEnable()
//         {
//             _playerHealth.OnHealthChanged += HandleHealthChanged;
//             _playerHealth.OnDead += HandleOnDead;
//         }
//         private void Start()
//         {
//             HandleHealthChanged();
//         }

//         private void HandleOnDead()
//         {
//             for(int i=0; i< _playerHealth.MaxHealth;i++)
//             {
//                 _filledHearts[i].gameObject.SetActive(true);
//             }
//         }

//         private void HandleHealthChanged()
//         {
//             if (_filledHearts.Length > _playerHealth.CurrentHealth)
//             {
//                 int deleteCount = _filledHearts.Length - _playerHealth.CurrentHealth;
//                 for (int i = 1; i < deleteCount + 1; i++)
//                 {
//                     _filledHearts[_filledHearts.Length - i].gameObject.SetActive(false);
//                 }
//             }
//         }
//     }

// }

using Combat;
using UnityEngine;
using UnityEngine.UI;

namespace UIs
{
    public class FilledHeartsImages : MonoBehaviour
    {
        [SerializeField] private Health[] _players; // assign both players
        private Health _sharedHealth;
        private Image[] _filledHearts;

        private void Awake()
        {
            _filledHearts = GetComponentsInChildren<Image>();

            // Auto-find all Health components if none assigned
            if (_players == null || _players.Length == 0)
                _players = FindObjectsOfType<Health>();

            if (_players.Length > 0)
                _sharedHealth = _players[0]; // reference shared health
        }

        private void OnEnable()
        {
            if (_players == null) return;

            // Subscribe to OnHealthChanged for BOTH players
            foreach (var player in _players)
            {
                player.OnHealthChanged += HandleHealthChanged;
                player.OnDead += HandleOnDead;
            }

            HandleHealthChanged(); // initial update
        }

        private void OnDisable()
        {
            if (_players == null) return;

            foreach (var player in _players)
            {
                player.OnHealthChanged -= HandleHealthChanged;
                player.OnDead -= HandleOnDead;
            }
        }

        private void HandleOnDead()
        {
            for (int i = 0; i < _sharedHealth.MaxHealth; i++)
                _filledHearts[i].gameObject.SetActive(true);
        }

        private void HandleHealthChanged()
        {
            int health = _sharedHealth.CurrentHealth;
            for (int i = 0; i < _filledHearts.Length; i++)
                _filledHearts[i].gameObject.SetActive(i < health);
        }
    }
}
