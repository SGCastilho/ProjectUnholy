using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UI_OptionsMenu : MonoBehaviour
    {
        #region Events

        #region Video Options Events
        public delegate Resolution[] GetClientSupportedResolutions();
        public event GetClientSupportedResolutions OnGetClientSupportedResolutions;

        public delegate Resolution GetClientCurrentResolution();
        public event GetClientCurrentResolution OnGetClientCurrentResolution;

        public delegate int GetClientCurrentFullscreenModeIndex();
        public event GetClientCurrentFullscreenModeIndex OnGetClientCurrentFullscreenModeIndex;

        public delegate int GetClientVSyncIndex();
        public event GetClientVSyncIndex OnGetClientVSyncIndex;

        public delegate int GetClientTargetFrameRate();
        public event GetClientTargetFrameRate OnGetClientTargetFrameRate;
        #endregion

        public delegate int GetClientTextureQuality();
        public event GetClientTextureQuality OnGetClientTextureQuality;

        public delegate int GetClientShadowQuality();
        public event GetClientShadowQuality OnGetClientShadowQuality;

        public delegate bool GetClientBloomState();
        public event GetClientBloomState OnGetClientBloomState;

        public delegate bool GetClientAntialiasingState();
        public event GetClientAntialiasingState OnGetClientAntialiasingState;
        
        public delegate bool GetClientAmbientOcclusion();
        public event GetClientAmbientOcclusion OnGetClientAmbientOcclusion;

        public delegate bool GetClientVolumetricLight();
        public event GetClientVolumetricLight OnGetClientVolumetricLight;

        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup optionsCanvasGroup;
        [SerializeField] private CanvasGroup previousCanvasGroup;

        [Space(10)]

        [SerializeField] private GameObject videoOptionsWindow;
        [SerializeField] private GameObject graphicsOptionsWindow;
        [SerializeField] private GameObject audioOptionsWindow;
        [SerializeField] private GameObject othersOptionsWindow;

        [Header("Video Options Classes")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fullscreenModeDropdown;

        [Space(5)]

        [SerializeField] private Toggle vsyncToggle;

        [Space(5)]

        [SerializeField] private TMP_Dropdown targetFrameRateDropdown;

        [Header("Graphics Options Classes")]
        [SerializeField] private TMP_Dropdown texturesDropdown;
        [SerializeField] private TMP_Dropdown shadowsDropdown;

        [Space(5)]

        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Toggle antialiasingToggle;
        [SerializeField] private Toggle ambientOcclusionToggle;
        [SerializeField] private Toggle volumetricLightToggle;

        private void Awake()
        {
            GetClientVideoOptions();
            SetClientCurrentVideoSettings();

            SetClientCurrentGraphicsSettings();

            ResetToDefault();
        }

        public void OpenOptionsMenu()
        {
            optionsCanvasGroup.DOKill();
            optionsCanvasGroup.DOFade(1f, 0.2f);

            optionsCanvasGroup.blocksRaycasts = true;
        }

        public void CloseOptionsMenu()
        {
            optionsCanvasGroup.DOKill();
            optionsCanvasGroup.DOFade(0f, 0.2f);

            optionsCanvasGroup.blocksRaycasts = false;

            if(previousCanvasGroup != null) { previousCanvasGroup.blocksRaycasts = true; }
        }

        #region Video Functions
        public void VideoOptions()
        {
            videoOptionsWindow.SetActive(true);
            graphicsOptionsWindow.SetActive(false);
            audioOptionsWindow.SetActive(false);
            othersOptionsWindow.SetActive(false);
        }

        private void GetClientVideoOptions()
        {
            GetClientResolutions();
        }

        private void GetClientResolutions()
        {
            Resolution[] supportedResolution = OnGetClientSupportedResolutions?.Invoke();

            List<string> resolutions = new List<string>();

            foreach (Resolution resolution in supportedResolution)
            {
                resolutions.Add($"{resolution.width}x{resolution.height}");
            }

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutions);
        }

        private void SetClientCurrentVideoSettings()
        {
            SetClientCurrentResolution();

            fullscreenModeDropdown.value = OnGetClientCurrentFullscreenModeIndex();

            if (OnGetClientVSyncIndex() == 1)
            {
                vsyncToggle.isOn = true;
            }
            else
            {
                vsyncToggle.isOn = false;
            }

            SetClientTargetFrameRate();
        }

        private void SetClientTargetFrameRate()
        {
            switch (OnGetClientTargetFrameRate())
            {
                case 30:
                    targetFrameRateDropdown.value = 0;
                    break;
                case 60:
                    targetFrameRateDropdown.value = 1;
                    break;
                case 120:
                    targetFrameRateDropdown.value = 2;
                    break;
                case 144:
                    targetFrameRateDropdown.value = 3;
                    break;
                case 240:
                    targetFrameRateDropdown.value = 4;
                    break;
                case -1:
                    targetFrameRateDropdown.value = 5;
                    break;
            }
        }

        private void SetClientCurrentResolution()
        {
            string clientResolution = $"{OnGetClientCurrentResolution().width}x{OnGetClientCurrentResolution().height}";

            for (int i = 0; i < resolutionDropdown.options.Count; i++)
            {
                if (resolutionDropdown.options[i].text == clientResolution)
                {
                    resolutionDropdown.value = i;
                    break;
                }
            }
        }

        public int SelectedResolutionIndex()
        {
            return resolutionDropdown.value;
        }

        public int SelectedFullscreenModeIndex()
        {
            return fullscreenModeDropdown.value;
        }

        public int SelectedVSyncIndex()
        {
            if(vsyncToggle.isOn)
            {
                return 1;
            }

            return 0;
        }

        public int SelectedTargetFrameRate()
        {
            switch(targetFrameRateDropdown.value)
            {
                case 0:
                    return 30;
                case 1:
                    return 60;
                case 2:
                    return 120;
                case 3:
                    return 144;
                case 4:
                    return 240;
                case 5:
                    return -1;
            }

            return 0;
        }
        #endregion

        #region Graphics Functions
        public void GraphicsOptions()
        {
            videoOptionsWindow.SetActive(false);
            graphicsOptionsWindow.SetActive(true);
            audioOptionsWindow.SetActive(false);
            othersOptionsWindow.SetActive(false);
        }

        private void SetClientCurrentGraphicsSettings()
        {
            texturesDropdown.value = OnGetClientTextureQuality();
            shadowsDropdown.value = OnGetClientShadowQuality();

            bloomToggle.isOn = OnGetClientBloomState();
            antialiasingToggle.isOn = OnGetClientAntialiasingState();
            ambientOcclusionToggle.isOn = OnGetClientAmbientOcclusion();
            volumetricLightToggle.isOn = OnGetClientVolumetricLight();
        }
        #endregion

        public void AudioOptions()
        {
            videoOptionsWindow.SetActive(false);
            graphicsOptionsWindow.SetActive(false);
            audioOptionsWindow.SetActive(true);
            othersOptionsWindow.SetActive(false);
        }

        public void OthersOptions()
        {
            videoOptionsWindow.SetActive(false);
            graphicsOptionsWindow.SetActive(false);
            audioOptionsWindow.SetActive(false);
            othersOptionsWindow.SetActive(true);
        }

        public void ResetToDefault()
        {
            VideoOptions();

            optionsCanvasGroup.alpha = 0f;
            optionsCanvasGroup.blocksRaycasts = false;
        }
    }
}
