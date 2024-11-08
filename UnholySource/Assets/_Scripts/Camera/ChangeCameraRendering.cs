using UnityEngine;

namespace Core.GameCamera
{
    public sealed class ChangeCameraRendering : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Camera gameCamera;

        [Header("Settings")]
        [SerializeField] private LayerMask defaultRenderingMask;

        [Space(6)]

        [SerializeField] private LayerMask uiRenderingMask;

        public void BackToDefaultRendering()
        {
            gameCamera.cullingMask = defaultRenderingMask;
        }

        public void OnlyRenderingUI()
        {
            gameCamera.cullingMask = uiRenderingMask;
        }
    }
}
