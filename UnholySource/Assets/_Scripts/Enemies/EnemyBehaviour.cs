using UnityEngine;

namespace Core.Enemies
{
    public class EnemyBehaviour : MonoBehaviour
    {
        #region Encapsulation
        internal EnemyStatus Status { get => status; }
        #endregion

        [Header("Classes")]
        [SerializeField] private EnemyStatus status;
    }
}
