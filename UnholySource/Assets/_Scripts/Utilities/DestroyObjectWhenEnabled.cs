using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Utilities
{
    public sealed class DestroyObjectWhenEnabled : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool enableOnStart;

        [Space(10)]

        [SerializeField] private float timeToDestroy = 20f;
        [SerializeField] private float delayToDestroy = 4f;

        [Header("Unity Event")]
        [Space(10)]

        [SerializeField] [Tooltip("When the timer ends, this will be executed")] private UnityEvent OnStartingToDestroy;

        private void Awake() 
        {
            gameObject.SetActive(enableOnStart);
        }

        private void OnEnable() 
        {
            StartCoroutine(DestroyCouroutine());
        }

        private void OnDisable() 
        {
            StopAllCoroutines();
        }   

        private IEnumerator DestroyCouroutine()
        {
            yield return new WaitForSeconds(timeToDestroy);

            OnStartingToDestroy?.Invoke();

            yield return new WaitForSeconds(delayToDestroy);

            DestroyObject();
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
