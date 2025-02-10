using Core.Audio;
using Core.Character;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public PlayerStatus Status { get => status; }
        public PlayerInputs Inputs { get => inputs; }
        public PlayerActions Actions { get => actions; }
        public PlayerResources Resources { get => resources; }
        public PlayerEquipment Equipment { get => equipment; }

        public LocalSoundEffects SFXManager { get => localSoundEffects; }
        
        internal PlayerMovement Movement { get => movement; }
        internal PlayerAnimation Animation { get => animationControl; }

        internal CharacterCorrection Correction { get => charCorrection; }

        internal Transform PlayerTransform { get => _transform; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerStatus status;
        [SerializeField] private PlayerInputs inputs;
        [SerializeField] private PlayerActions actions;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerResources resources;                                                                 
        [SerializeField] private PlayerEquipment equipment;
        [SerializeField] private PlayerAnimation animationControl;

        [Space(10)]

        [SerializeField] private CharacterCorrection charCorrection;

        [Space(10)]

        [SerializeField] private LocalSoundEffects localSoundEffects;

        private Transform _transform;

        private void Awake() 
        {
            _transform = GetComponent<Transform>();
        }

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

        public void FlipPlayer(bool isFlipped)
        {
            movement.IsFlipped = isFlipped;
        }

        public bool LookingToRight()
        {
            return !movement.IsFlipped;
        }

        public Vector3 GetCurrentPosistion()
        {
            return _transform.position;
        }
    }
}
