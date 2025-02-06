using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.Managers
{
    public class SettingsSaved
    {
        #region Encapsulation
        public Resolution ClientResolution { get => _clientResolution; internal set => _clientResolution = value; }
        public FullScreenMode ClientFullScreenMode { get => _clientFullScreenMode; internal set => _clientFullScreenMode = value; }
        public int ClientVSync { get => clientVSync; internal set => clientVSync = value; }
        public int ClientTargetFrameRate { get => clientTargetFrameRate; internal set => clientTargetFrameRate = value; }

        public int ClientTextureQuality { get => clientTextureQuality; internal set => clientTextureQuality = value; }
        public AnisotropicFiltering ClientAnisotropicFiltering { get => _clientAnisotropicFiltering; internal set => _clientAnisotropicFiltering = value; }
        public int ClientShadowQuality { get => clientShadowQuality; internal set => clientShadowQuality = value; }
        public bool ClientBloomActived { get => clientBloomActived; internal set => clientBloomActived = value; }
        public bool ClientAntialiasingActived { get => clientAntialisaingActived; internal set => clientAntialisaingActived = value; }
        public bool ClientAmbientOcclusionActived { get => clientAmbientOcclusionActived; internal set => clientAmbientOcclusionActived = value; }
        public bool ClientVolumetricLightActived { get => clientVolumetricLightActived; internal set => clientVolumetricLightActived = value; }

        public float ClientSoundTrackVolume { get => clientSoundTrackVolume; internal set => clientSoundTrackVolume = value; }
        public float ClientSoundEffectsVolume { get => clientSoundEffectsVolume; internal set => clientSoundEffectsVolume = value; }

        public int ClientLanguage { get => clientLanguage; internal set => clientLanguage = value; }

        public string ClientVersion { get => clientVersion; internal set => clientVersion = value; }
        #endregion

        public int clientResolutionWidth;
        public int clientResolutionHeight;
        public int clientFullScreenModeIndex;
        public int clientVSync;
        public int clientTargetFrameRate;

        public int clientTextureQuality;
        public int clientAnisotropicFilteringIndex;
        public int clientShadowQuality;
        public bool clientBloomActived;
        public bool clientAntialisaingActived;
        public bool clientAmbientOcclusionActived;
        public bool clientVolumetricLightActived;

        public float clientSoundTrackVolume;
        public float clientSoundEffectsVolume;

        public int clientLanguage;

        public string clientVersion;

        private Resolution _clientResolution;
        private FullScreenMode _clientFullScreenMode;
        private AnisotropicFiltering _clientAnisotropicFiltering;
    }

    public sealed class SettingsLoader : MonoBehaviour
    {
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

        [Space(10)]

        [SerializeField] private AudioMixer soundTrackAudioMixer;
        [SerializeField] private AudioMixer soundEffectsAudioMixer;

        private SettingsSaved _savedSettings;

        public void CreateSettingsFile(int selectedLanguage)
        {
            _savedSettings = new SettingsSaved();

            _savedSettings.ClientResolution = Screen.currentResolution;
            _savedSettings.ClientFullScreenMode = Screen.fullScreenMode;
            _savedSettings.ClientVSync = QualitySettings.vSyncCount;
            _savedSettings.ClientTargetFrameRate = Application.targetFrameRate;

            _savedSettings.ClientTextureQuality = QualitySettings.globalTextureMipmapLimit;
            _savedSettings.ClientAnisotropicFiltering = QualitySettings.anisotropicFiltering;

            for(int i = 0; i < shadowPresets.Length; i++)
            {
                if(shadowPresets[i] == currentUrpAsset)
                {
                    _savedSettings.ClientShadowQuality = i;
                    break;
                }
            }

            for (int i = 0; i < posprocessingProfile.components.Count; i++)
            {
                if (posprocessingProfile.components[i].name == "Bloom")
                {
                    _savedSettings.ClientBloomActived = posprocessingProfile.components[i].active;
                    break;
                }
            }

            if(currentUrpAsset.msaaSampleCount > 1)
            {
                _savedSettings.ClientAntialiasingActived = true;
            }
            else
            {
                _savedSettings.ClientAntialiasingActived = false;
            }

            _savedSettings.ClientAmbientOcclusionActived = urpAssetData.rendererFeatures[0].isActive;
            _savedSettings.ClientVolumetricLightActived = urpAssetData.rendererFeatures[0].isActive;

            _savedSettings.clientSoundTrackVolume = 0f;
            _savedSettings.ClientSoundEffectsVolume = 0f;

            _savedSettings.ClientLanguage = selectedLanguage;

            CreateSettingsJSON(_savedSettings);
        }

        public void CreateSettingsJSON(SettingsSaved settings)
        {
            //TEMPORARIO, SUBSTIUIR ISSO DE ACORDO COM OQUE O JOGO ESCOLHER NO MENU DE OPÇOES
            int clientLanguange = 0;

            if(File.Exists(Application.persistentDataPath + "/config.txt"))
            {
                var getCurrentSettings = GetSettingsFile();

                clientLanguange = getCurrentSettings.ClientLanguage;

                File.Delete(Application.persistentDataPath + "/config.txt");
            }

            settings.clientResolutionHeight = settings.ClientResolution.height;
            settings.clientResolutionWidth = settings.ClientResolution.width;

            switch(settings.ClientFullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    settings.clientFullScreenModeIndex = 0;
                    break;
                case FullScreenMode.FullScreenWindow:
                    settings.clientFullScreenModeIndex = 1;
                    break;
                case FullScreenMode.MaximizedWindow:
                    settings.clientFullScreenModeIndex = 2;
                    break;
                case FullScreenMode.Windowed:
                    settings.clientFullScreenModeIndex = 3;
                    break;
            }

            switch(settings.ClientAnisotropicFiltering)
            {
                case AnisotropicFiltering.ForceEnable:
                    settings.clientAnisotropicFilteringIndex = 0;
                    break;
                case AnisotropicFiltering.Enable:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    settings.clientAnisotropicFilteringIndex = 1;
                    break;
                case AnisotropicFiltering.Disable:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    settings.clientAnisotropicFilteringIndex = 2;
                    break;
            }         

            settings.ClientLanguage = clientLanguange;
            settings.ClientVersion = "Pre-Alpha";

            string jsonOutput = JsonUtility.ToJson(settings, true);

            File.WriteAllText(Application.persistentDataPath + "/config.txt", jsonOutput);
        }

        public void LoadSettings()
        {
            _savedSettings = new SettingsSaved();

            if(File.Exists(Application.persistentDataPath + "/config.txt"))
            {
                _savedSettings = GetSettingsFile();

                Screen.SetResolution(_savedSettings.ClientResolution.width, _savedSettings.ClientResolution.height, 
                    _savedSettings.ClientFullScreenMode);
                
                QualitySettings.vSyncCount = _savedSettings.ClientVSync;

                Application.targetFrameRate = _savedSettings.ClientTargetFrameRate;

                QualitySettings.globalTextureMipmapLimit = _savedSettings.ClientTextureQuality;

                QualitySettings.anisotropicFiltering = _savedSettings.ClientAnisotropicFiltering;

                QualitySettings.renderPipeline = shadowPresets[_savedSettings.ClientShadowQuality];
                currentUrpAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;

                for (int i = 0; i < posprocessingProfile.components.Count; i++)
                {
                    if (posprocessingProfile.components[i].name == "Bloom")
                    {
                        posprocessingProfile.components[i].active = _savedSettings.ClientBloomActived;
                        break;
                    }
                }

                if(_savedSettings.ClientAntialiasingActived)
                {
                    currentUrpAsset.msaaSampleCount = 4;
                }
                else
                {
                    currentUrpAsset.msaaSampleCount = 1;
                }

                urpAssetData.rendererFeatures[0].SetActive(_savedSettings.ClientAmbientOcclusionActived);
                urpAssetData.rendererFeatures[1].SetActive(_savedSettings.ClientVolumetricLightActived);

                soundTrackAudioMixer.SetFloat(KEY_SOUND_TRACK, _savedSettings.ClientSoundTrackVolume);
                soundEffectsAudioMixer.SetFloat(KEY_SOUND_EFFECT, _savedSettings.ClientSoundEffectsVolume);
            }
        }

        public int GetClientSavedLanguage()
        {
            if(File.Exists(Application.persistentDataPath + "/config.txt"))
            {
                return _savedSettings.ClientLanguage;
            }

            return 0;
        }

        private SettingsSaved GetSettingsFile()
        {
            string jsonFile = string.Empty;
            SettingsSaved loadedSettings = new SettingsSaved();

            Resolution loadedResolution = new Resolution();
            FullScreenMode loadedFullScreenMode = FullScreenMode.ExclusiveFullScreen;
            AnisotropicFiltering loadedAnisotropicFiltering = AnisotropicFiltering.ForceEnable;

            if(File.Exists(Application.persistentDataPath + "/config.txt"))
            {
                jsonFile = File.ReadAllText(Application.persistentDataPath + "/config.txt");
                loadedSettings = JsonUtility.FromJson<SettingsSaved>(jsonFile);

                loadedResolution.height = loadedSettings.clientResolutionHeight;
                loadedResolution.width = loadedSettings.clientResolutionWidth;

                loadedSettings.ClientResolution = loadedResolution;

                switch(loadedSettings.clientFullScreenModeIndex)
                {
                    case 0:
                        loadedSettings.ClientFullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        break;
                    case 1:
                        loadedSettings.ClientFullScreenMode = FullScreenMode.FullScreenWindow;
                        break;
                    case 2:
                        loadedSettings.ClientFullScreenMode = FullScreenMode.MaximizedWindow;
                        break;
                    case 3:
                        loadedSettings.ClientFullScreenMode = FullScreenMode.Windowed;
                        break;
                }

                switch(loadedSettings.clientAnisotropicFilteringIndex)
                {
                    case 0:
                        loadedSettings.ClientAnisotropicFiltering = AnisotropicFiltering.ForceEnable;
                        break;
                    case 1:
                        loadedSettings.ClientAnisotropicFiltering = AnisotropicFiltering.Enable;
                        break;
                    case 2:
                        loadedSettings.ClientAnisotropicFiltering = AnisotropicFiltering.Disable;
                        break;
                    case 3:
                        loadedSettings.ClientAnisotropicFiltering = AnisotropicFiltering.Disable;
                        break;
                }  

                return loadedSettings;
            }

            return null;
        }
    }
}
