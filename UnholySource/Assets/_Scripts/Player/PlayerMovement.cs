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
                    _currentSpeed = sprintSpeed;
                }
                else
                {
                    _currentSpeed = runningSpeed;
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
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Classes")]
        [SerializeField] private Rigidbody rigidBody;

        [Header("Settings")]
        [SerializeField] [Range(4f, 10f)] private float runningSpeed = 6f;
        [SerializeField] [Range(6f, 12f)] private float sprintSpeed = 10f;
        [Space(10)]
        [SerializeField] private bool sprintEnabled;
        [Space(10)]
        [SerializeField] private Transform flipTransform;
        [SerializeField] private bool isFlipped;

        private float _currentSpeed;
        private Vector3 _currentVelocity;

        private void OnEnable() 
        {
            IsFlipped = false;
            SprintEnabled = false;
        }

        private void Update()
        {
            CharacterFlip();
            CharacterMovement();
        }

        private void CharacterMovement()
        {
            _currentVelocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, behaviour.Inputs.MovementAxis * _currentSpeed);

            rigidBody.velocity = _currentVelocity;
        }

        private void CharacterFlip()
        {
            if (behaviour.Inputs.MovementAxis >= 1) { IsFlipped = false; }
            else if (behaviour.Inputs.MovementAxis <= -1) { IsFlipped = true; }
        }
    }
}
