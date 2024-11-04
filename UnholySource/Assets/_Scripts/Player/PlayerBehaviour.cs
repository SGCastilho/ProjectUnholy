using Core.Character;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public PlayerInputs Inputs { get => inputs; }
        public PlayerResources Resources { get => resources; }

        internal PlayerAttack Attack { get => attack; }
        internal PlayerMovement Movement { get => movement; }
        internal PlayerEquipment Equipment { get => equipment; }
        internal PlayerAnimation Animation { get => animationControl; }

        internal CharacterCorrection Correction { get => charCorrection;}
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerInputs inputs;
        [SerializeField] private PlayerAttack attack;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerResources resources;                                                                 
        [SerializeField] private PlayerEquipment equipment;
        [SerializeField] private PlayerAnimation animationControl;

        [Space(10)]

        [SerializeField] private CharacterCorrection charCorrection;

        private void Start() => HideCursor(true);

        public void HideCursor(bool hide)
        {
            if(hide)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
