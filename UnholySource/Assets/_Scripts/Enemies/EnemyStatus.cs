using Core.Interfaces;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Enemies
{
    public sealed class EnemyStatus : MonoBehaviour, IDamagable
    {
        #region Encapsulation
        public int Damage { get => currentDamage; }
        public bool IsDead { get => isDead; }
        #endregion

        [Header("Data")]
        [SerializeField] private EnemyData data;

        [Header("Behaviour")]
        [SerializeField] private EnemyBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private int currentHealth;
        [SerializeField] private int currentDamage;

        [Space(10)]

        [SerializeField] private bool isDead;

        [Header("Events")]
        [SerializeField] private UnityEvent OnEnemyTakeDamage;

        [Space(10)]

        [SerializeField] private UnityEvent OnEnemyDeath;

        private void Start()
        {
            currentHealth = data.MaxHealth;
            currentDamage = data.Damage;

            isDead = false;
        }

        private void ModifyHealth(bool increase, int amount)
        {
            if(isDead) return;

            if(increase)
            {
                currentHealth += amount;
                if(currentDamage > data.MaxHealth)
                {
                    currentHealth = data.MaxHealth;
                }
            }
            else
            {
                currentHealth -= amount;
                if(currentHealth <= 0)
                {
                    currentHealth = 0;
                    isDead = true;
                    
                    behaviour.Death();

                    OnEnemyDeath?.Invoke();
                }
                else
                {
                    OnEnemyTakeDamage?.Invoke();
                }
            }
        }

        public void ApplyDamage(int damageToApply)
        {
            ModifyHealth(false, damageToApply);
        }
    }
}
