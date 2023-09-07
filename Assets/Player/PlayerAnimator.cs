using UnityEngine;
using Random = UnityEngine.Random;

namespace TarodevController {
    /// <summary>
    /// This is a pretty filthy script. I was just arbitrarily adding to it as I went.
    /// You won't find any programming prowess here.
    /// This is a supplementary script to help with effects and animation. Basically a juice factory.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour {
        [SerializeField] private Animator _anim;
        [SerializeField] private AnimatorOverrideController _animOV;
        // [SerializeField] private AudioSource _source;
        [SerializeField] private LayerMask _groundMask;

        private IPlayerController _player;
        private bool _playerGrounded;
        private ParticleSystem.MinMaxGradient _currentGradient;
        private Vector2 _movement;

        void Awake() => _player = GetComponentInParent<IPlayerController>();

        void Update() {
            if (_player == null) return;

            // X movement
            _anim.SetFloat(XMovementKey, Mathf.Abs(_movement.x));
            
            // Y movement
            _anim.SetFloat(YMovementKey, _movement.y);
            
            if (_player.LandingThisFrame) {
                _anim.SetBool(GroundedKey, true);
            }

            if (_player.JumpingThisFrame) {
                _anim.SetTrigger(JumpKey);
                _anim.SetBool(GroundedKey, false);
            }

            _movement = _player.RawMovement; // Previous frame movement is more valuable
        }

        #region Animation Keys

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int XMovementKey = Animator.StringToHash("XMovement");
        private static readonly int YMovementKey = Animator.StringToHash("YMovement");
        private static readonly int JumpKey = Animator.StringToHash("Jump");

        #endregion
    }
}