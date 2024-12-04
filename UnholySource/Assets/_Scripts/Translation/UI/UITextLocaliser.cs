using TMPro;
using UnityEngine;

namespace Core.Translation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class UITextLocaliser : MonoBehaviour
    {
        [Header("Data")]
        public LocalisedString localisedString;

        private TextMeshProUGUI _tmpPro;

        private void Awake() 
        {
            _tmpPro = GetComponent<TextMeshProUGUI>();

            ReloadTranslation();
        }

        public void ReloadTranslation()
        {
            _tmpPro.text = localisedString.value;
        }
    }
}
