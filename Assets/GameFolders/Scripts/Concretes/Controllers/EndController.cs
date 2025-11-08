// using Managers;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Controllers
// {
//     public class EndController : MonoBehaviour
//     {
//         private void OnTriggerEnter2D(Collider2D collision)
//         {
//             GameManager.Instance.EndGame();
//             SoundManager.Instance.StopAllSounds();
//             SoundManager.Instance.PlaySound(13);
//             SoundManager.Instance.PlaySound(14);
//         }
//     }

// }


using Managers;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Controllers
{
    public class EndController : MonoBehaviour
    {
        [Tooltip("Drag the TextMeshPro (3D Text) GameObject here.")]
        [SerializeField] private GameObject playerCountTextObject;
        private TextMeshPro playerCountText;

        private HashSet<PlayerType> playersInZone = new HashSet<PlayerType>();

        private void Awake()
        {
            if (playerCountTextObject != null)
            {
                playerCountText = playerCountTextObject.GetComponent<TextMeshPro>();
                if (playerCountText == null)
                    Debug.LogWarning("[EndController] Assigned object does not have TextMeshPro component.", playerCountTextObject);
            }
            else
            {
                Debug.LogWarning("[EndController] playerCountTextObject not assigned in inspector.");
            }
        }

        private void Start()
        {
            UpdatePlayerCountUI();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player == null) return;

            bool added = playersInZone.Add(player.playerType);
            if (added)
            {
                Debug.Log($"[EndController] {player.playerType} entered zone.");
                UpdatePlayerCountUI();
            }

            if (playersInZone.Contains(PlayerType.Player1) && playersInZone.Contains(PlayerType.Player2))
            {
                EndGame();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player == null) return;

            bool removed = playersInZone.Remove(player.playerType);
            if (removed)
            {
                Debug.Log($"[EndController] {player.playerType} left zone.");
                UpdatePlayerCountUI();
            }
        }

        private void UpdatePlayerCountUI()
        {
            if (playerCountText == null) return;

            int count = playersInZone.Count;
            playerCountText.text = $"{count}/2";
        }

        private void EndGame()
        {
            if (playerCountTextObject != null)
                playerCountTextObject.SetActive(false);

            Debug.Log("[EndController] Both players in zone — ending game.");
            GameManager.Instance.EndGame();
            SoundManager.Instance.StopAllSounds();
            SoundManager.Instance.PlaySound(13);
            SoundManager.Instance.PlaySound(14);
        }
    }
}
