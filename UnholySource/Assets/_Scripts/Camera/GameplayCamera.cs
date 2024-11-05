using UnityEngine;

namespace Core.GameCamera
{
    public sealed class GameplayCamera : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Transform cameraTarget;

        [Header("Settings")]
        [SerializeField] private float cameraSmooth = 2f;

        [Space(10)]

        [SerializeField] private Vector3 cameraOffset;

        private Camera _camera;
        private Transform _transform;

        private Vector3 _currentCameraOffset;

        private void Awake() 
        {
            _camera = GetComponent<Camera>();
            _transform = GetComponent<Transform>();
        }

        private void LateUpdate() 
        {
            if(cameraTarget == null) return;

            _currentCameraOffset = new Vector3(cameraOffset.x, cameraOffset.y, cameraTarget.position.z);

            _transform.position = _currentCameraOffset;
        }
    }
}
