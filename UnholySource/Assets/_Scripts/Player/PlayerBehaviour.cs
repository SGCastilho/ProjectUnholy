using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        internal PlayerInputs Inputs { get => inputs; }
        internal PlayerMovement Movement { get => movement; }
        internal PlayerAnimation Animation { get => animationControl; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerInputs inputs;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerAnimation animationControl;
    }
}
