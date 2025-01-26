using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.Managers
{
    public sealed class OptionsManager : MonoBehaviour
    {
        #region Events

        #region Video Options
        public delegate int SelectedResolutionIndex();
        public event SelectedResolutionIndex OnSelectedResolutionIndex;

        public delegate int SelectedFullscreenIndex();
        public event SelectedFullscreenIndex OnSelectedFullscreenIndex;

        public delegate int SelectedVSync();
        public event SelectedVSync OnSelectedVSync;

        public delegate int SelectedTargetFrameRateIndex();
        public event SelectedTargetFrameRateIndex OnSelectedTargetFrameRateIndex;
        #endregion
        
        #region Graphics Options
        public delegate int SelectedTextureQuality();
        public event SelectedTextureQuality OnSelectedTextureQuality;

        public delegate int SelectedShadowQuality();
        public event SelectedShadowQuality OnSelectedShadowQuality;

        public delegate bool SelectedBloom();
        public event SelectedBloom OnSelectedBloom;

        public delegate bool SelectedAntialiasing();
        public event SelectedAntialiasing OnSelectedAntialiasing;

        public delegate bool SelectedAmbientOcclusion();
        public event SelectedAmbientOcclusion OnSelectedAmbientOcclusion;

        public delegate bool SelectedVolumetricLight();
        public event SelectedVolumetricLight OnSelectedVolumetricLight;
        #endregion

        #region Sound Options
        public delegate float SelectedSoundEffectVolume();
        public event SelectedSoundEffectVolume OnSelectedSoundEffectVolume;
        
        public delegate float SelectedSoundTrackVolume();
        public event SelectedSoundTrackVolume OnSelectedSoundTrackVolume;
        #endregion

        #endregion

        #region Constants
        private const string KEY_SOUND_TRACK = "SoundTrack";
        private const string KEY_SOUND_EFFECT = "SoundEffects";
        #endregion

        [Header("Classes")]
        [SerializeField] private UniversalRenderPipelineAsset currentUrpAsset;
        [SerializeField] private UniversalRendererData urpAssetData;

        [Space(10)]

        [SerializeField] private UniversalRenderPipelineAsset[] shadowPresets;

        [Space(10)]

        [SerializeField] private VolumeProfile posprocessingProfile;
        
        [SerializeField] private AudioMixer soundTrackAudioMixer;
        [SerializeField] private AudioMixer soundEffectsAudioMixer;

        private Resolution[] _clientSupportedResolutions;

        private float _soundTrackVolume;
        private float _soundEffectVolume;

        private void Awake() 
        {
            currentUrpAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;

            _clientSupportedResolutions = GetClientSupportedResolutions();

            soundTrackAudioMixer.GetFloat(KEY_SOUND_TRACK, out _soundTrackVolume);
            soundEffectsAudioMixer.GetFloat(KEY_SOUND_EFFECT, out _soundEffectVolume);
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
            if(!currentUrpAsset.supportsMainLightShadows)
            {
                return 0;
            }

            switch(currentUrpAsset.mainLightShadowmapResolution)
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
            if(currentUrpAsset.msaaSampleCount > 1)
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

        public float GetClientSoundTrackVolume()
        {
            return _soundTrackVolume;
        }

        public float GetClientSoundEffectVolume()
        {
            return _soundEffectVolume;
        }

        public void SaveButton()
        {
            ApplyVideoSettings();
            ApplyGraphicsSettings();
            ApplySoundSettings();
        }

        private void ApplySoundSettings()
        {
            soundTrackAudioMixer.SetFloat(KEY_SOUND_TRACK, OnSelectedSoundTrackVolume());
            soundEffectsAudioMixer.SetFloat(KEY_SOUND_EFFECT, OnSelectedSoundEffectVolume());
        }

        private void ApplyGraphicsSettings()
        {
            QualitySettings.globalTextureMipmapLimit = OnSelectedTextureQuality();

            switch (OnSelectedTextureQuality())
            {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 3:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
            }

            QualitySettings.renderPipeline = shadowPresets[OnSelectedShadowQuality()];

            currentUrpAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;

            for (int i = 0; i < posprocessingProfile.components.Count; i++)
            {
                if (posprocessingProfile.components[i].name == "Bloom")
                {
                    posprocessingProfile.components[i].active = OnSelectedBloom();
                    break;
                }
            }

            if (OnSelectedAntialiasing())
            {
                currentUrpAsset.msaaSampleCount = 4;
            }
            else
            {
                currentUrpAsset.msaaSampleCount = 1;
            }

            urpAssetData.rendererFeatures[0].SetActive(OnSelectedAmbientOcclusion());
            urpAssetData.rendererFeatures[1].SetActive(OnSelectedVolumetricLight());
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
