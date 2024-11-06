using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIOverworldTriggerIcon : MonoBehaviour
    {
        private enum ArrowDirection { LEFT, RIGHT, UP, DOWN }

        [Header("Classes")]
        [SerializeField] private Image triggerImage;
        [SerializeField] private Canvas triggerCanvas;

        [Header("Settings")]
        [SerializeField] private Sprite interactionSprite;
        [SerializeField] private ArrowDirection arrowDirection;

        [Space(10)]

        [SerializeField] [Range(0.1f, 1f)] private float fadeTime = 0.2f;
        [SerializeField] private float canvasUpMovementY = 2.86f;
        [SerializeField] private float canvasBackMovementY = 2.2f;

        private RectTransform canvasTransform;

        private void Awake() 
        {
            canvasTransform = triggerCanvas.GetComponent<RectTransform>();
        }

        private void OnEnable() 
        {
            if(interactionSprite == null) return;

            triggerImage.sprite = interactionSprite;
        }

        private void Start() 
        {
            switch(arrowDirection)
            {
                case ArrowDirection.LEFT:
                    canvasTransform.eulerAngles = new Vector3(0f, 90f, 0f);
                    canvasTransform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                case ArrowDirection.RIGHT:
                    canvasTransform.eulerAngles = new Vector3(0f, 90f, 0f);
                    canvasTransform.localScale = new Vector3(-1f, 1f, 1f);
                    break;
                case ArrowDirection.UP:
                    canvasTransform.eulerAngles = new Vector3(0f, 90f, 90f);
                    canvasTransform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                case ArrowDirection.DOWN:
                    canvasTransform.eulerAngles = new Vector3(0f, 90f, 90f);
                    canvasTransform.localScale = new Vector3(-1f, 1f, 1f);
                    break;
            }
        }

        public void ShowInteraction()
        {
            triggerImage.DOKill();
            triggerCanvas.DOKill();

            triggerImage.DOFade(1f, fadeTime);
            triggerCanvas.transform.DOLocalMoveY(canvasUpMovementY, fadeTime);
        }

        public void HideInteraction()
        {
            triggerImage.DOKill();
            triggerCanvas.DOKill();

            triggerImage.DOFade(0f, fadeTime);
            triggerCanvas.transform.DOLocalMoveY(canvasBackMovementY, fadeTime);
        }
    }
}
