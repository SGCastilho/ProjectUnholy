using UnityEngine;
using Cinemachine;

namespace Core.GameCamera
{
    public sealed class CameraShake : MonoBehaviour
    {
        #region Instance
        public static CameraShake Instance;
        #endregion

        [Header("Classes")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [Header("Settings")]
        [SerializeField] private float attackShakeTime = 0.2f;
        [SerializeField] private float attackShakeIntensity = 1f;

        [Space(6)]

        [SerializeField] private float hittedShakeTime = 0.2f;
        [SerializeField] private float hittedShakeIntensity = 1f;

        private CinemachineBasicMultiChannelPerlin _cinemachineShake;

        private float _currentShakeTimer;

        private void Awake() 
        {
            Instance = this;

            _cinemachineShake = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnEnable() 
        {
            StopShake();
        }

        private void Update() 
        {
            if(_currentShakeTimer > 0)
            {
                _currentShakeTimer -= Time.deltaTime;

                if(_currentShakeTimer <= 0)
                {
                    StopShake();
                }
            }
        }

        public void AttackShake()
        {
            _cinemachineShake.m_AmplitudeGain = attackShakeIntensity;

            _currentShakeTimer = attackShakeTime;
        }

        public void HittedShake()
        {
            _cinemachineShake.m_AmplitudeGain = hittedShakeIntensity;

            _currentShakeTimer = hittedShakeTime;
        }

        private void StopShake()
        {
            _cinemachineShake.m_AmplitudeGain = 0f;

            _currentShakeTimer = 0f;
        }
    }
}
