using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        #region Constants
        private float FLIPPED_ROTATION = 180f;
        private float UNFLIPPED_ROTATION = 0f;
        #endregion

        #region Encapsulation
        internal bool SprintEnabled 
        {
            get => sprintEnabled;
            set 
            {
                sprintEnabled = value;

                if(sprintEnabled)
                {
                    if(!behaviour.Actions.IsAiming)
                    {
                        _currentSpeed = sprintSpeed;
                    }
                }
                else
                {
                    if(!behaviour.Actions.IsAiming)
                    {
                        _currentSpeed = runningSpeed;
                    }
                }
            }
        }

        internal bool IsFlipped 
        { 
            get => isFlipped;
            set 
            {
                isFlipped = value;

                if(isFlipped)
                {
                    flipTransform.localRotation = new Quaternion(0f, FLIPPED_ROTATION, 0, flipTransform.localRotation.w);
                }
                else
                {
                    flipTransform.localRotation = new Quaternion(0f, UNFLIPPED_ROTATION, 0, flipTransform.localRotation.w);
                }
            }
        }

        internal bool MovementSide { get => _movementSide; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Classes")]
        [SerializeField] private Rigidbody rigidBody;

        [Space(6)]

        [SerializeField] private Transform slopeCheckTransform;
        [SerializeField] private Transform groundCheckTransform;

        [Header("Settings")]
        [SerializeField] private float slopeForce;
        [SerializeField] private float slopeCheckDistance;

        [Space(6)]

        [SerializeField] private float groundCheckSize;
        [SerializeField] private LayerMask groundCheckLayer;
        [SerializeField] private bool isGrounded;

        [Space(10)]

        [SerializeField] [Range(4f, 10f)] private float runningSpeed = 6f;
        [SerializeField] [Range(6f, 12f)] private float sprintSpeed = 10f;
        [SerializeField] [Range(1f, 4f)] private float aimingSpeed = 2f;

        [Space(10)]

        [SerializeField] private bool sprintEnabled;

        [Space(10)]

        [SerializeField] private Transform flipTransform;
        [SerializeField] private bool isFlipped;

        private bool _movementSide;
        private float _currentSpeed;
        private Vector3 _currentVelocity;

        private bool OnSlope()
        {
            RaycastHit hit;

            if(Physics.Raycast(slopeCheckTransform.position, Vector3.down, out hit, slopeCheckDistance))
            {
                if(hit.normal != Vector3.up)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnEnable() 
        {
            IsFlipped = false;
            SprintEnabled = false;

            ResetMovementSpeed();
        }

        private void Update()
        {
            CharacterFlip();
            CharacterMovement();
        }

        private void CharacterMovement()
        {
            isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckSize, groundCheckLayer);

            _currentVelocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, behaviour.Inputs.MovementAxis * _currentSpeed);

            rigidBody.velocity = _currentVelocity;

            if(isGrounded && OnSlope())
            {
                rigidBody.AddForce(Vector3.down * 0.85f * slopeForce * Time.deltaTime, ForceMode.Impulse);
            }
        }

        private void CharacterFlip()
        {
            if(behaviour.Actions.IsAiming) return;

            if (behaviour.Inputs.MovementAxis >= 1) { IsFlipped = false; _movementSide = true; }
            else if (behaviour.Inputs.MovementAxis <= -1) { IsFlipped = true; _movementSide = false; }
        }

        internal void ChangeToAimMovementSpeed()
        {
            _currentSpeed = aimingSpeed;
        }

        internal void ResetMovementSpeed()
        {
            _currentSpeed = runningSpeed;
        }

        #region Editor Function
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() 
        {
            if(groundCheckTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckSize);

                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(slopeCheckTransform.position, slopeCheckDistance * Vector3.down);
            }
        }
        #endif
        #endregion
    }
}
