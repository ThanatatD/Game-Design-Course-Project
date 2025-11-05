using Combat;
using UnityEngine;
using UnityEngine.UI;

namespace UIs
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private Health _playerHealth;
        [SerializeField] private Image _fillImage; // Assign the fill image of your bar in the Inspector

        private void OnEnable()
        {
            _playerHealth.OnHealthChanged += UpdateHealthBar;
            _playerHealth.OnDead += HandleOnDead;
        }

        private void Start()
        {
            UpdateHealthBar();
        }

        private void OnDisable()
        {
            _playerHealth.OnHealthChanged -= UpdateHealthBar;
            _playerHealth.OnDead -= HandleOnDead;
        }

        private void UpdateHealthBar()
        {
            float fillAmount = (float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth;
            _fillImage.fillAmount = fillAmount;
        }

        private void HandleOnDead()
        {
            _fillImage.fillAmount = 0f;
        }
    }
}