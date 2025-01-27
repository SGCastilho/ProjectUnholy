using UnityEngine;

namespace Core.Utilities
{
    public sealed class TurnOffRealtimeLights : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Light[] lightsToDisable;
        [SerializeField] private GameObject[] lightOnGameObject;
        [SerializeField] private GameObject[] lightOffGameObject;

        [Header("Settings")]
        [SerializeField] private bool changeGraphics;

        public void TurnLightsOff()
        {
            for(int i = 0; i < lightsToDisable.Length; i++)
            {
                lightsToDisable[i].gameObject.SetActive(false);

                if(changeGraphics)
                {
                    lightOnGameObject[i].gameObject.SetActive(false);
                    lightOffGameObject[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
