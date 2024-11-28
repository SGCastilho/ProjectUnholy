using Core.Enemies;
using Core.Interfaces;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class PredatorAnimationEvents : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private PredatorBehaviour behaviour;

        [Header("Classes")]
        [SerializeField] private Transform hurtBoxTransform;

        [Header("Settings")]
        [SerializeField] [Range(6f, 10f)] private float dashLenght;

        [Space(10)]

        [SerializeField] private LayerMask hurtLayerMask;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(20)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color hurtBoxColor = Color.red;
        #endif
        #endregion

        private Ray _rayDetection;
        private RaycastHit _raycastHitDetection;

        private Transform _playerTransform;

        private void Awake() 
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        public void DrawHurtBox()
        {
            if(_playerTransform == null) return;

            behaviour.Movement.DashTo(dashLenght, behaviour.Movement.MoveRight, behaviour.Movement.DashSpeed);

            if(behaviour.Movement.MoveRight)
            {
                _rayDetection = new Ray(hurtBoxTransform.position, Vector3.forward);
                _raycastHitDetection = new RaycastHit();

                if(Physics.Raycast(_rayDetection, out _raycastHitDetection, dashLenght, hurtLayerMask))
                {
                    if(_raycastHitDetection.transform.GetComponent<IDamagable>() == null) return;
                    
                    _raycastHitDetection.transform.GetComponent<IDamagable>().ApplyDamage(behaviour.Damage);
                }
            }
            else
            {
                _rayDetection = new Ray(hurtBoxTransform.position, Vector3.back);
                _raycastHitDetection = new RaycastHit();

                if(Physics.Raycast(_rayDetection, out _raycastHitDetection, dashLenght, hurtLayerMask))
                {
                    if(_raycastHitDetection.transform.GetComponent<IDamagable>() == null) return;

                    _raycastHitDetection.transform.GetComponent<IDamagable>().ApplyDamage(behaviour.Damage);
                }
            }
        }

        #region Editor Function
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                Gizmos.color = hurtBoxColor;
                Gizmos.DrawRay(hurtBoxTransform.position, Vector3.forward * dashLenght);
            }    
        }
        #endif
        #endregion
    }
}
