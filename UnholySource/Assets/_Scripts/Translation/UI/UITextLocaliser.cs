using TMPro;
using UnityEngine;

namespace Core.Translation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UITextLocaliser : MonoBehaviour
    {
        [Header("Data")]
        public LocalisedString localisedString;

        protected TextMeshProUGUI _tmpPro;

        protected void Awake() 
        {
            _tmpPro = GetComponent<TextMeshProUGUI>();

            ReloadTranslation();
        }

        public virtual void ReloadTranslation()
        {
            _tmpPro.text = localisedString.value;
            _tmpPro.text = _tmpPro.text.Replace("\r", "");
        }
    }
}
