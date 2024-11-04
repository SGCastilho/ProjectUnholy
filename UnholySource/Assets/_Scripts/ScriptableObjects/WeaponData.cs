using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
        [SerializeField] private string weaponDescription = "Weapon Description here.";

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

    #region Custom Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(WeaponData))] //Isso directiona o script para qual lugar onde queremos aplicar o custom editor, no caso o Weapon Data
    class WeaponDataEditor : Editor
    {
        public override void OnInspectorGUI() //Essa função irá sobrescrever a GUI do inspector
        {
            var weaponData = (WeaponData) target; //Ira pegar o script do WeaponData, caso nada for achado o script ira retornar nulo
            if(weaponData == null) return;

            EditorGUILayout.LabelField("Info Settings", EditorStyles.boldLabel);

            EditorGUILayout.Space(6);

            weaponData.Name = EditorGUILayout.TextField("Weapon Name", weaponData.Name);
            weaponData.Type = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponData.Type);

            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Weapon Description");
            weaponData.Description = EditorGUILayout.TextArea(weaponData.Description, GUILayout.MaxHeight(60));

            EditorGUILayout.Space(6);

            EditorGUILayout.LabelField("Data Settings", EditorStyles.boldLabel);

            EditorGUILayout.Space(6);

            weaponData.Damage = EditorGUILayout.IntField("Weapon Damage", weaponData.Damage);

            if(weaponData.Type == WeaponType.RANGED)
            {
                weaponData.Bullets = EditorGUILayout.IntField("Weapon Bullets", weaponData.Bullets);
                weaponData.Munition = EditorGUILayout.IntField("Weapon Bullets", weaponData.Munition);
            }

            EditorGUILayout.Space(6);

            weaponData.Prefab = EditorGUILayout.ObjectField("Weapon Prefab", weaponData.Prefab, typeof(GameObject), false) as GameObject;

            EditorGUILayout.Space(40);

            EditorGUILayout.LabelField("Dev Notes", EditorStyles.boldLabel);
            weaponData.devNotes = EditorGUILayout.TextArea(weaponData.devNotes, GUILayout.MaxHeight(60));
        }
    }
    #endif
    #endregion
}
