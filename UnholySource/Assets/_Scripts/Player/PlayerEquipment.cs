using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerEquipment : MonoBehaviour
    {
        #region Encapsulation
        internal WeaponData MeleeData { get => meleeData; }
        internal WeaponData RangedData { get => rangedData; }

        public bool MeleeEnabled { get => meleeEnabled; set => meleeEnabled = value; }
        public bool RangedEnabled { get => rangedEnabled; set => rangedEnabled = value; }

        internal bool MeleeEquipped { get => meleeEquipped; }
        internal bool RangedEquipped { get => rangedEquipped; }

        public int WeaponDamage { get => _currentWeaponDamage; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Data")]
        [SerializeField] private WeaponData meleeData;
        [SerializeField] private WeaponData rangedData;

        [Header("Settings")]
        [SerializeField] private bool meleeEnabled;
        [SerializeField] private bool rangedEnabled;

        [Space(10)]

        [SerializeField] private bool meleeEquipped;
        [SerializeField] private bool rangedEquipped;

        [Space(10)]

        [SerializeField] private GameObject meleeWeaponModel;
        [SerializeField] private GameObject rangedWeaponModel;

        [Space(6)]

        [SerializeField] private GameObject healingBottleModel;

        private int _currentWeaponDamage;

        private void OnEnable() 
        {
            meleeWeaponModel.SetActive(false);
            rangedWeaponModel.SetActive(false);

            healingBottleModel.SetActive(false);

            if(meleeData && rangedData == null) return;

            _currentWeaponDamage = meleeData.Damage;
        }

        private void Start() => EquipMeleeWeapon(); //TEMPORARIO

        internal void EquipMeleeWeapon()
        {
            if(!meleeEnabled) return;
        
            meleeEquipped = true;
            rangedEquipped = false;

            meleeWeaponModel.SetActive(meleeEquipped);
            rangedWeaponModel.SetActive(rangedEquipped);

            _currentWeaponDamage = meleeData.Damage;

            behaviour.Animation.MeleeEquippedAnimation = meleeEquipped;
            behaviour.Animation.RangedEquippedAnimation = rangedEquipped;
        }

        internal void EquipRangedWeapon()
        {
            if(!rangedEnabled) return;

            meleeEquipped = false;
            rangedEquipped = true;

            meleeWeaponModel.SetActive(meleeEquipped);
            rangedWeaponModel.SetActive(rangedEquipped);

            _currentWeaponDamage = rangedData.Damage;

            behaviour.Animation.MeleeEquippedAnimation = meleeEquipped;
            behaviour.Animation.RangedEquippedAnimation = rangedEquipped;
        }

        public void EquipHealingBottle(bool equip) => healingBottleModel.SetActive(equip);
    }
}
