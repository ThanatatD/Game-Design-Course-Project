// using Combat;
// using TMPro;
// using UnityEngine;

// namespace UIs
// {
//     public class EnergyTextUI : MonoBehaviour
//     {
//         [SerializeField] Health _playerEnergy;
//         [SerializeField] TMP_Text _energyText; // Drag your TMP text object here

//         private void OnEnable()
//         {
//             _playerEnergy.OnHealthChanged += UpdateHealthText;
//             _playerEnergy.OnDead += HandleOnDead;

//             // ✅ Immediately show correct value when enabled
//             UpdateHealthText();
//         }

//         private void OnDisable()
//         {
//             _playerEnergy.OnHealthChanged -= UpdateHealthText;
//             _playerEnergy.OnDead -= HandleOnDead;
//         }

//         // ✅ Subscribe to event that passes values (more flexible)
//         private void UpdateHealthText()
//         {
//             if (_playerEnergy != null)
//                 _energyText.text = $"Energy: {_playerEnergy.CurrentHealth}"; // / {_playerEnergy.MaxHealth}";
//         }

//         private void HandleOnDead()
//         {
//             _energyText.text = "DEAD";
//         }
//     }
// }

using Combat;
using TMPro;
using UnityEngine;

namespace UIs
{
    public class EnergyTextUI : MonoBehaviour
    {
        [SerializeField] private Energy _playerEnergy;
        [SerializeField] private TMP_Text _energyText; // Drag your TMP text object here

        private void OnEnable()
        {
            if (_playerEnergy == null || _energyText == null) return;

            _playerEnergy.OnEnergyChanged += UpdateEnergyText;
            _playerEnergy.OnRunOutOfEnergy += HandleRunOutOfEnergy;

            // ✅ Immediately update when enabled
            UpdateEnergyText();
        }

        private void OnDisable()
        {
            if (_playerEnergy == null) return;

            _playerEnergy.OnEnergyChanged -= UpdateEnergyText;
            _playerEnergy.OnRunOutOfEnergy -= HandleRunOutOfEnergy;
        }

        private void UpdateEnergyText()
        {
            if (_playerEnergy != null && _energyText != null)
                _energyText.text = $"Energy: {_playerEnergy.CurrentEnergy:F0}";
        }

        private void HandleRunOutOfEnergy()
        {
            if (_energyText != null)
                _energyText.text = "OUT OF ENERGY";
        }
    }
}
