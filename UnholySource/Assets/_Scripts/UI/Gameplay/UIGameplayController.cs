using Core.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public sealed class UIGameplayController : MonoBehaviour
    {
        #region Events
        public delegate void PlaySFX(AudioClip audioClip, float volume);
        public event PlaySFX OnPlaySFX;
        #endregion

        #region Constants
        private const string SFX_CANCEL_KEY = "sfx_cancel";
        private const string SFX_CONFIRM_KEY = "sfx_confirm";
        #endregion

        #region Encapsulation
        public UIRangedWeapon UI_RangedWeapon { get => uIRangedWeapon; }
        public UIHealingBottles UI_HealingBottles { get => uIHealingBottles; }
        public UIHurtAlertOverlay UI_HurtAlertOverlay { get => uIHurtAlertOverlay; }

        public UIInventory UI_Inventory { get => uIInventory; }
        public UIPauseGame UI_PauseGame { get => uiPauseGame; }

        public UIItemNotificantion UI_ItemNotification { get => uIItemNotificantion; }
        #endregion

        [Header("Player UI Classes")]
        [SerializeField] private EventSystem eventSystem;

        [Space(10)]

        [SerializeField] private UIRangedWeapon uIRangedWeapon;
        [SerializeField] private UIHealingBottles uIHealingBottles;
        [SerializeField] private UIHurtAlertOverlay uIHurtAlertOverlay;

        [Space(10)]

        [SerializeField] private UIInventory uIInventory;
        [SerializeField] private UIPauseGame uiPauseGame;

        [Space(10)]

        [SerializeField] private UIItemNotificantion uIItemNotificantion;

        [Space(20)]

        [SerializeField] private SFXHandler sfxHandler;

        private AudioClip _confirmSFX;
        private AudioClip _cancelSFX;

        private float _confirmVolume;
        private float _cancelVolume;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _confirmSFX = sfxHandler.GetAudioClipByKey(SFX_CONFIRM_KEY);
            _cancelSFX = sfxHandler.GetAudioClipByKey(SFX_CANCEL_KEY);

            _confirmVolume = sfxHandler.GetVolumeByKey(SFX_CONFIRM_KEY);
            _cancelVolume = sfxHandler.GetVolumeByKey(SFX_CANCEL_KEY);
        }

        public void UnSelectButton()
        {
            eventSystem.SetSelectedGameObject(null);
        }

        public void PlaySFXConfirm()
        {
            OnPlaySFX?.Invoke(_confirmSFX, _confirmVolume);
        }

        public void PlayerSFXCancel()
        {
            OnPlaySFX?.Invoke(_cancelSFX, _cancelVolume);
        }
    }
}
