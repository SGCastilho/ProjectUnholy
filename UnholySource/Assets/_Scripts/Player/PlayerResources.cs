using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerResources : MonoBehaviour
    {
        #region Constants
        private const int HEALING_BOTTLES_LIMITATION = 99;
        #endregion

        #region Events
        public delegate void RefreshWeaponUI(ref int bullets, ref int munition);
        public event RefreshWeaponUI OnRefreshWeaponUI;

        public delegate void RefreshBottlesUI(ref int bottlesAmount);
        public event RefreshBottlesUI OnRefreshBottlesUI;
        #endregion

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

            OnRefreshWeaponUI?.Invoke(ref playerBullets, ref playerMunition);
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

            OnRefreshWeaponUI?.Invoke(ref playerBullets, ref playerMunition);
        }

        public void ModifyHealingBottles(bool increase, int amount)
        {
            if(increase)
            {
                playerHealthBottles += amount;

                if(playerHealthBottles > HEALING_BOTTLES_LIMITATION)
                {
                    playerHealthBottles = HEALING_BOTTLES_LIMITATION;
                }
            }
            else
            {
                playerHealthBottles -= amount;

                if(playerHealthBottles < 0)
                {
                    playerHealthBottles = 0;
                }

                OnRefreshBottlesUI?.Invoke(ref playerHealthBottles);
            }
        }

        public void AddHealingBottles() => ModifyHealingBottles(true, 1);
        
        public void AddMunition(int amount) => ModifyMunition(true, amount);

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

        public int CurrentHealingBottles()
        {
            return playerHealthBottles;
        }

        public int CurrentBullets()
        {
            return playerBullets;
        }

        public int CurrentAmmo()
        {
            return playerMunition;
        }
    }
}
