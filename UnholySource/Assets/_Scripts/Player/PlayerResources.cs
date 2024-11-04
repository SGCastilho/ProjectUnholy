using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerResources : MonoBehaviour
    {
        #region Encapsulation
        public int HealthBottles { get => playerHealthBottles; }

        public int Bullets { get => playerBullets; }
        public int MaxBullets { get => playerMaxBullets; }
        public int Munition { get => playerMunition; }
        public int MaxMunition { get => playerMaxMunition; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private int playerHealthBottles;

        [Space(10)]

        [SerializeField] private int playerBullets;
        [SerializeField] private int playerMunition;

        private int playerMaxBullets;
        private int playerMaxMunition;

        private void OnEnable() 
        {
            playerMaxBullets = behaviour.Equipment.RangedData.Bullets;
            playerMaxMunition = behaviour.Equipment.RangedData.Munition;
        }

        public void ModifyBullets(bool increase, int amount)
        {
            if(increase)
            {
                playerBullets += amount;

                if(playerBullets > playerMaxBullets)
                {
                    playerBullets = playerMaxBullets;
                }
            }
            else
            {
                playerBullets -= amount;

                if(playerBullets < 0)
                {
                    playerBullets = 0;
                }
            }
        }

        public void ModifyMunition(bool increase, int amount)
        {
            if(increase)
            {
                playerMunition += amount;

                if(playerMunition > playerMaxMunition)
                {
                    playerBullets = playerMaxMunition;
                }
            }
            else
            {
                playerMunition -= amount;

                if(playerMunition < 0)
                {
                    playerMunition = 0;
                }
            }
        }

        internal bool CanReload()
        {
            int missingBullets = playerMaxBullets - playerBullets;

            if(missingBullets <= 0 || playerMunition <= 0)
            {
                return false;
            }
            
            return true;
        }

        internal void ReloadingBullets()
        {
            int missingBullets = playerMaxBullets - playerBullets;

            if(playerMunition <= missingBullets)
            {
                ModifyBullets(true, playerMunition);
                ModifyMunition(false, playerMunition);
            }
            else
            {
                ModifyBullets(true, missingBullets);
                ModifyMunition(false, missingBullets);
            }
        }
    }
}
