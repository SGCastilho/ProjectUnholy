using UnityEngine;

namespace Core.Enemies
{
    public class EnemyBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public EnemyStatus Status { get => status; }

        internal Transform EnemyTransform { get => _transform; }
        #endregion

        [Header("Classes")]
        [SerializeField] private EnemyStatus status;

        private Transform _playerPosistion;

        private Transform _transform;

        internal virtual void Awake() => CacheVariables();

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

        internal virtual void Death()
        {
            _transform.GetComponent<Rigidbody>().isKinematic = true;
            _transform.GetComponent<Collider>().enabled = false;
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
