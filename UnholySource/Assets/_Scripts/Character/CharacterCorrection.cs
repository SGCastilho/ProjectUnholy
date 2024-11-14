using DG.Tweening;
using UnityEngine;

namespace Core.Character
{
    public sealed class CharacterCorrection : MonoBehaviour
    {
        #region Encapsulation
        public bool FindCollider { get => _findCollider; }
        #endregion

        [Header("Classes")]
        [SerializeField] private Transform raycastTransform;

        [Header("Settings")]
        [SerializeField] private LayerMask raycastLayer;
        [SerializeField] private float raycastLegth = 4f;

        [Space(6)]

        [SerializeField] [Range(0.1f, 1f)] private float correctionDuration = 0.6f;
        [SerializeField] [Range(0.1f, 1f)] private float correctionSpacing = 0.2f;
        [SerializeField] [Range(0.6f, 2f)] private float correctionLimit = 1f;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(20)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private bool showBackwards;

        [Space(6)]

        [SerializeField] private Color raycastLineColor;
        #endif
        #endregion

        private Ray _rayDetection;
        private RaycastHit _raycastHitDetection;

        private bool _findCollider;
        private float _correctionValue;

        private Transform _transform;

        private void Awake() 
        {
            _transform = GetComponent<Transform>();
        }

        public void Correct(bool forwardCast)
        {
            _findCollider = false;

            if(forwardCast)
            {
                _rayDetection = new Ray(raycastTransform.position, Vector3.forward);
                _raycastHitDetection = new RaycastHit();

                _findCollider = Physics.Raycast(_rayDetection, out _raycastHitDetection, raycastLegth, raycastLayer);

                if(_findCollider)
                {
                    if(Mathf.Abs(_raycastHitDetection.transform.position.z - _transform.position.z) <= correctionLimit) return;

                    _correctionValue = _raycastHitDetection.transform.position.z - correctionSpacing;

                    _transform.DOKill();
                    _transform.DOMoveZ(_correctionValue, correctionDuration);
                }
            }
            else
            {
                _rayDetection = new Ray(raycastTransform.position, Vector3.back);
                _raycastHitDetection = new RaycastHit();

                _findCollider = Physics.Raycast(_rayDetection, out _raycastHitDetection, raycastLegth, raycastLayer);

                if(_findCollider)
                {
                    if(Mathf.Abs(_raycastHitDetection.transform.position.z - _transform.position.z) <= correctionLimit) return;

                    _correctionValue = _raycastHitDetection.transform.position.z + correctionSpacing;

                    _transform.DOKill();
                    _transform.DOMoveZ(_correctionValue, correctionDuration);
                }
            }
        }

        #region Editor Functions
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                if(showBackwards)
                {
                    Gizmos.color = raycastLineColor;
                    Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + raycastLegth * Vector3.back);
                }
                else
                {
                    Gizmos.color = raycastLineColor;
                    Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + raycastLegth * Vector3.forward);
                }
            }
        }   
        #endif
        #endregion
    }
}
