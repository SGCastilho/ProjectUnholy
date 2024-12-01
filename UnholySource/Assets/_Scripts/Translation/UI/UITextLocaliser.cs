using TMPro;
using UnityEngine;

namespace Core.Translation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class UITextLocaliser : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private string localisationKey;

        private TextMeshProUGUI _tmpPro;

        private void Awake() 
        {
            _tmpPro = GetComponent<TextMeshProUGUI>();
        }

        private void Start() 
        {
            string value = LocalisationSystem.GetLocalisedValue(localisationKey);

            _tmpPro.text = value;
        }
    }
}
