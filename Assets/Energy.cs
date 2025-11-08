// using Animations;
// using Managers;
// using UnityEngine;
// using Controllers;  


// namespace Combat
// {
//     public class Energy : MonoBehaviour
//     {
//         // [SerializeField] private float _cooldownTimeAfterEmpty = 0.5f; // optional delay before regaining control
//         [SerializeField] private float _initialEnergy = 100f;

//         private float _currentEnergy;
//         private bool _isOutOfEnergy;

//         public float CurrentEnergy => _currentEnergy;
//         public bool IsOutOfEnergy => _isOutOfEnergy;

//         public event System.Action OnRunOutOfEnergy;
//         public event System.Action OnEnergyChanged;

//         // private CharacterAnimation _anim;
//         private PlayerController _movement; // movement script reference       // i think no need to enable false. just cant jump and walk when no energy which will handle if in playercontroller instead..

//         private void Awake()
//         {
//             // _anim = GetComponent<CharacterAnimation>();
//             _movement = GetComponent<PlayerController>();
//             _currentEnergy = _initialEnergy;
//         }

//         private void OnEnable()
//         {
//             OnRunOutOfEnergy += HandleRunOutOfEnergy;
//         }

//         private void Start()
//         {
//             OnEnergyChanged?.Invoke(); // update UI at start
//         }

//         private void HandleRunOutOfEnergy()
//         {
//             if (_isOutOfEnergy) return;
//             _isOutOfEnergy = true;

//             // Prevent player from using input
//             if (_movement != null)
//                 _movement.CanAcceptInput = false;

//             // // Optional animation feedback
//             // if (_anim != null)
//             //     _anim.TakeHitAnim(true); // or make a “tired” animation

//             // // Optional cooldown before re-enabling (if you want)
//             // Invoke(nameof(EnableControl), _cooldownTimeAfterEmpty);
//         }

//         private void HandleEnergyChanged()
//         {
//             // If player recovers energy, allow input again
//             if (_isOutOfEnergy && _currentEnergy > 0)
//             {
//                 _isOutOfEnergy = false;
//                 if (_movement != null)
//                     _movement.CanAcceptInput = true;

//                 // Optional: stop tired animation
//                 // _anim?.TakeHitAnim(false);
//             }
//         }

//         /// <summary>
//         /// Adds or removes energy (can exceed 100).
//         /// </summary>
//         public void ModifyEnergy(float amount)
//         {
//             _currentEnergy += amount;
//             if (_currentEnergy < 0) _currentEnergy = 0;

//             OnEnergyChanged?.Invoke();

//             if (_currentEnergy <= 0)
//                 OnRunOutOfEnergy?.Invoke();
//         }

//         /// <summary>
//         /// Shortcut for using energy (positive value reduces energy).
//         /// </summary>
//         public void ReduceEnergy(float amount)
//         {
//             ModifyEnergy(-Mathf.Abs(amount));
//         }
//     }
// }

// using Animations;
// using Managers;
// using UnityEngine;
// using Controllers;
// using System.Diagnostics;
// using Debug = UnityEngine.Debug;

// namespace Combat
// {
//     public class Energy : MonoBehaviour
//     {
//         [SerializeField] private float _initialEnergy = 100f;
//         [SerializeField] private float _shareDistance = 2f;  // distance to allow sharing
//         [SerializeField] private float _shareAmount = 20f;   // amount transferred per share
//         [SerializeField] private float _shareCooldown = 1f;  // optional cooldown to prevent spam

//         private float _currentEnergy;
//         private bool _isOutOfEnergy;
//         private bool _canShare = true;

//         public float CurrentEnergy => _currentEnergy;
//         public bool IsOutOfEnergy => _isOutOfEnergy;

//         public event System.Action OnRunOutOfEnergy;
//         public event System.Action OnEnergyChanged;

//         private PlayerController _movement;

//         private void Awake()
//         {
//             _movement = GetComponent<PlayerController>();
//             _currentEnergy = _initialEnergy;
//         }

//         private void OnEnable()
//         {
//             OnRunOutOfEnergy += HandleRunOutOfEnergy;
//         }

//         private void Start()
//         {
//             OnEnergyChanged?.Invoke(); // update UI at start
//         }

//         private void Update()
//         {
//             HandleEnergyChanged();
//             HandleEnergyShareInput();
//         }

//         private void HandleRunOutOfEnergy()
//         {
//             if (_isOutOfEnergy) return;
//             _isOutOfEnergy = true;

//             if (_movement != null)
//                 _movement.CanAcceptInput = false;
//         }

//         private void HandleEnergyChanged()
//         {
//             // if player recovers energy, allow input again
//             if (_isOutOfEnergy && _currentEnergy > 0)
//             {
//                 _isOutOfEnergy = false;
//                 if (_movement != null)
//                     _movement.CanAcceptInput = true;
//             }
//         }

//         private void HandleEnergyShareInput()
//         {
//             if (!_canShare) return;

//             if (Input.GetKeyDown(KeyCode.Space))
//             {
//                 Energy nearest = FindNearestPlayer();
//                 if (nearest != null)
//                 {
//                     TryShareEnergy(nearest);
//                     StartCoroutine(ShareCooldown());
//                 }
//             }
//         }

//         private System.Collections.IEnumerator ShareCooldown()
//         {
//             _canShare = false;
//             yield return new WaitForSeconds(_shareCooldown);
//             _canShare = true;
//         }

//         private Energy FindNearestPlayer()
//         {
//             Energy nearest = null;
//             float nearestDist = Mathf.Infinity;

//             foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
//             {
//                 if (player == gameObject) continue;

//                 float dist = Vector2.Distance(transform.position, player.transform.position);
//                 if (dist < _shareDistance && dist < nearestDist)
//                 {
//                     nearest = player.GetComponent<Energy>();
//                     nearestDist = dist;
//                 }
//             }

//             return nearest;
//         }

//         private void TryShareEnergy(Energy other)
//         {
//             if (other == null) return;

//             // Determine who has more energy
//             if (_currentEnergy > other._currentEnergy + _shareAmount)
//             {
//                 _currentEnergy -= _shareAmount;
//                 other._currentEnergy += _shareAmount;
//                 Debug.Log($"{name} shared {_shareAmount} energy to {other.name}");
//             }
//             else if (other._currentEnergy > _currentEnergy + _shareAmount)
//             {
//                 other._currentEnergy -= _shareAmount;
//                 _currentEnergy += _shareAmount;
//                 Debug.Log($"{other.name} shared {_shareAmount} energy to {name}");
//             }

//             // Notify both
//             OnEnergyChanged?.Invoke();
//             other.OnEnergyChanged?.Invoke();
//         }

//         public void ModifyEnergy(float amount)
//         {
//             _currentEnergy += amount;
//             if (_currentEnergy < 0) _currentEnergy = 0;

//             OnEnergyChanged?.Invoke();

//             if (_currentEnergy <= 0)
//                 OnRunOutOfEnergy?.Invoke();
//         }

//         public void ReduceEnergy(float amount)
//         {
//             ModifyEnergy(-Mathf.Abs(amount));
//         }
//     }
// }

using Animations;
using Managers;
using UnityEngine;
using Controllers;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Combat
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private float _initialEnergy = 100f;
        [SerializeField] private float _shareDistance = 2f;  // distance to allow sharing
        [SerializeField] private float _shareAmount = 20f;   // amount transferred per share
        [SerializeField] private float _shareCooldown = 0f;  // cooldown to prevent spam

        private float _currentEnergy;
        private bool _isOutOfEnergy;
        private bool _canShare = true;

        public float CurrentEnergy => _currentEnergy;
        public bool IsOutOfEnergy => _isOutOfEnergy;

        public event System.Action OnRunOutOfEnergy;
        public event System.Action OnEnergyChanged;

        private PlayerController _movement;

        private void Awake()
        {
            _movement = GetComponent<PlayerController>();
            _currentEnergy = _initialEnergy;
        }

        private void OnEnable()
        {
            OnRunOutOfEnergy += HandleRunOutOfEnergy;
        }

        private void Start()
        {
            OnEnergyChanged?.Invoke(); // update UI at start
        }

        private void Update()
        {
            // keep input handling out of Energy to avoid conflicts.
            // Energy still monitors state so we keep HandleEnergyChanged here:
            HandleEnergyChanged();
        }

        private void HandleRunOutOfEnergy()
        {
            if (_isOutOfEnergy) return;
            _isOutOfEnergy = true;
            if (_movement != null)
                _movement.CanAcceptInput = false;
        }

        private void HandleEnergyChanged()
        {
            if (_isOutOfEnergy && _currentEnergy > 0)
            {
                _isOutOfEnergy = false;
                if (_movement != null)
                    _movement.CanAcceptInput = true;
            }
        }

        // /// <summary>
        // /// Try to share energy with the nearest other player within _shareDistance.
        // /// Called externally (PlayerController) when player presses Interact/Space.
        // /// Returns true if a transfer occurred.
        // /// </summary>
        // public bool TryShareWithNearby()
        // {
        //     if (!_canShare) return false;

        //     Energy other = FindNearestPlayer();
        //     if (other == null) return false;

        //     // who gives? choose higher -> lower. If equal or difference < 1, do nothing.
        //     float diff = _currentEnergy - other._currentEnergy;
        //     if (Mathf.Abs(diff) < 1f) return false;

        //     // Determine donor and receiver and practical transfer amount (clamped)
        //     Energy donor = diff > 0 ? this : other;
        //     Energy receiver = diff > 0 ? other : this;
        //     float amount = Mathf.Min(_shareAmount, donor._currentEnergy); // can't give more than donor has

        //     if (amount <= 0f) return false;

        //     donor._currentEnergy -= amount;
        //     receiver._currentEnergy += amount;

        //     // clamp receiver if you want no cap — you said can exceed 100 so no clamp
        //     // notify both
        //     donor.OnEnergyChanged?.Invoke();
        //     receiver.OnEnergyChanged?.Invoke();

        //     // start cooldown
        //     StartCoroutine(ShareCooldown());

        //     Debug.Log($"[Energy] {donor.name} -> {receiver.name} : {amount}");
        //     return true;
        // }

        public bool TryShareWithNearby()
        {
            if (!_canShare) return false;

            Energy other = FindNearestPlayer();
            if (other == null) return false;

            float amount = _shareAmount; // e.g., 20 units

            // Check if this player (who pressed the key) has enough energy
            if (_currentEnergy < amount)
            {
                Debug.Log($"{name}: Not enough energy to share!");
                SoundManager.Instance.PlaySound(7);
                return false;
            }

            // Check if the receiver already has max energy
            if (other._currentEnergy >= 100f)
            {
                Debug.Log($"{other.name} already has >=100 energy! Cannot transfer.");
                SoundManager.Instance.PlaySound(7);
                return false;
            }

            // Transfer energy to the other player
            _currentEnergy -= amount;
            other._currentEnergy += amount;

            // Notify UI or other systems
            OnEnergyChanged?.Invoke();
            other.OnEnergyChanged?.Invoke();

            // Check if this player ran out of energy after sharing
            if (_currentEnergy <= 0f)
                HandleRunOutOfEnergy();

            // Optional cooldown to prevent rapid sharing
            // StartCoroutine(ShareCooldown());

            Debug.Log($"[Energy] {name} -> {other.name} : {amount}");
            SoundManager.Instance.PlaySound(5);
            return true;
        }

        private System.Collections.IEnumerator ShareCooldown()
        {
            _canShare = false;
            yield return new WaitForSeconds(_shareCooldown);
            _canShare = true;
        }

        private Energy FindNearestPlayer()
        {
            Energy nearest = null;
            float nearestDist = Mathf.Infinity;

            // Ensure players are tagged "Player"
            var players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                if (player == gameObject) continue;
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < _shareDistance && dist < nearestDist)
                {
                    nearest = player.GetComponent<Energy>();
                    nearestDist = dist;
                }
            }
            return nearest;
        }

        public void ModifyEnergy(float amount)
        {
            _currentEnergy += amount;
            if (_currentEnergy < 0f) _currentEnergy = 0f;
            OnEnergyChanged?.Invoke();
            if (_currentEnergy <= 0f) OnRunOutOfEnergy?.Invoke();
        }

        public void ReduceEnergy(float amount)
        {
            ModifyEnergy(-Mathf.Abs(amount));
        }
    }
}
