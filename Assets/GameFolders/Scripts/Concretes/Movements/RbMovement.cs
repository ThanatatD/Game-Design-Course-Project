using UnityEngine;

namespace Movements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RbMovement : MonoBehaviour
    {
        [SerializeField] float _jumpForce = 10f;
        [SerializeField] float _horizontalSpeed = 10f;
        private float _horizontalDirection;

        Rigidbody2D _rb;

        public float VelocityY => _rb.velocity.y;
        public float HorizontalDirection { get => _horizontalDirection; set => _horizontalDirection = value; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Jump()
        {
            _rb.velocity = Vector2.up * _jumpForce;
        }

        public void HorizontalMove(float direction)
        {
            HorizontalDirection = Mathf.Sign(direction);
            _rb.velocity = new Vector2(direction * _horizontalSpeed, _rb.velocity.y);
        }
    }
}
