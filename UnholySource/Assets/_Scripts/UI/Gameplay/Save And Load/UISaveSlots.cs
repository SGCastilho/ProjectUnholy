using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UISaveSlots : MonoBehaviour
    {
        #region Events
        public delegate void SaveFile(int slotIndex);
        public event SaveFile OnSaveFile;

        public delegate bool SavingState();
        public event SavingState OnSavingState;

        public delegate string[] SlotInfo(int slotIndex);
        public event SlotInfo OnGetSlotInfo;

        public delegate Texture2D SlotScreenshoot(int slotIndex);
        public event SlotScreenshoot OnGetSlotScreenshoot;

        public delegate void LoadFile(int slotIndex);
        public event LoadFile OnLoadFile;
        #endregion

        #region Encapsulation
        internal int Index { set => slotIndex = value; }
        internal bool IsSaving { get => isSaving; set => isSaving = value; }
        #endregion

        [Header("Classes")]
        [SerializeField] private TextMeshProUGUI chapterNameTMP;

        [Space(5)]

        [SerializeField] private GameObject saveInfoGroup;
        [SerializeField] private TextMeshProUGUI playTimeTMP;
        [SerializeField] private TextMeshProUGUI saveDateTMP;

        [Space(10)]

        [SerializeField] private Image screenShootImage;

        [Header("Settings")]
        [SerializeField] private int slotIndex;

        [Space(10)]

        [SerializeField] private bool isSaving;

        [Space(10)]

        [SerializeField] private Color slotOccupedColor = Color.white;
        [SerializeField] private Color slotEmptyColor;

        [Space(6)]

        [SerializeField] private Sprite emptySlotSprite;

        private string[] _slotInfo;
        private Texture2D _slotScreenshoot;

        void Awake()
        {
            _slotInfo = new string[3];
        }

        void OnEnable()
        {
            _slotInfo = OnGetSlotInfo?.Invoke(slotIndex);
            //_slotScreenshoot = OnGetSlotScreenshoot?.Invoke(slotIndex);

            if(_slotInfo != null)
            {
                OccupedSlotContent();
            }
            else
            {
                EmptySlotContent();
            }
        }

        public void ExecuteAction()
        {
            if(isSaving)
            {
                if(OnSavingState?.Invoke() == true) return;

                OnSaveFile?.Invoke(slotIndex);

                StartCoroutine(CheckIfSaveEnds());
            }
            else
            {
                OnLoadFile?.Invoke(slotIndex);
            }
        }

        private IEnumerator CheckIfSaveEnds()
        {
            while(OnSavingState?.Invoke() == true)
            {
                yield return null;
            }

            _slotInfo = OnGetSlotInfo?.Invoke(slotIndex);
            //_slotScreenshoot = OnGetSlotScreenshoot?.Invoke(slotIndex);

            if(_slotInfo != null)
            {
                OccupedSlotContent();
            }
        }

        private void OccupedSlotContent()
        {
            chapterNameTMP.text = _slotInfo[0];
            chapterNameTMP.color = slotOccupedColor;

            playTimeTMP.text = "Playtime: " + _slotInfo[1];
            saveDateTMP.text = "Save Date: " + _slotInfo[2];

            saveInfoGroup.SetActive(true);

            //screenShootImage.sprite = Sprite.Create(_slotScreenshoot, new Rect(0, 0, _slotScreenshoot.width, _slotScreenshoot.height), Vector2.zero);
        }

        private void EmptySlotContent()
        {
            chapterNameTMP.text = "Empty Slot";
            chapterNameTMP.color = slotEmptyColor;

            saveInfoGroup.SetActive(false);

            screenShootImage.sprite = emptySlotSprite;
        }
    }
}
