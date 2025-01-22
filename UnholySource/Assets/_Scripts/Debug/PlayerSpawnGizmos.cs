using UnityEngine;

namespace Core.Debug
{
    public sealed class PlayerSpawnGizmos : MonoBehaviour
    {
        #if UNITY_EDITOR
        [Header("Settings")]
        [SerializeField] private Color gizmosColor = Color.red;

        [Space(10)]

        [SerializeField] private float playerHeight = 2.56f;

        private Vector3 _gizmosSize;
        private Vector3 _gizmosPosition;

        private void OnDrawGizmosSelected() 
        {
            _gizmosSize = new Vector3(1f, playerHeight, 1f);
            _gizmosPosition = new Vector3(transform.position.x, 1.281f, transform.position.z);

            Gizmos.color = gizmosColor;
            Gizmos.DrawWireCube(_gizmosPosition, _gizmosSize);
        }
        #endif
    }
}
