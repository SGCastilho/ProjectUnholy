using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerStatus : MonoBehaviour
    {
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
            }
            else
            {
                playerHealth -= amount;
                if(playerHealth <= 0)
                {
                    playerHealth = 0;
                    
                    DeathSequence();
                }
            }
        }

        public void ApplyHealthBottleHealing()
        {
            ModifyHealth(true, healingData.HealingAmount);
        }

        private void DeathSequence()
        {
            isDead = true;

            behaviour.Inputs.BlockControls();
            behaviour.Animation.CallDeathTrigger();
        }

        //DEBUG
        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                ModifyHealth(false, 999);
            }
        }
    }
}
