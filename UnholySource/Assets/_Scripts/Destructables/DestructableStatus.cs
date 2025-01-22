using Core.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Destructables
{
    public sealed class DestructableStatus : MonoBehaviour, IDamagable
    {
        [Header("Settings")]
        [SerializeField] private int destructableHealth;

        [Space(10)]

        [SerializeField] private bool destroyed;

        [Header("Unity Event")]
        [Space(10)]
        
        [SerializeField] private UnityEvent OnDestroyed;

        private int _currentHealth;

        private void Start() 
        {
            _currentHealth = destructableHealth;
        }

        public void ApplyDamage(int damageToApply)
        {
            if(destroyed) return;

            _currentHealth -= damageToApply;
            if(_currentHealth <= 0)
            {
                destroyed = true;
                _currentHealth = 0;

                OnDestroyed?.Invoke();
            }
        }
    }
}
