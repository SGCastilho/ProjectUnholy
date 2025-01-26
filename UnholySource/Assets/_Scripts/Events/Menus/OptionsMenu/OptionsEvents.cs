using Core.UI;
using Core.Managers;
using UnityEngine;

namespace Core.Events
{
    public sealed class OptionsEvents : MonoBehaviour
    {
        #region Constants
        private const string TAG_IDENTIFIER = "OptionsMenu";
        #endregion

        [Header("Classes")]
        [SerializeField] private GameObject optionsManagerPrefab;
        [SerializeField] private GameObject UI_optionsMenuPrefab;

        private OptionsManager _optionsManager;
        private UI_OptionsMenu _uiOptionsMenu;

        private void Awake() 
        {
            SpawnUI();
            SpawnManager();
        }

        private void OnEnable()
        {
            EnableVideoSettings();
            EnableGraphicsSettings();

            _uiOptionsMenu.OnGetClientSoundTrackVolume += _optionsManager.GetClientSoundTrackVolume;
            _uiOptionsMenu.OnGetClientSoundEffectVolume += _optionsManager.GetClientSoundEffectVolume;

            _optionsManager.OnSelectedSoundTrackVolume += _uiOptionsMenu.SelectedSoundTrackVolume;
            _optionsManager.OnSelectedSoundEffectVolume += _uiOptionsMenu.SelectedSoundEffectVolume;
        }

        private void EnableGraphicsSettings()
        {
            _uiOptionsMenu.OnGetClientTextureQuality += _optionsManager.GetClientTextures;
            _uiOptionsMenu.OnGetClientShadowQuality += _optionsManager.GetClientShadowQuality;
            _uiOptionsMenu.OnGetClientBloomState += _optionsManager.GetClientBloomStatus;
            _uiOptionsMenu.OnGetClientAntialiasingState += _optionsManager.GetClientAntialiasingStatus;
            _uiOptionsMenu.OnGetClientAmbientOcclusion += _optionsManager.GetClientAmbientOcclusion;
            _uiOptionsMenu.OnGetClientVolumetricLight += _optionsManager.GetClientVolumetricLight;

            _optionsManager.OnSelectedTextureQuality += _uiOptionsMenu.SelectedTextureQualityIndex;
            _optionsManager.OnSelectedShadowQuality += _uiOptionsMenu.SelectedShadowQualityIndex;
            _optionsManager.OnSelectedBloom += _uiOptionsMenu.SelectedBloom;
            _optionsManager.OnSelectedAntialiasing += _uiOptionsMenu.SelectedAntialiasing;
            _optionsManager.OnSelectedAmbientOcclusion += _uiOptionsMenu.SelectedAmbientOcclusion;
            _optionsManager.OnSelectedVolumetricLight += _uiOptionsMenu.SelectedVolumetricLight;
        }

        private void EnableVideoSettings()
        {
            _uiOptionsMenu.OnGetClientSupportedResolutions += _optionsManager.GetClientSupportedResolutions;
            _uiOptionsMenu.OnGetClientCurrentResolution += _optionsManager.GetClientCurrentResolution;

            _uiOptionsMenu.OnGetClientCurrentFullscreenModeIndex += _optionsManager.GetClientCurrentFullscreenModeIndex;

            _uiOptionsMenu.OnGetClientVSyncIndex += _optionsManager.GetClientCurrentVSyncIndex;

            _uiOptionsMenu.OnGetClientTargetFrameRate += _optionsManager.GetClientTargetFrameRate;

            _optionsManager.OnSelectedResolutionIndex += _uiOptionsMenu.SelectedResolutionIndex;
            _optionsManager.OnSelectedFullscreenIndex += _uiOptionsMenu.SelectedFullscreenModeIndex;
            _optionsManager.OnSelectedVSync += _uiOptionsMenu.SelectedVSyncIndex;
            _optionsManager.OnSelectedTargetFrameRateIndex += _uiOptionsMenu.SelectedTargetFrameRate;
        }

        private void OnDisable()
        {
            DisableVideoSettigs();
            DisableGraphicsSettings();

            _uiOptionsMenu.OnGetClientSoundTrackVolume -= _optionsManager.GetClientSoundTrackVolume;
            _uiOptionsMenu.OnGetClientSoundEffectVolume -= _optionsManager.GetClientSoundEffectVolume;

            _optionsManager.OnSelectedSoundTrackVolume -= _uiOptionsMenu.SelectedSoundTrackVolume;
            _optionsManager.OnSelectedSoundEffectVolume -= _uiOptionsMenu.SelectedSoundEffectVolume;
        }

        private void DisableGraphicsSettings()
        {
            _uiOptionsMenu.OnGetClientTextureQuality -= _optionsManager.GetClientTextures;
            _uiOptionsMenu.OnGetClientShadowQuality -= _optionsManager.GetClientShadowQuality;
            _uiOptionsMenu.OnGetClientBloomState -= _optionsManager.GetClientBloomStatus;
            _uiOptionsMenu.OnGetClientAntialiasingState -= _optionsManager.GetClientAntialiasingStatus;
            _uiOptionsMenu.OnGetClientAmbientOcclusion -= _optionsManager.GetClientAmbientOcclusion;
            _uiOptionsMenu.OnGetClientVolumetricLight -= _optionsManager.GetClientVolumetricLight;

            _optionsManager.OnSelectedTextureQuality -= _uiOptionsMenu.SelectedTextureQualityIndex;
            _optionsManager.OnSelectedShadowQuality -= _uiOptionsMenu.SelectedShadowQualityIndex;
            _optionsManager.OnSelectedBloom -= _uiOptionsMenu.SelectedBloom;
            _optionsManager.OnSelectedAntialiasing -= _uiOptionsMenu.SelectedAntialiasing;
            _optionsManager.OnSelectedAmbientOcclusion -= _uiOptionsMenu.SelectedAmbientOcclusion;
            _optionsManager.OnSelectedVolumetricLight -= _uiOptionsMenu.SelectedVolumetricLight;
        }

        private void DisableVideoSettigs()
        {
            _uiOptionsMenu.OnGetClientSupportedResolutions -= _optionsManager.GetClientSupportedResolutions;
            _uiOptionsMenu.OnGetClientCurrentResolution -= _optionsManager.GetClientCurrentResolution;

            _uiOptionsMenu.OnGetClientCurrentFullscreenModeIndex -= _optionsManager.GetClientCurrentFullscreenModeIndex;

            _uiOptionsMenu.OnGetClientVSyncIndex -= _optionsManager.GetClientCurrentVSyncIndex;

            _uiOptionsMenu.OnGetClientTargetFrameRate -= _optionsManager.GetClientTargetFrameRate;

            _optionsManager.OnSelectedResolutionIndex -= _uiOptionsMenu.SelectedResolutionIndex;
            _optionsManager.OnSelectedFullscreenIndex -= _uiOptionsMenu.SelectedFullscreenModeIndex;
            _optionsManager.OnSelectedVSync -= _uiOptionsMenu.SelectedVSyncIndex;
            _optionsManager.OnSelectedTargetFrameRateIndex -= _uiOptionsMenu.SelectedTargetFrameRate;
        }

        private void SpawnUI()
        {
            _uiOptionsMenu = FindObjectOfType<UI_OptionsMenu>();

            if(_uiOptionsMenu == null)
            {
                Debug.Log("Instanciando Options UI");

                _uiOptionsMenu = Instantiate(UI_optionsMenuPrefab).GetComponentInChildren<UI_OptionsMenu>();
            }
        }

        private void SpawnManager()
        {
            _optionsManager = FindObjectOfType<OptionsManager>();

            if(_optionsManager == null)
            {
                Debug.Log("Instanciando Options Manager");

                _optionsManager = Instantiate(optionsManagerPrefab).GetComponentInChildren<OptionsManager>();
            }
        }
    }
}
