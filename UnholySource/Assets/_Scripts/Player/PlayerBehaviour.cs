using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        internal PlayerInputs Inputs { get => inputs; }
        internal PlayerMovement Movement { get => movement; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerInputs inputs;
        [SerializeField] private PlayerMovement movement;
    }
}
