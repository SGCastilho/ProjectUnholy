using Core.Player;
using Core.GameCamera;
using Core.Interfaces;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class PlayerAnimationEvents : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Classes")]
        [SerializeField] private Transform hurtBoxTransform;
        [SerializeField] private Transform shootingRayTransform;

        [Header("Settings")]
        [SerializeField] private Vector3 hurtBoxSize;
        [SerializeField] private LayerMask enemyLayer;

        [Space(10)]

        [SerializeField] private float raycastLegth = 4f;

        private Ray _rayDetection;
        private RaycastHit _raycastHitDetection;

        private Collider[] searchingEnemy;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(10)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color hurtBoxColor = Color.red;
        #endif
        #endregion

        public void BlockActions()
        {
            behaviour.Inputs.BlockActions();
            behaviour.Inputs.BlockMovement();
        }

        public void EnableActions()
        {
            behaviour.Inputs.AllowActions();
            behaviour.Inputs.AllowMovement();
        }

        public void ApplyMeleeDamage()
        {
            DrawHurtBox();
        }

        private void DrawHurtBox()
        {
            searchingEnemy = Physics.OverlapBox(hurtBoxTransform.position, hurtBoxSize, hurtBoxTransform.rotation, enemyLayer);

            if(searchingEnemy == null || searchingEnemy.Length <= 0) return;

            AttackCameraShake();
            
            foreach(Collider enemy in searchingEnemy)
            {
                if(enemy.GetComponent<IDamagable>() == null) return;

                enemy.GetComponent<IDamagable>().ApplyDamage(behaviour.Equipment.WeaponDamage);
            }
        }

        public void ApplyRangedDamage()
        {
            AttackCameraShake();
            DrawShootingRay();
        }

        private void DrawShootingRay()
        {
            if(behaviour.LookingToRight())
            {
                _rayDetection = new Ray(shootingRayTransform.position, Vector3.forward);
                _raycastHitDetection = new RaycastHit();

                if(Physics.Raycast(_rayDetection, out _raycastHitDetection, raycastLegth, enemyLayer))
                {
                    if(_raycastHitDetection.transform.GetComponent<IDamagable>() == null) return;
                    
                    _raycastHitDetection.transform.GetComponent<IDamagable>().ApplyDamage(behaviour.Equipment.WeaponDamage);
                }
            }
            else
            {
                _rayDetection = new Ray(shootingRayTransform.position, Vector3.back);
                _raycastHitDetection = new RaycastHit();

                if(Physics.Raycast(_rayDetection, out _raycastHitDetection, raycastLegth, enemyLayer))
                {
                    if(_raycastHitDetection.transform.GetComponent<IDamagable>() == null) return;

                    _raycastHitDetection.transform.GetComponent<IDamagable>().ApplyDamage(behaviour.Equipment.WeaponDamage);
                }
            }
        }

        private void AttackCameraShake()
        {
            if(CameraShake.Instance == null) return;

            CameraShake.Instance.AttackShake();
        }

        public void WhenReloadingEnds()
        {
            behaviour.Inputs.AllowActions();

            behaviour.Actions.IsReloading = false;
        }

        public void WhenHealingEnds()
        {
            behaviour.Status.ApplyHealthBottleHealing();

            behaviour.Actions.IsHealing = false;
            behaviour.Equipment.EquipHealingBottle(false);

            behaviour.Inputs.AllowActions();
        }

        #region Editor Function
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                if(hurtBoxTransform == null || shootingRayTransform == null) return;

                Gizmos.color = hurtBoxColor;

                Gizmos.DrawWireCube(hurtBoxTransform.position, hurtBoxSize);
                Gizmos.DrawLine(shootingRayTransform.position, shootingRayTransform.position + raycastLegth * Vector3.forward);
            }    
        }
        #endif
        #endregion
    }
}
