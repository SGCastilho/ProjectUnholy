using Core.ScriptableObjects;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIInventoryAlert : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private GameObject alertPrefab;

        [Space(10)]

        [SerializeField] private Transform alertTransform;

        [Header("Settings")]
        [SerializeField] private int maxAlertsInScreen = 3;

        private GameObject[] _alertsInstances;

        private int _currentAlertsInScreen;

        private void Awake() 
        {
            _alertsInstances = new GameObject[maxAlertsInScreen];
        }

        public void CallAlert(ItemData itemData)
        {
            if(itemData == null || _currentAlertsInScreen >= maxAlertsInScreen) return;

            GameObject instance = Instantiate(alertPrefab, alertTransform);

            UIAlertSlot alertInstance = instance.GetComponent<UIAlertSlot>();

            alertInstance.OnNotificationEnd += EndAlert;

            alertInstance.RefreshSlot(itemData.Icon, itemData.Name);
            alertInstance.ShowNotification();

            _currentAlertsInScreen++;

            _alertsInstances[_currentAlertsInScreen-1] = instance;
        }

        private void EndAlert(GameObject alert)
        {
            for(int i = 0; i<_alertsInstances.Length; i++)
            {
                if(_alertsInstances[i] == alert)
                {
                    _alertsInstances[i] = null;

                    Destroy(alert);

                    _currentAlertsInScreen--;
                    if(_currentAlertsInScreen < 0)
                    {
                        _currentAlertsInScreen = 0;
                    }

                    break;
                }
            }
        }
    }
}
