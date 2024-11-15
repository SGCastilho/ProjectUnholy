using Core.Character;
using UnityEngine;

namespace Core.Enemies
{
    [RequireComponent(typeof(CharacterMovement))]
    public sealed class EnemyMeleeBehaviour : EnemyBehaviour
    {
        #region Encapsulation
        public EnemyMeleeAnimations Animation { get => animations; }

        internal CharacterMovement Movement { get => charMovement; }
        internal CharacterCorrection Correction { get => charCorrection; }
        #endregion

        [Header("Exclusive Classes")]
        [SerializeField] private EnemyMeleeAnimations animations;

        [Space(10)]

        [SerializeField] private CharacterMovement charMovement;
        [SerializeField] private CharacterCorrection charCorrection;

        internal override void Death()
        {
            base.Death();

            charMovement.enabled = false;
        }
    }
}
