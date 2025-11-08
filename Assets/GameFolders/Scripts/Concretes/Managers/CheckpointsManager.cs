// using Controllers;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using Combat;
// using Managers;

// public class CheckpointsManager : MonoBehaviour
// {
//     [SerializeField] Health _playerHealth;
//     CheckpointController[] _checkpoints;
//     StartpointController _startpoint;
//     private void Awake()
//     {
//         _startpoint = GetComponentInChildren<StartpointController>();
//         _checkpoints = GetComponentsInChildren<CheckpointController>();
//     }
//     private void OnEnable()
//     {
//         _playerHealth.OnDead += HandleOnDead;
//     }
//     public void HandleOnDead()
//     {
//         SoundManager.Instance.PlaySound(10);
//         if(_checkpoints.LastOrDefault(x => x.IsChecked) == null)
//             _playerHealth.transform.position = _startpoint.transform.position;
//         else
//         {
//             _playerHealth.transform.position = _checkpoints.LastOrDefault(x=>x.IsChecked).transform.position;
//         }  
//     }

// }
using Controllers;
using System.Linq;
using UnityEngine;
using Combat;
using Managers;

public class CheckpointsManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Health _player1Health;
    [SerializeField] private Health _player2Health;

    private CheckpointController[] _checkpoints;
    private StartpointController _startpoint;

    private void Awake()
    {
        _startpoint = FindObjectOfType<StartpointController>();
        _checkpoints = FindObjectsOfType<CheckpointController>();
    }

    private void OnEnable()
    {
        if (_player1Health != null)
            _player1Health.OnDead += HandlePlayer1Dead;

        if (_player2Health != null)
            _player2Health.OnDead += HandlePlayer2Dead;
    }

    private void OnDisable()
    {
        if (_player1Health != null)
            _player1Health.OnDead -= HandlePlayer1Dead;

        if (_player2Health != null)
            _player2Health.OnDead -= HandlePlayer2Dead;
    }

    private void HandlePlayer1Dead() => HandleOnDead(_player1Health);
    private void HandlePlayer2Dead() => HandleOnDead(_player2Health);

    private void HandleOnDead(Health playerHealth)
    {
        if (playerHealth == null) return;
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(10);

        if (_startpoint == null)
        {
            Debug.LogError("No StartpointController found in scene!");
            return;
        }

        var lastCheckpoint = _checkpoints?.LastOrDefault(x => x.IsChecked);

        if (lastCheckpoint == null)
            playerHealth.transform.position = _startpoint.transform.position;
        else
            playerHealth.transform.position = lastCheckpoint.transform.position;
    }
}
