using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Translation
{
    public sealed class ScriptableObjectLoader : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ItemData[] keyItemsToTranslate;
        [SerializeField] private ItemData[] puzzleItemsToTranslate;
        [SerializeField] private ItemData[] abstractItemsToTranslate;

        private void Awake() 
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Translate()
        {
            keyItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Keys");
            puzzleItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Puzzles");
            abstractItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Abstract");

            foreach(ItemData keyItem in keyItemsToTranslate)
            {
                keyItem.Name = LocalisationSystem.GetLocalisedValue(keyItem.NameKey);
                keyItem.Description = LocalisationSystem.GetLocalisedValue(keyItem.DescriptionKey);
            }

            foreach(ItemData puzzleItem in puzzleItemsToTranslate)
            {
                puzzleItem.Name = LocalisationSystem.GetLocalisedValue(puzzleItem.NameKey);
                puzzleItem.Description = LocalisationSystem.GetLocalisedValue(puzzleItem.DescriptionKey);
            }

            foreach(ItemData abstractItem in abstractItemsToTranslate)
            {
                abstractItem.Name = LocalisationSystem.GetLocalisedValue(abstractItem.NameKey);
                abstractItem.Description = LocalisationSystem.GetLocalisedValue(abstractItem.DescriptionKey);
            }
        }
    }
}
