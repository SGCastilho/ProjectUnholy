using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Utilities
{
    public sealed class VolumeProfileHandler : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Volume posprocessingProfile;

        [Header("Settings")]
        [SerializeField] private bool enabledWhenStarted;

        [Space(10)]

        [SerializeField] [Range(0.1f, 4f)] private float transistionSpeed = 2f;

        private void Start() 
        {
            gameObject.SetActive(enabledWhenStarted);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            while(posprocessingProfile.weight < 1f)
            {
                posprocessingProfile.weight = Mathf.Lerp(0f, 1f, transistionSpeed);

                yield return null;
            }
        }

        public void Hide()
        {

        }
    }
}
