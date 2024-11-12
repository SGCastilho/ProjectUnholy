using Core.Character;
using UnityEngine;

namespace Core.Enemies
{
    [RequireComponent(typeof(CharacterMovement))]
    public sealed class EnemyMeleeBehaviour : EnemyBehaviour
    {
        #region Encapsulation
        public CharacterMovement Movement { get => charMovement; }
        #endregion

        [Header("Exclusive Classes")]
        [SerializeField] private CharacterMovement charMovement;
    }
}
