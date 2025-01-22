using UnityEngine;

namespace Core.Utilities
{
    public sealed class DestroyObject : MonoBehaviour
    {
        public void DestroyObj()
        {
            Destroy(gameObject);
        }
    }
}
