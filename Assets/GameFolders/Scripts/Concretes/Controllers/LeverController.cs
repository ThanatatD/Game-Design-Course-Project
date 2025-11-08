// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Managers;

// namespace Controllers
// {
//     public class LeverController : MonoBehaviour
//     {
//         [SerializeField] DoorController _door;
//         [SerializeField] GameObject _checkMark;
//         [SerializeField] GameObject _leverFruits;
//         Animator _anim;
//         bool IsLeverOn;
//         bool CanLeverWork;
//         private void Awake()
//         {
//             _anim = GetComponent<Animator>();
//         }
//         public void LeverInteraction()
//         {
//             if (!CanLeverWork)
//             {
//                 TryActivateLever();
                
//             }
//             else
//                 TriggerLever();
//         }
//         private void TryActivateLever()
//         {
//             if (CheckConditions())
//             {
//                 CanLeverWork = true;
//                 FruitManager.Instance.DecreaseFruitNumber(_door.DoorFruitType, _door.DoorFruitNumber);
//                 TriggerLever();
//                 _checkMark.SetActive(true); 
//                 if(_leverFruits != null) _leverFruits.SetActive(false);
//             }
//             else
//                 SoundManager.Instance.PlaySound(7);
//         }
//         private void TriggerLever()
//         {
//             SoundManager.Instance.PlaySound(6);
//             if (IsLeverOn)
//                 LeverOff();
//             else
//                 LeverOn();

//         }
//         private void LeverOn()
//         {
//             IsLeverOn = true;
//             _anim.SetBool("IsActive", true);
//             _door.OpenDoor();
//         }
//         private void LeverOff()
//         {
//             IsLeverOn = false;
//             _anim.SetBool("IsActive", false);
//             _door.CloseDoor();
//         }
//         private bool CheckConditions()
//         {
//             return FruitManager.Instance.AreThereEnoughFruit(_door.DoorFruitType, _door.DoorFruitNumber);
//         }
//     }

// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Combat; // ✅ for PlayerEnergy reference

namespace Controllers
{
    public class LeverController : MonoBehaviour
    {
        [Header("Doors Controlled by This Lever")]
        [SerializeField] private List<DoorController> _doors = new List<DoorController>();

        [Header("Lever Visuals")]
        [SerializeField] private GameObject _checkMark;
        [SerializeField] private GameObject _leverFruits;

        [Header("Energy Settings")]
        [SerializeField] private float _energyCost = 5f; // ✅ default energy cost per lever use

        private Animator _anim;
        private bool IsLeverOn;
        private bool CanLeverWork;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void LeverInteraction(GameObject player)
        {
            if (!CanLeverWork)
            {
                TryActivateLever(player);
            }
            else
            {
                TriggerLever(player);
            }
        }

        private void TryActivateLever(GameObject player)
        {
            if (CheckConditions())
            {
                CanLeverWork = true;

                // Decrease fruits
                foreach (var door in _doors)
                {
                    if (door != null)
                        FruitManager.Instance.DecreaseFruitNumber(door.DoorFruitType, door.DoorFruitNumber);
                }

                TriggerLever(player);

                if (_checkMark != null)
                    _checkMark.SetActive(true);

                if (_leverFruits != null)
                    _leverFruits.SetActive(false);
            }
            else
            {
                SoundManager.Instance.PlaySound(7);
            }
        }

        private void TriggerLever(GameObject player)
        {
            // ✅ Get player's energy
            var energy = player.GetComponent<Energy>();
            if (energy == null)
            {
                Debug.LogWarning($"{player.name} has no Energy component — cannot use lever.");
                return;
            }

            // ✅ Check if enough energy
            if (energy.CurrentEnergy < _energyCost)
            {
                Debug.Log($"{player.name} does not have enough energy to activate lever.");
                SoundManager.Instance.PlaySound(7); // fail sound
                return;
            }

            // ✅ Reduce energy
            energy.ReduceEnergy(_energyCost);
            Debug.Log($"{player.name} used {_energyCost} energy to activate lever.");

            // ✅ Sound for successful activation
            SoundManager.Instance.PlaySound(6);

            // ✅ Toggle lever state
            if (IsLeverOn)
                LeverOff();
            else
                LeverOn();
        }

        private void LeverOn()
        {
            IsLeverOn = true;
            _anim.SetBool("IsActive", true);

            foreach (var door in _doors)
            {
                if (door != null)
                    door.OpenDoor();
            }
        }

        private void LeverOff()
        {
            IsLeverOn = false;
            _anim.SetBool("IsActive", false);

            foreach (var door in _doors)
            {
                if (door != null)
                    door.CloseDoor();
            }
        }

        private bool CheckConditions()
        {
            // Ensure all doors have their fruit requirements met
            foreach (var door in _doors)
            {
                if (door != null && !FruitManager.Instance.AreThereEnoughFruit(door.DoorFruitType, door.DoorFruitNumber))
                    return false;
            }
            return true;
        }
    }
}
