using DG.Tweening;
using UnityEngine;

namespace Core.Scenarios
{
    public sealed class SpotLightDefective : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Light spotLight;

        [Header("Settings")]
        [SerializeField] [Range(1f, 10f)] private float flickiringCouldown = 2f;
        [SerializeField] [Range(1f, 10f)] private float flickingEndTimer = 1f;
        [SerializeField] [Range(0.1f, 2f)] private float flickiringDuration = 1f;

        [Space(10)]

        [SerializeField] private bool isFlicking;

        private bool _endingFlicking;
        private float _spotLightIntensity;
        private float _currentFlickringCouldown;
        private float _currentFlickingTimerEnd;

        private void OnEnable() 
        {
            if(spotLight == null) { enabled = false; }

            _spotLightIntensity = spotLight.intensity;
        }

        private void Update() 
        {
            if(!isFlicking && !_endingFlicking)
            {
                _currentFlickringCouldown += Time.deltaTime;
                if(_currentFlickringCouldown >= flickiringCouldown)
                {
                    isFlicking = true;
                    _currentFlickringCouldown = 0;

                    StartFlicking();
                }
            }
            else if(isFlicking)
            {
                _currentFlickingTimerEnd += Time.deltaTime;
                if(_currentFlickingTimerEnd >= flickingEndTimer)
                {
                    _endingFlicking = true;
                    isFlicking = false;
                    _currentFlickingTimerEnd = 0;
                }
            }
        }

        private void StartFlicking()
        {
            spotLight.DOKill();
            spotLight.DOIntensity(0f, flickiringDuration).OnComplete(EndFlicking);
        }

        private void EndFlicking()
        {
            spotLight.DOKill();
            spotLight.DOIntensity(_spotLightIntensity, flickiringDuration).OnComplete(CheckFlicking);
        }

        private void CheckFlicking()
        {
            if(_endingFlicking)
            {
                spotLight.DOKill();
                spotLight.intensity = _spotLightIntensity;

                _endingFlicking = false;
            }
            else
            {
                StartFlicking();
            }
        }
    }
}
