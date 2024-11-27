using Core.Character;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class PredatorBehaviour : MonoBehaviour
    {
        #region Encapsulation
        internal CharacterMovement Movement { get => charMovement; }
        internal PredatorAnimation Animation { get => charAnimation; }
        #endregion

        [Header("Classes")]
        [SerializeField] private CharacterMovement charMovement;

        [Space(10)]

        [SerializeField] private PredatorAnimation charAnimation;

        private Transform _playerPosistion;

        private Transform _transform;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            if(_transform == null)
            {
                _transform = transform;
            }

            if (_playerPosistion == null)
            { 
                _playerPosistion = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            }
        }

        public float GetPlayerDistance()
        {
            return Mathf.Abs(_transform.position.z - _playerPosistion.position.z);
        }

        public bool GetPlayerSide()
        {
            //False significa mover para a direita e true para a esquerda

            if(_transform.position.z < _playerPosistion.position.z)
            {
                return true;
            }

            return false;
        }
    }
}
