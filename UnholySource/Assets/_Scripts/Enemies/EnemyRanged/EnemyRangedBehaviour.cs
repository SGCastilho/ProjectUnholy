using Core.Character;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedBehaviour : EnemyBehaviour
    {
        #region Encapsulation
        public EnemyRangedAnimation Animation { get => enemyRangedAnimation; }

        public CharacterMovement Movement { get => characterMovement; }
        #endregion

        [Header("Exclusive Classes")]
        [SerializeField] private EnemyRangedAnimation enemyRangedAnimation;

        [Space(10)]

        [SerializeField] private CharacterMovement characterMovement;

        private Vector3 _startingPosistion;

        internal override void Awake()
        {
            base.Awake();

            _startingPosistion = _transform.position;
        }

        void OnEnable()
        {
            if(Status.IsDead)
            {
                enemyRangedAnimation.IsDead = Status.IsDead;
            }
        }

        void OnDisable()
        {
            if(!Status.IsDead)
            {
                _transform.position = _startingPosistion;
            }
        }
    }
}
