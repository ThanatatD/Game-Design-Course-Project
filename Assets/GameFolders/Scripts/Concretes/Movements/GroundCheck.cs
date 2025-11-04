// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Movements
// {
//     public class GroundCheck : MonoBehaviour
//     {
//         [SerializeField] Transform[] _rayOrigins;
//         [SerializeField] float _maxRayLength=0.15f;
//         [SerializeField] LayerMask _layerMask;
//         bool _isOnGround;

//         public bool IsOnGround { get => _isOnGround; set => _isOnGround = value; }

//         private void Update()
//         {
//             foreach(Transform rayOrigin in _rayOrigins)
//             {
//                 CheckOnGround(rayOrigin);
//                 if (_isOnGround) break;
//             }
//         }
//         private void CheckOnGround(Transform rayOrigin)
//         {
//             RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.down, _maxRayLength, _layerMask);
//             // Ignore the player itself
//             if (hit.collider != null && hit.collider.gameObject != gameObject && !hit.collider.CompareTag("Trap"))
//                 _isOnGround = true;
//             else
//                 _isOnGround = false;
//         }

//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Movements
// {
//     public class GroundCheck : MonoBehaviour
//     {
//         [Header("Ray Settings")]
//         [SerializeField] Transform[] _rayOrigins;   // left, mid, right foot
//         [SerializeField] float _maxRayLength = 0.35f;
//         [SerializeField] LayerMask _layerMask;

//         [Header("Edge Stability")]
//         [SerializeField] float _raySpread = 0.15f;  // offset for backup rays
//         [SerializeField] int _extraRayCount = 1;    // how many extra between feet

//         [Header("Coyote Time")]
//         [SerializeField] float _coyoteTime = 0.1f;

//         bool _isOnGround;
//         float _coyoteTimer;

//         public bool IsOnGround => _isOnGround || _coyoteTimer > 0f;

//         private void Update()
//         {
//             bool grounded = false;

//             // check main rays
//             foreach (Transform rayOrigin in _rayOrigins)
//             {
//                 if (CastRay(rayOrigin.position)) 
//                 {
//                     grounded = true;
//                     break;
//                 }

//                 // check small side offset rays (helps with edges)
//                 for (int i = 1; i <= _extraRayCount; i++)
//                 {
//                     Vector2 leftOffset = rayOrigin.position + Vector3.left * (_raySpread * i);
//                     Vector2 rightOffset = rayOrigin.position + Vector3.right * (_raySpread * i);

//                     if (CastRay(leftOffset) || CastRay(rightOffset))
//                     {
//                         grounded = true;
//                         break;
//                     }
//                 }

//                 if (grounded) break;
//             }

//             if (grounded)
//             {
//                 _isOnGround = true;
//                 _coyoteTimer = _coyoteTime;
//             }
//             else
//             {
//                 if (_coyoteTimer > 0)
//                     _coyoteTimer -= Time.deltaTime;
//                 else
//                     _isOnGround = false;
//             }
//         }

//         private bool CastRay(Vector2 origin)
//         {
//             RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _maxRayLength, _layerMask);
//             Debug.DrawRay(origin, Vector2.down * _maxRayLength, hit.collider ? Color.green : Color.red);
//             if (hit.collider != null && hit.collider.gameObject != gameObject && !hit.collider.CompareTag("Trap"))
//                 return true;
//             return false;
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movements
{
    public class GroundCheck : MonoBehaviour
    {
        [Header("Ray Settings")]
        [SerializeField] Transform[] _rayOrigins;   // left, mid, right foot
        [SerializeField] float _maxRayLength = 0.35f;
        [SerializeField] LayerMask _layerMask; // Must include "Ground" and "Player"

        [Header("Edge Stability")]
        [SerializeField] float _raySpread = 0.15f;
        [SerializeField] int _extraRayCount = 1;

        [Header("Coyote Time")]
        [SerializeField] float _coyoteTime = 0.1f;

        bool _isOnGround;
        float _coyoteTimer;

        public bool IsOnGround => _isOnGround || _coyoteTimer > 0f;

        private void Update()
        {
            bool grounded = false;

            // main + extra rays
            foreach (Transform rayOrigin in _rayOrigins)
            {
                if (CastRay(rayOrigin.position))
                {
                    grounded = true;
                    break;
                }

                for (int i = 1; i <= _extraRayCount; i++)
                {
                    Vector2 leftOffset = rayOrigin.position + Vector3.left * (_raySpread * i);
                    Vector2 rightOffset = rayOrigin.position + Vector3.right * (_raySpread * i);

                    if (CastRay(leftOffset) || CastRay(rightOffset))
                    {
                        grounded = true;
                        break;
                    }
                }

                if (grounded) break;
            }

            // fallback: overlap check for player-on-player standing
            if (!grounded)
            {
                grounded = CheckOverlapGround();
            }

            // coyote time
            if (grounded)
            {
                _isOnGround = true;
                _coyoteTimer = _coyoteTime;
            }
            else
            {
                if (_coyoteTimer > 0)
                    _coyoteTimer -= Time.deltaTime;
                else
                    _isOnGround = false;
            }
        }

        private bool CastRay(Vector2 origin)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _maxRayLength, _layerMask);
            Debug.DrawRay(origin, Vector2.down * _maxRayLength, hit.collider ? Color.green : Color.red);
            if (hit.collider != null && hit.collider.gameObject != gameObject && !hit.collider.CompareTag("Trap"))
                return true;
            return false;
        }

        private bool CheckOverlapGround()
        {
            // small circle below feet to catch player contact
            Vector2 checkPos = transform.position + Vector3.down * 0.1f;
            float radius = 0.2f;
            Collider2D hit = Physics2D.OverlapCircle(checkPos, radius, _layerMask);

            if (hit != null && hit.gameObject != gameObject && !hit.CompareTag("Trap"))
                return true;

            return false;
        }
    }
}