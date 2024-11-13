using Core.Character;
using UnityEngine;

namespace Core.Enemies
{
    [RequireComponent(typeof(CharacterMovement))]
    public sealed class EnemyMeleeBehaviour : EnemyBehaviour
    {
        #region Encapsulation
        internal EnemyMeleeAnimations Animation { get => animations; }

        public CharacterMovement Movement { get => charMovement; }
        #endregion

        [Header("Exclusive Classes")]
        [SerializeField] private EnemyMeleeAnimations animations;

        [Space(10)]

        [SerializeField] private CharacterMovement charMovement;
    }
}
