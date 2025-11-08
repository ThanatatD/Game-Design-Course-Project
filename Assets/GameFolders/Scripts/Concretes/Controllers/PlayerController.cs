using Abstracts.Input;
using Movements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using Animations;
using Mechanics;
using Managers;
using System;
using Combat;

namespace Controllers
{
    public enum PlayerType { Player1, Player2 }

    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        public PlayerType playerType; // = PlayerType.Player1;

        bool _isJumped;
        float _horizontalAxis;
        IPlayerInput _input;
        CharacterAnimation _anim;
        RbMovement _rb;
        Flip _flip;
        GroundCheck _groundCheck;
        PlatformHandler _platform;
        InteractHandler _interact;
        private bool _isPaused;

        private Energy _energy; //
        public bool CanAcceptInput { get; set; } = true; // NEW: controls if input is accepted

        private void Awake()
        {
            _rb = GetComponent<RbMovement>();
            _anim = GetComponent<CharacterAnimation>();
            _flip = GetComponent<Flip>();
            _groundCheck = GetComponent<GroundCheck>();
            _platform = GetComponent<PlatformHandler>();
            _interact = GetComponent<InteractHandler>();

            _energy = GetComponent<Energy>(); //

            // Choose input per player
            if (playerType == PlayerType.Player1)
                _input = new Inputs.PcInputPlayer1();
            else
                _input = new Inputs.PcInputPlayer2();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGamePaused += HandleGamePaused;
            GameManager.Instance.OnGameUnpaused += HandleGameUnpaused;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGamePaused -= HandleGamePaused;
            GameManager.Instance.OnGameUnpaused -= HandleGameUnpaused;
        }

        private void HandleEnergyChanged()
        {
            // Automatically enable input if energy > 0
            CanAcceptInput = _energy.CurrentEnergy > 0;
        }

        private void Update()
        {
            if (_input.IsExitButton)
            {
                SoundManager.Instance.PlaySound(2);
                if (_isPaused)
                    GameManager.Instance.UnpauseGame();
                else
                    GameManager.Instance.PauseGame();
            }

            if (_isPaused) return;

            // _horizontalAxis = _input.HorizontalAxis;
            // Read input only if allowed
            _horizontalAxis = CanAcceptInput ? _input.HorizontalAxis : 0f;

            if (_horizontalAxis != 0 && _groundCheck.IsOnGround)
                SoundManager.Instance.PlaySound(1);
            else
                SoundManager.Instance.StopSound(1);

            // if (_input.IsJumpButtonDown && _groundCheck.IsOnGround)
            //     _isJumped = true;
            if (_input.IsJumpButtonDown && _groundCheck.IsOnGround && CanAcceptInput)
                _isJumped = true;

            if (_input.IsDownButton)
                _platform.DisableCollider();

            // Try energy share first (if player has Energy component)
            bool didShare = false;
            if (_input.IsInteractButton)
            {
                if (_energy != null)
                {
                    didShare = _energy.TryShareWithNearby();
                }

                // If share didn't happen, perform normal interact
                if (!didShare)
                {
                    _interact.Interact();
                }
            }

            _anim.JumpAnFallAnim(_groundCheck.IsOnGround, _rb.VelocityY);
            _anim.HorizontalAnim(_horizontalAxis);
            _flip.FlipCharacter(_horizontalAxis);
        }

        private void FixedUpdate()
        {
            // Horizontal movement respects CanAcceptInput
            _rb.HorizontalMove(_horizontalAxis);

            if (_isJumped)
            {
                SoundManager.Instance.PlaySound(0);
                _rb.Jump();
                _isJumped = false;

                // Reduce energy for jump
                if (_energy != null)
                    _energy.ReduceEnergy(5f);
            }
        }

        private void HandleGameUnpaused() => _isPaused = false;
        private void HandleGamePaused() => _isPaused = true;

        // Handle stacking/collision between players
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
                Collider2D otherCollider = collision.gameObject.GetComponent<Collider2D>();
                Collider2D myCollider = GetComponent<Collider2D>();

                foreach (ContactPoint2D contact in collision.contacts)
                {
                    // Only allow collision if this player is above the other
                    if (contact.normal.y > 0.5f) // my feet on other player's head
                    {
                        Physics2D.IgnoreCollision(myCollider, otherCollider, false);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(myCollider, otherCollider, true);
                    }
                }
            }
        }

    }
}

namespace Inputs
{
    using Abstracts.Input;
    using UnityEngine;

    // ========================
    // PLAYER 1 INPUT (W A D)
    // ========================
    public class PcInputPlayer1 : IPlayerInput
    {
        public float HorizontalAxis
        {
            get
            {
                float axis = 0f;
                if (Input.GetKey(KeyCode.A)) axis -= 1f;
                if (Input.GetKey(KeyCode.D)) axis += 1f;
                return axis;
            }
        }

        public bool IsJumpButtonDown => Input.GetKeyDown(KeyCode.W);
        public bool IsJumpButton => Input.GetKeyDown(KeyCode.W);   // if want long press jump, change this to GetKey
        public bool IsDownButton =>  Input.GetKeyDown(KeyCode.None);  // not work and not use yet
        public bool IsInteractButton => Input.GetKeyDown(KeyCode.E);
        public bool IsExitButton => Input.GetKeyDown(KeyCode.Escape);
    }

    // ========================
    // PLAYER 2 INPUT (ARROW KEYS)
    // ========================
    public class PcInputPlayer2 : IPlayerInput
    {
        public float HorizontalAxis
        {
            get
            {
                float axis = 0f;
                if (Input.GetKey(KeyCode.LeftArrow)) axis -= 1f;
                if (Input.GetKey(KeyCode.RightArrow)) axis += 1f;
                return axis;
            }
        }

        public bool IsJumpButtonDown => Input.GetKeyDown(KeyCode.UpArrow);
        public bool IsJumpButton => Input.GetKeyDown(KeyCode.UpArrow);  // if want long press jump, change this to GetKey
        public bool IsDownButton => Input.GetKeyDown(KeyCode.None);   // not work and not use yet
        public bool IsInteractButton => Input.GetKeyDown(KeyCode.RightShift);
        public bool IsExitButton => Input.GetKeyDown(KeyCode.None); // no exit for player 2
    }
}

// // Not use PcInput class anymore (I already comment that out in PcInput.cs)
// // Now Playeres can do multiple jump and make them fly (cuz now i change playerxplayer off and make it only foreground collision, also the collision now is in foreground layer)
// // if want to fix this, make it to be player layer and allow player x player collision again and also add player at foreground ground check ... (but this will cuz some flicker error smth)


// now some item enenemy not work and not follow player 2. (not do this yet)



// checkpoint should let 2 player reach it to save. (not do this yet)