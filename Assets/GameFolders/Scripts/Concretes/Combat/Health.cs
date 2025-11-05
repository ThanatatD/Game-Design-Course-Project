// using Animations;
// using Managers;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;


// namespace Combat
// {
//     public class Health : MonoBehaviour
//     {
//         [SerializeField] int _maxHealth;
//         [SerializeField] bool _IsCooldownAfterHit;
//         [SerializeField] float _cooldownTimeAfterHit;
//         bool _isInvulnerable;
//         int _currentHealth;
//         public bool IsDead => _currentHealth <= 0;

//         public int MaxHealth { get => _maxHealth; }
//         public int CurrentHealth { get => _currentHealth;  }
//         public float CooldownTimeAfterHit { get => _cooldownTimeAfterHit; set => _cooldownTimeAfterHit = value; }

//         public event System.Action OnDead;
//         public event System.Action OnHealthChanged;
//         CharacterAnimation _anim;

//         private void Awake()
//         {
//             _anim = GetComponent<CharacterAnimation>();
//             _currentHealth = _maxHealth;
//         }
//         private void OnEnable()
//         {
//             OnHealthChanged?.Invoke();
//             OnDead += HandleOnDead;
//         }

//         private void HandleOnDead()
//         {
//             _currentHealth = _maxHealth;
//             _anim.AppearAnim(0.4f);
//         }

//         public void TakeHit(Damage damage)
//         {
//             if (_isInvulnerable)
//             {
//                 return;
//             }
//             SoundManager.Instance.PlaySound(4);
//             _currentHealth -= damage.HitDamage;
//             OnHealthChanged?.Invoke();
//             StartCoroutine(HitCooldown());
//             if (IsDead)
//                 OnDead?.Invoke();
//         }
//         IEnumerator HitCooldown()
//         {
//             _isInvulnerable = true;
//             _anim.TakeHitAnim(true);
//             yield return new WaitForSeconds(_cooldownTimeAfterHit);
//             _isInvulnerable = false;
//             _anim.TakeHitAnim(false);

//         }



//     }

// }


using Animations;
using Managers;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int _maxHealth = 100;
        [SerializeField] bool _IsCooldownAfterHit = true;
        [SerializeField] float _cooldownTimeAfterHit = 0.5f;

        bool _isInvulnerable;
        int _currentHealth;

        public bool IsDead => _currentHealth <= 0;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public float CooldownTimeAfterHit { get => _cooldownTimeAfterHit; set => _cooldownTimeAfterHit = value; }

        public event System.Action OnDead;
        public event System.Action OnHealthChanged;

        CharacterAnimation _anim;

        private void Awake()
        {
            _anim = GetComponent<CharacterAnimation>();
            _currentHealth = _maxHealth;
        }

        private void OnEnable()
        {
            OnDead += HandleOnDead;
        }

        private void Start()
        {
            // ✅ Now safely trigger the first update after Awake has run
            OnHealthChanged?.Invoke();
        }

        private void HandleOnDead()
        {
            _currentHealth = _maxHealth;
            _anim.AppearAnim(0.4f);
        }

        public void TakeHit(Damage damage)
        {
            if (_isInvulnerable) return;

            SoundManager.Instance.PlaySound(4);
            _currentHealth -= damage.HitDamage;
            if (_currentHealth < 0) _currentHealth = 0;

            OnHealthChanged?.Invoke();

            if (_IsCooldownAfterHit)
                StartCoroutine(HitCooldown());

            if (IsDead)
                OnDead?.Invoke();
        }

        IEnumerator HitCooldown()
        {
            _isInvulnerable = true;
            _anim.TakeHitAnim(true);
            yield return new WaitForSeconds(_cooldownTimeAfterHit);
            _isInvulnerable = false;
            _anim.TakeHitAnim(false);
        }
    }
}
