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


// ------------ for case jump cooldown but use collider as foreground to prevent some bugged position that think it still falling but already on ground (however this sol make the player animation jump and fall gone) ------------

// using UnityEngine;

// namespace Movements
// {
//     [RequireComponent(typeof(Rigidbody2D))]
//     public class RbMovement : MonoBehaviour
//     {
//         [SerializeField] float _jumpForce = 10f;
//         [SerializeField] float _horizontalSpeed = 10f;
//         [SerializeField] float _jumpCooldown = 0.2f; // cd between jumps

//         private float _horizontalDirection;
//         private bool _canJump = true;
//         private Rigidbody2D _rb;

//         public float VelocityY => _rb.velocity.y;
//         public float HorizontalDirection { get => _horizontalDirection; set => _horizontalDirection = value; }

//         private void Awake()
//         {
//             _rb = GetComponent<Rigidbody2D>();
//         }

//         public void Jump()
//         {
//             if (_canJump)
//             {
//                 _canJump = false;
//                 _rb.velocity = new Vector2(_rb.velocity.x, 0f); // reset Y velocity for consistent jump
//                 _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
//                 Invoke(nameof(ResetJump), _jumpCooldown);
//             }
//         }

//         private void ResetJump()
//         {
//             _canJump = true;
//         }

//         public void HorizontalMove(float direction)
//         {
//             HorizontalDirection = Mathf.Sign(direction);
//             _rb.velocity = new Vector2(direction * _horizontalSpeed, _rb.velocity.y);
//         }
//     }
// }
