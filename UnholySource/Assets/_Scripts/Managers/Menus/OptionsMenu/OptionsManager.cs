using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.Managers
{
    public sealed class OptionsManager : MonoBehaviour
    {
        #region Events
        public delegate int SelectedResolutionIndex();
        public event SelectedResolutionIndex OnSelectedResolutionIndex;

        public delegate int SelectedFullscreenIndex();
        public event SelectedFullscreenIndex OnSelectedFullscreenIndex;

        public delegate int SelectedVSync();
        public event SelectedVSync OnSelectedVSync;

        public delegate int SelectedTargetFrameRateIndex();
        public event SelectedTargetFrameRateIndex OnSelectedTargetFrameRateIndex;
        #endregion

        [Header("Classes")]
        [SerializeField] private UniversalRenderPipelineAsset urpAsset;
        [SerializeField] private UniversalRendererData urpAssetData;

        [Space(10)]

        [SerializeField] private VolumeProfile posprocessingProfile;

        private Resolution[] _clientSupportedResolutions;
        
        private void Awake() 
        {
            _clientSupportedResolutions = GetClientSupportedResolutions();
        }

        public Resolution[] GetClientSupportedResolutions()
        {
            return Screen.resolutions;
        }

        public Resolution GetClientCurrentResolution()
        {
            return Screen.currentResolution;
        }

        public int GetClientCurrentFullscreenModeIndex()
        {
            switch(Screen.fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    return 0;
                case FullScreenMode.FullScreenWindow:
                    return 1;
                case FullScreenMode.MaximizedWindow:
                    return 2;
                case FullScreenMode.Windowed:
                    return 3;
            }

            return 0;
        }

        public int GetClientCurrentVSyncIndex()
        {
            if(QualitySettings.vSyncCount == 1)
            {
                return 1;
            }

            return 0;
        }

        public int GetClientTargetFrameRate()
        {
            return Application.targetFrameRate;
        }

        public int GetClientTextures()
        {
            return QualitySettings.globalTextureMipmapLimit;
        }

        public int GetClientShadowQuality()
        {
            if(!urpAsset.supportsMainLightShadows)
            {
                return 0;
            }

            switch(urpAsset.mainLightShadowmapResolution)
            {
                case 256:
                    return 1;
                case 512:
                    return 2;
                case 1024:
                    return 3;
                case 4096:
                    return 4;
            }

            return 0;
        }

        public bool GetClientBloomStatus()
        {
            for(int i = 0; i < posprocessingProfile.components.Count; i++)
            {
                if(posprocessingProfile.components[i].name == "Bloom")
                {
                    return posprocessingProfile.components[i].active;
                }
            }

            return false;
        }

        public bool GetClientAntialiasingStatus()
        {
            if(urpAsset.msaaSampleCount > 1)
            {
                return true;
            }

            return false;
        }

        public bool GetClientAmbientOcclusion()
        {
            return urpAssetData.rendererFeatures[0].isActive;
        }

        public bool GetClientVolumetricLight()
        {
            return urpAssetData.rendererFeatures[1].isActive;
        }

        public void SaveButton()
        {
            ApplyVideoSettings();
        }

        private void ApplyVideoSettings()
        {
            FullScreenMode selectedFullScreenMode = FullScreenMode.ExclusiveFullScreen;

            switch (OnSelectedFullscreenIndex())
            {
                case 0:
                    selectedFullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    selectedFullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2:
                    selectedFullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
                case 3:
                    selectedFullScreenMode = FullScreenMode.Windowed;
                    break;
            }

            Screen.SetResolution(_clientSupportedResolutions[OnSelectedResolutionIndex()].width,
                _clientSupportedResolutions[OnSelectedResolutionIndex()].height, selectedFullScreenMode);

            QualitySettings.vSyncCount = OnSelectedVSync();

            Application.targetFrameRate = OnSelectedTargetFrameRateIndex();
        }
    }
}
