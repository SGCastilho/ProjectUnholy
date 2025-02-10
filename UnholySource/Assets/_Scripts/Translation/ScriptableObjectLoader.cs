using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Translation
{
    public sealed class ScriptableObjectLoader : MonoBehaviour
    {
        private enum ChapterTranslation { CHAPTER_1, CHAPTER_1_2, CHAPTER_2, CHAPTER_3 }

        [Header("Settings")]
        [SerializeField] private ChapterTranslation currentChapter;

        [Header("Data")]
        [SerializeField] private ItemData[] abstractItemsToTranslate;

        [Space(10)]

        [SerializeField] private ItemData[] keyItemsToTranslate;
        [SerializeField] private ItemData[] puzzleItemsToTranslate;

        private void Awake() 
        {
            switch(currentChapter)
            {
                case ChapterTranslation.CHAPTER_1:
                    TranslateChapterOne();
                    break;
                case ChapterTranslation.CHAPTER_1_2:
                    TranslateChapterOnePartTwo();
                    break;
                case ChapterTranslation.CHAPTER_2:
                    break;
                case ChapterTranslation.CHAPTER_3:  
                    break;
            }
        }

        private void TranslateChapterOne()
        {
            TranslateGlobal();
            TranslateAbstract();

            keyItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Chapter 1/Keys");
            puzzleItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Chapter 1/Puzzles");

            foreach (ItemData keyItem in keyItemsToTranslate)
            {
                keyItem.Name = LocalisationSystem.GetLocalisedValue(keyItem.NameKey);

                if(keyItem.DescriptionKey != null && keyItem.DescriptionKey != string.Empty)
                {
                    keyItem.Description = LocalisationSystem.GetLocalisedValue(keyItem.DescriptionKey);
                }
            }

            foreach (ItemData puzzleItem in puzzleItemsToTranslate)
            {
                puzzleItem.Name = LocalisationSystem.GetLocalisedValue(puzzleItem.NameKey);
                puzzleItem.Description = LocalisationSystem.GetLocalisedValue(puzzleItem.DescriptionKey);
            }

            var textDataToTranslate = Resources.LoadAll<TextData>("ScriptableObjects/ScriptableText/Chapter 1");

            foreach(TextData textData in textDataToTranslate)
            {
                textData.Value = LocalisationSystem.GetLocalisedValue(textData.Key);
            }
        }

        private void TranslateChapterOnePartTwo()
        {
            TranslateGlobal();
            TranslateAbstract();

            keyItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Chapter 1 P2/Keys");
            puzzleItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Chapter 1 P2/Puzzles");

            foreach (ItemData keyItem in keyItemsToTranslate)
            {
                keyItem.Name = LocalisationSystem.GetLocalisedValue(keyItem.NameKey);
                keyItem.Description = LocalisationSystem.GetLocalisedValue(keyItem.DescriptionKey);
            }

            foreach (ItemData puzzleItem in puzzleItemsToTranslate)
            {
                puzzleItem.Name = LocalisationSystem.GetLocalisedValue(puzzleItem.NameKey);
                puzzleItem.Description = LocalisationSystem.GetLocalisedValue(puzzleItem.DescriptionKey);
            }

            var textDataToTranslate = Resources.LoadAll<TextData>("ScriptableObjects/ScriptableText/Chapter 1 P2");

            foreach(TextData textData in textDataToTranslate)
            {
                textData.Value = LocalisationSystem.GetLocalisedValue(textData.Key);
            }
        }

        private void TranslateAbstract()
        {
            abstractItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Abstract");

            foreach (ItemData abstractItem in abstractItemsToTranslate)
            {
                abstractItem.Name = LocalisationSystem.GetLocalisedValue(abstractItem.NameKey);
                abstractItem.Description = LocalisationSystem.GetLocalisedValue(abstractItem.DescriptionKey);
            }
        }

        private void TranslateGlobal()
        {
            var globalTextToTranslate = Resources.LoadAll<TextData>("ScriptableObjects/ScriptableText/Global");

            foreach(TextData textData in globalTextToTranslate)
            {
                textData.Value = LocalisationSystem.GetLocalisedValue(textData.Key);
            }
        }
    }
}
