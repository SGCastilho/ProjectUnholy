using Core.Interfaces;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerStatus : MonoBehaviour, IDamagable
    {
        #region Events
        public delegate void TakingDamage();
        public event TakingDamage OnTakingDamage;

        public delegate void Death();
        public event Death OnDeath;

        public delegate void ModifingHealth(ref int currentHealth);
        public event ModifingHealth OnModifingHealth;
        #endregion

        #region Encapsulation
        public int Health { get => playerHealth; }
        public int MaxHealth { get => playerMaxHealth; }

        public bool IsDead { get => isDead; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Data")]
        [SerializeField] private ItemData healingData;

        [Header("Settings")]
        [SerializeField] private int playerHealth = 100;
        [SerializeField] private int playerMaxHealth = 100;

        [Space(10)]

        [SerializeField] private bool isDead;

        public void ModifyHealth(bool increase, int amount)
        {
            if(increase)
            {
                playerHealth += amount;
                if(playerHealth > playerMaxHealth)
                {
                    playerHealth = playerMaxHealth;
                }

                OnModifingHealth?.Invoke(ref playerHealth);
            }
            else
            {
                playerHealth -= amount;
                if(playerHealth <= 0)
                {
                    playerHealth = 0;
                    
                    DeathSequence();
                }

                OnModifingHealth?.Invoke(ref playerHealth);
            }
        }

        public void ApplyHealthBottleHealing()
        {
            ModifyHealth(true, healingData.HealingAmount);
        }

        private void DeathSequence()
        {
            //TEMPORARIO!!! JOGAR TUDO ISSO PARA O GAME CONTROLLER
            isDead = true;

            behaviour.Inputs.BlockControls();
            behaviour.Animation.CallDeathTrigger();

            OnDeath?.Invoke();
            //TEMPORARIO!!! JOGAR TUDO ISSO PARA O GAME CONTROLLER
        }

        public void ApplyDamage(int damageToApply)
        {
            ModifyHealth(false, damageToApply);

            OnTakingDamage?.Invoke();
        }
    }
}
