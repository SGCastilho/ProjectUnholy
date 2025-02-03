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

        private void OnEnable() 
        {
            cameraViewerObject.SetActive(false);
        }

        public void CallViewer()
        {
            triggerInteraction.Player.Inputs.AllowViewerInputs();
            triggerInteraction.Player.Inputs.SubscribeViewer(UnCallViewer);

            cameraViewerObject.SetActive(true);
        }

        public void UnCallViewer()
        {
            OnViewerExit?.Invoke();

            cameraViewerObject.SetActive(false);

            triggerInteraction.Player.Inputs.UnsubscribeViewer();
            triggerInteraction.Player.Inputs.BlockViewerInputs();
        }
    }
}
