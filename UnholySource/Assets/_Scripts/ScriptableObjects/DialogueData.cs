using UnityEngine;

namespace Core.ScriptableObjects
{
    [System.Serializable]
    public struct Dialogue
    {
        #region Encapsulation
        public string Name { get => characterName; }

        public string Key { get => dialogueKey; }
        public string[] Dialogues { get => dialogues; set => dialogues = value; }
        #endregion

        [SerializeField] private string characterName;

        [Space(10)]

        [SerializeField] private string dialogueKey;
        [SerializeField] [Multiline(6)] private string[] dialogues;
    }

    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "Data/New Dialogue Data")]
    public sealed class DialogueData : ScriptableObject
    {
        #region Encapsulation
        public Dialogue[] DialogueTree { get => dialogueTree; }
        #endregion

        [Header("Settings")]
        [SerializeField] private Dialogue[] dialogueTree;
    }
}
