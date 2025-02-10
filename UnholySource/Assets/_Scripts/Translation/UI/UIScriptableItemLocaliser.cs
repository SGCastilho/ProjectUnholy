using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Translation
{
    public sealed class UIScriptableItemLocaliser : UITextLocaliser
    {
        [Header("Exclusive Data")]
        [SerializeField] private ItemData textData;

        public override void ReloadTranslation()
        {
            _tmpPro.text = textData.Name;
            _tmpPro.text = _tmpPro.text.Replace("\r", "");
        }
    }
}
