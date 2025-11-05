using Combat;
using TMPro;
using UnityEngine;

namespace UIs
{
    public class EnergyTextUI : MonoBehaviour
    {
        [SerializeField] Health _playerEnergy;
        [SerializeField] TMP_Text _energyText; // Drag your TMP text object here

        private void OnEnable()
        {
            _playerEnergy.OnHealthChanged += UpdateHealthText;
            _playerEnergy.OnDead += HandleOnDead;

            // ✅ Immediately show correct value when enabled
            UpdateHealthText();
        }

        private void OnDisable()
        {
            _playerEnergy.OnHealthChanged -= UpdateHealthText;
            _playerEnergy.OnDead -= HandleOnDead;
        }

        // ✅ Subscribe to event that passes values (more flexible)
        private void UpdateHealthText()
        {
            if (_playerEnergy != null)
                _energyText.text = $"Energy: {_playerEnergy.CurrentHealth}"; // / {_playerEnergy.MaxHealth}";
        }

        private void HandleOnDead()
        {
            _energyText.text = "DEAD";
        }
    }
}
