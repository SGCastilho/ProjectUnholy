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

        #region Constants
        private const string SFX_HITTED_0 = "audio_hitted_0";
        private const string SFX_HITTED_1 = "audio_hitted_1";

        private const string SFX_HEALING_0 = "audio_healing_0";
        private const string SFX_HEALING_1 = "audio_healing_1";
        private const string SFX_HEALING_2 = "audio_healing_2";
        #endregion

        #region Encapsulation
        public int Health { get => playerHealth; }
        public int MaxHealth { get => playerMaxHealth; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Data")]
        [SerializeField] private ItemData healingData;

        [Header("Settings")]
        [SerializeField] private int playerHealth = 100;
        [SerializeField] private int playerMaxHealth = 100;

        [Space(10)]

        [SerializeField] private int maxHittedSFX = 1;
        [SerializeField] private int maxHealingSFX = 2;

        private float _currentHittedSFX = 0;
        private float _currentHealingSFX = 0;

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
            HealingSFX();
            ModifyHealth(true, healingData.HealingAmount);
        }

        private void DeathSequence()
        {
            behaviour.Inputs.BlockControls();
            behaviour.Animation.CallDeathTrigger();

            OnDeath?.Invoke();
        }

        public void ApplyDamage(int damageToApply)
        {
            ModifyHealth(false, damageToApply);

            HittedSFX();

            OnTakingDamage?.Invoke();
        }

        private void HittedSFX()
        {
            if(_currentHittedSFX > maxHittedSFX) { _currentHittedSFX = 0; }

            switch(_currentHittedSFX)
            {
                case 0:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_HITTED_0);
                    break;
                case 1:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_HITTED_1);
                    break;
            }

            _currentHittedSFX++;
        }

        private void HealingSFX()
        {
            if(_currentHealingSFX > maxHealingSFX) { _currentHealingSFX = 0; }

            switch(_currentHealingSFX)
            {
                case 0:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_HEALING_0);
                    break;
                case 1:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_HEALING_1);
                    break;
                case 2:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_HEALING_2);
                    break;
            }

            _currentHealingSFX++;
        }

        public int GetCurrentHealth()
        {
            return playerHealth;
        }

        public void LoadedPlayerHealth(int savedHealth)
        {
            playerHealth = savedHealth;
            OnModifingHealth?.Invoke(ref playerHealth);
        }
    }
}
