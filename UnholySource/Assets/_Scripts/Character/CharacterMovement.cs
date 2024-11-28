using UnityEngine;

namespace Core.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        #region Constants
        private const float DASH_TO_SPEED = 40f;
        private const float FLIPPED_ROTATION = 180f;
        private const float UNFLIPPED_ROTATION = 0f;
        #endregion

        #region Encapsulation
        public float Speed { get => speed; set => speed = value; }
        public float DashSpeed { get => DASH_TO_SPEED; }

        public bool IsMoving 
        { 
            get => isMoving; 
            set 
            { 
                isMoving = value;
                if(isMoving == false)
                    rb3D.velocity = new Vector3(rb3D.velocity.x, rb3D.velocity.y, 0f); 
            } 
        }

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

        [SerializeField] [Range(1f, 4f)] private float dashMinOffset = 1f;
        [SerializeField] private LayerMask wallCheckLayerMask;

        [Space(10)]

        [SerializeField] private bool isMoving;
        [SerializeField] private bool moveRight;

        private bool _isDashing;
        private bool _lastIsMovingState;

        private float _dashSpeed;
        private float _currentSpeed;
        private float _sideMovement;
        private float _currentDistanceFromTarget;

        private Vector3 _currentVelocity;
        private Vector3 _dashTargetPosition;

        private Transform _dashTargetTransform;
        private Transform _transform;

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

        private void Awake() 
        {
            _transform = GetComponent<Transform>();
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

        private void Update() 
        {
            if(_isDashing)
            {
                if(_dashTargetTransform != null)
                {
                    _currentDistanceFromTarget = Vector3.Distance(_transform.position, _dashTargetTransform.position);
                }
                else 
                {
                    _currentDistanceFromTarget = Vector3.Distance(_transform.position, _dashTargetPosition);
                }

                if(_currentDistanceFromTarget <= dashMinOffset)
                {
                    _currentDistanceFromTarget = 0f;

                    EndDash();
                }
            }
        }

        private void FixedUpdate() 
        {
            isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckSize, groundCheckLayer);

            if(isGrounded && OnSlope())
            {
                rb3D.AddForce(Vector3.down * 0.85f * slopeForce * Time.deltaTime, ForceMode.Impulse);
            }

            if(isMoving && !_isDashing)
            {
                _currentVelocity = new Vector3(rb3D.velocity.x, rb3D.velocity.y, _sideMovement * _currentSpeed);

                rb3D.velocity = _currentVelocity;
            }

            if(_isDashing)
            {
                if(moveRight)
                {
                    _currentVelocity = new Vector3(rb3D.velocity.x, rb3D.velocity.y, _dashSpeed);
                }
                else
                {
                    _currentVelocity = new Vector3(rb3D.velocity.x, rb3D.velocity.y, -_dashSpeed);
                }

                rb3D.velocity = _currentVelocity;
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

        public void DashTo(Transform target, bool dashRight, float speed)
        {
            if(_isDashing) return;
            
            _dashTargetTransform = target;

            _dashSpeed = speed;
            _lastIsMovingState = isMoving;

            IsMoving = false;

            FlipToPlayer(dashRight);

            _isDashing = true;
        }

        public void DashTo(float dashLenght, bool dashRight, float speed)
        {
            if(_isDashing) return;
            
            _dashTargetTransform = null;

            if(dashRight)
            {
                Ray _wallRayCheck = new Ray(_transform.position, Vector3.forward);
                RaycastHit _wallHit = new RaycastHit();

                Physics.Raycast(_wallRayCheck, out _wallHit, dashLenght, wallCheckLayerMask);

                if(_wallHit.collider != null)
                {
                    _dashTargetPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _wallHit.point.z);

                    Debug.Log($"Wall founded, {_transform.localPosition.z - dashLenght}, to {_wallHit.point.z}");
                }
                else
                {
                    _dashTargetPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _transform.localPosition.z + dashLenght);
                }
            }
            else
            {
                Ray _wallRayCheck = new Ray(_transform.position, Vector3.back);
                RaycastHit _wallHit = new RaycastHit();

                Physics.Raycast(_wallRayCheck, out _wallHit, dashLenght, wallCheckLayerMask);

                if(_wallHit.collider != null)
                {
                    _dashTargetPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _wallHit.point.z);

                    Debug.Log($"Wall founded, {_transform.localPosition.z - dashLenght}, to {_wallHit.point.z}");
                }
                else
                {
                    _dashTargetPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _transform.localPosition.z - dashLenght);
                }
            }

            _dashSpeed = speed;
            _lastIsMovingState = isMoving;

            IsMoving = false;

            _isDashing = true;
        }
        
        private void EndDash()
        {
            if(!_isDashing) return;
            
            _dashTargetTransform = null;

            IsMoving = _lastIsMovingState;

            _isDashing = false;
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

        private void OnDrawGizmos() 
        {
            if(_isDashing && _dashTargetTransform != null)
            {
                Debug.DrawLine(_transform.position + (0.85f * Vector3.up), _dashTargetTransform.position + (0.85f * Vector3.up), Color.red);
            }
            else if(_isDashing && _dashTargetTransform == null)
            {
                Debug.DrawLine(_transform.position + (0.85f * Vector3.up), _dashTargetPosition + (0.85f * Vector3.up), Color.red);
            }
        }
        #endif
        #endregion
    }
}
