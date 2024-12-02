using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Translation
{
    public sealed class ScriptableObjectLoader
    {
        private ItemData[] keyItemsToTranslate;
        private ItemData[] abstractItemsToTranslate;

        public void Translate()
        {
            keyItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Keys");
            abstractItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Abstract");

            foreach(ItemData keyItem in keyItemsToTranslate)
            {
                keyItem.Name = LocalisationSystem.GetLocalisedValue(keyItem.NameKey);
                keyItem.Description = LocalisationSystem.GetLocalisedValue(keyItem.DescriptionKey);
            }

            foreach(ItemData abstractItem in abstractItemsToTranslate)
            {
                abstractItem.Name = LocalisationSystem.GetLocalisedValue(abstractItem.NameKey);
                abstractItem.Description = LocalisationSystem.GetLocalisedValue(abstractItem.DescriptionKey);
            }
        }
    }
}
