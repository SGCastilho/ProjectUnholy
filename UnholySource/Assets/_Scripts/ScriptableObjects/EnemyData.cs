using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/New Enemy Data")]
    public sealed class EnemyData : ScriptableObject
    {
        #region Encapsulation
        public int MaxHealth { get => enemyHealth; }
        public int Damage { get => enemyDamage; }
        #endregion

        [Header("Settings")]
        [SerializeField] private int enemyHealth = 100;
        [SerializeField] private int enemyDamage = 33;
    }
}
