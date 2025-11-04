using Abstracts.Input;
using Inputs;
using UnityEngine;

namespace Movements
{
    public class FallControl : MonoBehaviour
    {
        [SerializeField] float _fallMultiplier = 2f;
        [SerializeField] float _lowJumpMulitplier = 2f;
        [SerializeField] Controllers.PlayerType playerType = Controllers.PlayerType.Player1;

        Rigidbody2D _rb;
        IPlayerInput _input;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = playerType == Controllers.PlayerType.Player1
                ? new PcInputPlayer1()
                : new PcInputPlayer2();
        }

        private void Update()
        {
            if (_rb.velocity.y < 0)
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            else if (_rb.velocity.y > 0.01f && !_input.IsJumpButton)
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMulitplier - 1) * Time.deltaTime;
        }
    }
}
