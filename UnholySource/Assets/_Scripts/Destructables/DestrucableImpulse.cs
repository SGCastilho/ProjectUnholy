using System.Collections;
using UnityEngine;

namespace Core.Destructables
{
    public sealed class DestrucableImpulse : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private GameObject noPhysicsDestructable;
        [SerializeField] private GameObject physicsDestrucable;

        [Space(10)]

        [SerializeField] private GameObject[] physicsPieces;

        [Header("Settings")]
        [SerializeField] private float impulseForce;
        [SerializeField] private Vector3 impulseDirection;

        [Space(10)]

        [SerializeField] [Range(2f, 6f)] private float timeToDisableColliders = 4f;

        private Rigidbody[] piecesRigidBody;
        private Collider[] piecesColliders;

        private void Awake() 
        {
            piecesRigidBody = new Rigidbody[physicsPieces.Length];
            piecesColliders = new Collider[physicsPieces.Length];

            for(int i = 0; i < physicsPieces.Length; i++)
            {
                piecesRigidBody[i] = physicsPieces[i].GetComponent<Rigidbody>();
                piecesColliders[i] = physicsPieces[i].GetComponent<Collider>();
            }
        }

        public void ImpulseDestructables()
        {
            noPhysicsDestructable.SetActive(false);
            physicsDestrucable.SetActive(true);

            for(int i = 0; i < physicsPieces.Length; i++)
            {
                piecesRigidBody[i].AddForce(impulseDirection * impulseForce, ForceMode.Impulse);
            }

            StartCoroutine(DisableColliderCouroutine());
        }

        public void GoToDestroyedMode()
        {
            noPhysicsDestructable.SetActive(false);
            physicsDestrucable.SetActive(true);

            for(int i = 0; i < physicsPieces.Length; i++)
            {
                physicsPieces[i].SetActive(false);
            }
        }

        private IEnumerator DisableColliderCouroutine()
        {
            yield return new WaitForSeconds(timeToDisableColliders);

            for(int i = 0; i < physicsPieces.Length; i++)
            {
                piecesRigidBody[i].isKinematic = true;
                yield return null;
            }

            for(int i = 0; i < physicsPieces.Length; i++)
            {
                piecesColliders[i].enabled = false;
                yield return null;
            }
        }
    }
}
