using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIMainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Classes")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI tmpPro;

        [Header("Settings")]
        [SerializeField] private TMP_ColorGradient defaultColors;
        [SerializeField] private TMP_ColorGradient highlightColors;
        [SerializeField] private TMP_ColorGradient disableColors;

        [Space(10)]

        [SerializeField] private float horizontalMovementX = 342f;

        private RectTransform rectTransform;

        private float startXPosistion;

        private void Awake() 
        {
            rectTransform = GetComponent<RectTransform>();

            startXPosistion = rectTransform.anchoredPosition.x;
        }

        private void Start() 
        {
            if(button.interactable == false) { tmpPro.colorGradientPreset = disableColors; }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(button.interactable == false) return;

            rectTransform.DOKill();
            rectTransform.DOAnchorPosX(horizontalMovementX, 0.2f);

            tmpPro.colorGradientPreset = highlightColors;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(button.interactable == false) return;

            rectTransform.DOKill();
            rectTransform.DOAnchorPosX(startXPosistion, 0.2f);

            tmpPro.colorGradientPreset = defaultColors;
        }
    }
}
