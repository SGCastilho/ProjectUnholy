using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerEquipment : MonoBehaviour
    {
        #region Encapsulation
        public bool MeleeEnabled { set => meleeEnabled = value; }
        public bool RangedEnabled { set => rangedEnabled = value; }

        public float CurrentBullets 
        { 
            get => rangedCurrentBullets; 
            set 
            {
                rangedCurrentBullets += value;

                if(rangedCurrentBullets > rangedWeapon.Bullets) { rangedCurrentBullets = rangedWeapon.Bullets; }
            }
        }
        public float CurrentAmmo
        {
            get => rangedCurrentAmmo;
            set
            {
                rangedCurrentAmmo += value;

                if(rangedCurrentAmmo > rangedWeapon.MaxBullets) { rangedCurrentAmmo = rangedWeapon.MaxBullets; }
            }
        }

        internal bool MeleeEquipped { get => meleeEquipped; }

        internal float WeaponDamage { get => _currentWeaponDamage; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Data")]
        [SerializeField] private WeaponData meleeWeapon;
        [SerializeField] private WeaponData rangedWeapon;

        [Header("Settings")]
        [SerializeField] private bool meleeEnabled;
        [SerializeField] private bool rangedEnabled;

        [Space(10)]

        [SerializeField] private bool meleeEquipped;
        [SerializeField] private bool rangedEquipped;

        [Space(10)]

        [SerializeField] private float rangedCurrentBullets;
        [SerializeField] private float rangedCurrentAmmo;

        [Space(10)]

        [SerializeField] private GameObject meleeWeaponModel;
        [SerializeField] private GameObject rangedWeaponModel;

        private int _currentWeaponDamage;

        private void OnEnable() 
        {
            meleeWeaponModel.SetActive(false);
            rangedWeaponModel.SetActive(false);

            if(meleeWeapon && rangedWeapon == null) return;

            _currentWeaponDamage = meleeWeapon.Damage;
        }

        internal void EquipMeleeWeapon()
        {
            if(!meleeEnabled) return;

            meleeEquipped = true;
            rangedEquipped = false;

            meleeWeaponModel.SetActive(meleeEquipped);
            rangedWeaponModel.SetActive(rangedEquipped);

            behaviour.Animation.MeleeEquippedAnimation = meleeEquipped;
        }

        internal void EquipRangedWeapon()
        {
            if(!rangedEnabled) return;

            meleeEquipped = false;
            rangedEquipped = true;

            meleeWeaponModel.SetActive(meleeEquipped);
            rangedWeaponModel.SetActive(rangedEquipped);

            behaviour.Animation.MeleeEquippedAnimation = meleeEquipped;
        }
    }
}
