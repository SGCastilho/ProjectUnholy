using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Text Data", menuName = "Data/Translation/New Text Data")]
    public sealed class TextData : ScriptableObject
    {
        #region Encapsulation
        public string Key { get => textKey; }
        public string Value { get => textValue; set => textValue = value; }
        #endregion

        [Header("Settings")]
        [SerializeField] private string textKey = "your_key_here";

        [Space(10)]

        [SerializeField] private string textValue = "Your text here.";
    }
}
