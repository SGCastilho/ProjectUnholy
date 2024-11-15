using UnityEngine;

namespace Core.ScriptableObjects
{
    public enum WeaponType { MELEE, RANGED }

    [CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/New Weapon Data")]
    public sealed class WeaponData : ScriptableObject
    {
        #region Encapsulation
        public string Name { get => weaponName; internal set => weaponName = value; }
        public WeaponType Type { get => weaponType; internal set => weaponType = value; }
        public string Description { get => weaponDescription; internal set => weaponDescription = value; }

        public int Damage { get => weaponDamage; internal set => weaponDamage = value; }
        public int Bullets { get => weaponBullets; internal set => weaponBullets = value; }
        public int Munition { get => weaponMunition; internal set => weaponMunition = value; }

        public GameObject Prefab { get => weaponPrefab; internal set => weaponPrefab = value; }
        #endregion

        [Header("Info Settings")]
        [SerializeField] private string weaponName = "Weapon Name";
        [SerializeField] private WeaponType weaponType = WeaponType.MELEE;
        [SerializeField] [Multiline(6)] private string weaponDescription = "Weapon Description here.";

        [Header("Data Settings")]
        [SerializeField] private int weaponDamage = 10;
        [SerializeField] private int weaponBullets = 16;
        [SerializeField] private int weaponMunition = 128;
        [Space(12)]
        [SerializeField] private GameObject weaponPrefab;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(40)]
        [SerializeField] internal string devNotes = "Put your dev notes here.";
        #endif
        #endregion
    }
}
