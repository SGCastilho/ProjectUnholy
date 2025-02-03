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

        [Space(6)]

        [SerializeField] private LayerMask viewerRenderingMask;

        public void BackToDefaultRendering()
        {
            gameCamera.cullingMask = defaultRenderingMask;
        }

        public void OnlyViewerRendering()
        {
            gameCamera.cullingMask = viewerRenderingMask;
        }

        public void OnlyRenderingUI()
        {
            gameCamera.cullingMask = uiRenderingMask;
        }
    }
}
