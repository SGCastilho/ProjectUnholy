using Core.Audio;
using Core.Character;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class PursuerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public Transform PursuerTransform { get => _transform; }
        public CharacterMovement Movement { get => movement; }
        public PursuerAnimation Animation { get => anim; }
        public LocalSoundEffects SFXManager { get => localSoundEffects; }

        internal EnemyData Data { get => data; }

        public int Damage { get => data.Damage; }
        #endregion

        [Header("Data")]
        [SerializeField] private EnemyData data;

        [Header("Classes")]
        [SerializeField] private PursuerAnimation anim;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private LocalSoundEffects localSoundEffects;

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
