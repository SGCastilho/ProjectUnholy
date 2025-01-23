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

        [Space(10)]

        [SerializeField] [Range(0.1f, 4f)] private float transistionSpeed = 2f;

        private Tween weightTween;

        public void Show()
        {
            weightTween = null;
            weightTween = DOTween.To(() => posprocessingProfile.weight, x => posprocessingProfile.weight = x, 1f, transistionSpeed);
        }

        public void Hide()
        {
            weightTween = null;
            weightTween = DOTween.To(() => posprocessingProfile.weight, x => posprocessingProfile.weight = x, 0f, transistionSpeed)
            .OnComplete(() => { weightTween = null; });
        }
    }
}
