using UnityEngine;
using UnityEngine.Events;

namespace Core.Triggers
{
    [RequireComponent(typeof(TriggerInteraction))]
    public sealed class ViewerInteractionTrigger : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private TriggerInteraction triggerInteraction;

        [Space(6)]

        [SerializeField] private GameObject cameraViewerObject;

        [Header("Events")]
        [Space(10)]

        [SerializeField] private UnityEvent OnViewerExit;

        private bool _inViewer;

        private void OnEnable() 
        {
            cameraViewerObject.SetActive(false);
        }

        public void CallViewer()
        {
            if(_inViewer) return;

            triggerInteraction.Player.Inputs.AllowViewerInputs();
            triggerInteraction.Player.Inputs.SubscribeViewer(UnCallViewer);

            cameraViewerObject.SetActive(true);

            _inViewer = true;
        }

        public void UnCallViewer()
        {
            if(!_inViewer) return;

            OnViewerExit?.Invoke();

            cameraViewerObject.SetActive(false);

            triggerInteraction.Player.Inputs.UnsubscribeViewer();
            triggerInteraction.Player.Inputs.BlockViewerInputs();

            _inViewer = false;
        }
    }
}
