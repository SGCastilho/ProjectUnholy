using UnityEngine;

namespace Core.Utilities
{
    public class DisableObjectByTime : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float timeToDisable = 4f;

        private float _currentTimeToDisable;

        void OnDisable() => _currentTimeToDisable = 0f;

        void Update()
        {
            _currentTimeToDisable += Time.deltaTime;
            if(_currentTimeToDisable >= timeToDisable)
            {
                _currentTimeToDisable = 0f;

                gameObject.SetActive(false);
            }
        }
    }
}
