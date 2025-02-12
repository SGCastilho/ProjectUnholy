using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Managers
{
    public sealed class TakeScreenshoot : MonoBehaviour
    {
        public bool TakeScreenShoot { get; set; }
        public byte[] ScreenShootBytes { get; private set; }

        void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += RenderPipelineEndCameraRendering;
        }

        void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= RenderPipelineEndCameraRendering;
        }

        private RenderTexture resizeRenderTexture;
        private Texture2D screenshootTexture;

        /// <summary>
        /// Irá ser executado assim que a camera terminar de renderizar, com isso a hud não ira aparecer na screenshoot /// </summary>
        private void RenderPipelineEndCameraRendering(ScriptableRenderContext arg1, Camera arg2)
        {
            if(TakeScreenShoot)
            {
                TakeScreenShoot = false;

                ScreenShootBytes = null;

                int width = Screen.width; //Pega o tamanho da tela atual do usuário
                int height = Screen.height;

                screenshootTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

                Rect screenshootRect = new Rect(0f, 0f, width, height);
                screenshootTexture.ReadPixels(screenshootRect, 0, 0);
                screenshootTexture.Apply();

                ScreenShootBytes = screenshootTexture.EncodeToPNG();
            }
        }
    }
}
