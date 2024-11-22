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

        [Space(10)]

        [SerializeField] private Transform slopeCheckTransform;
        [SerializeField] private Transform groundCheckTransform;

        [Header("Settings")]
        [SerializeField] private float slopeForce;
        [SerializeField] private float slopeCheckDistance;

        [Space(6)]

        [SerializeField] private float groundCheckSize;
        [SerializeField] private LayerMask groundCheckLayer;
        [SerializeField] private bool isGrounded;

        [SerializeField] private bool variableSpeed = true;
        [SerializeField] [Range(2f, 12f)] private float speed = 5f;

        [Space(10)]

        [SerializeField] private bool isMoving;
        [SerializeField] private bool moveRight;

        private float _currentSpeed;
        private float _sideMovement;
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
            if(variableSpeed)
            {
                _currentSpeed = Random.Range(speed-1f, speed);
            }
            else { _currentSpeed = speed; }
        }

        private void OnDisable() 
        {
            isMoving = false;
        }

        private void FixedUpdate() 
        {
            if(!isMoving) return;

            isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckSize, groundCheckLayer);

            _currentVelocity = new Vector3(rb3D.velocity.x, rb3D.velocity.y, _sideMovement * _currentSpeed);

            rb3D.velocity = _currentVelocity;

            if(isGrounded && OnSlope())
            {
                rb3D.AddForce(Vector3.down * 0.85f * slopeForce * Time.deltaTime, ForceMode.Impulse);
            }
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

        public void FlipToPlayer(bool playerInTheRight)
        {
            MoveRight = playerInTheRight;
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
