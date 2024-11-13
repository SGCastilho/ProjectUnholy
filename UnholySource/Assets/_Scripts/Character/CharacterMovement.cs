using UnityEngine;

namespace Core.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        #region Constants
        private float FLIPPED_ROTATION = 180f;
        private float UNFLIPPED_ROTATION = 0f;
        #endregion

        #region Encapsulation
        public float Speed { get => speed; set => speed = value; }

        public bool IsMoving { get => isMoving; set => isMoving = value; }
        public bool MoveRight { get => moveRight; set { moveRight = value; ChangeSide(); } }
        #endregion

        [Header("Classes")]
        [SerializeField] private Rigidbody rb3D;

        [Space(10)]

        [SerializeField] private Transform graphicsFlip;

        [Header("Settings")]
        [SerializeField] private bool variableSpeed = true;
        [SerializeField] [Range(2f, 12f)] private float speed = 5f;

        [Space(10)]

        [SerializeField] private bool isMoving;
        [SerializeField] private bool moveRight;

        private float _currentSpeed;
        private float _sideMovement;
        private Vector3 _currentVelocity;

        private void OnEnable() 
        {
            if(variableSpeed)
            {
                _currentSpeed = Random.Range(speed-1f, speed);
            }
            else { _currentSpeed = speed; }
        }

        private void FixedUpdate() 
        {
            if(!isMoving) return;

            _currentVelocity = new Vector3(rb3D.velocity.x, rb3D.velocity.y, _sideMovement * _currentSpeed);

            rb3D.velocity = _currentVelocity;
        }

        private void ChangeSide()
        {
            if(moveRight)
            {
                if(_sideMovement == Vector3.forward.z && graphicsFlip.localRotation.y == UNFLIPPED_ROTATION) return;

                graphicsFlip.localRotation = new Quaternion(0f, UNFLIPPED_ROTATION, 0, graphicsFlip.localRotation.w);
                _sideMovement = Vector3.forward.z;
            }
            else
            {
                 if(_sideMovement == Vector3.back.z && graphicsFlip.localRotation.y == FLIPPED_ROTATION) return;

                graphicsFlip.localRotation = new Quaternion(0f, FLIPPED_ROTATION, 0, graphicsFlip.localRotation.w);
                _sideMovement = Vector3.back.z;
            }
        }
    }
}
