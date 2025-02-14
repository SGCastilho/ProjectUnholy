using UnityEngine;

namespace Core.Utilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveObjectHorizontal : MonoBehaviour
    {
        #region Encapsulaton
        public bool MoveRight { set => moveRight = value; }
        #endregion

        [Header("Classes")]
        [SerializeField] private Rigidbody rb3D;

        [Header("Settings")]
        [SerializeField] private float speed = 4f;
        [SerializeField] private bool moveRight;

        private void Update()
        {
            if(moveRight)
            {
                rb3D.velocity = speed * Vector3.forward;
            }
            else
            {
                rb3D.velocity = speed * Vector3.back;
            }
        }
    }
}
