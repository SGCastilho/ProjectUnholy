using Core.Player;
using Core.GameCamera;
using Core.Interfaces;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class PlayerAnimationEvents : MonoBehaviour
    {
        #region Constants
        private const string SFX_PISTOL_SHOOT = "audio_pistol_shoot";
        private const string SFX_PISTOL_MAG_OUT = "audio_pistol_reload_magOut";
        private const string SFX_PISTOL_MAG_IN = "audio_pistol_reload_magIn";

        private const string SFX_WOOD_IMPACT_0 = "audio_wood_hit_0";
        private const string SFX_WOOD_IMPACT_1 = "audio_wood_hit_1";
        private const string SFX_WOOD_IMPACT_2 = "audio_wood_hit_2";

        private const string SFX_FOOTSTEP_0 = "audio_footstep_0";
        private const string SFX_FOOTSTEP_1 = "audio_footstep_1";
        private const string SFX_FOOTSTEP_2 = "audio_footstep_2";
        #endregion

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

        [Space(20)]

        [SerializeField] private int maxFootstepSFX = 2;
        [SerializeField] private int maxWoodImpactSFX = 2;

        private Ray _rayDetection;
        private RaycastHit _raycastHitDetection;
        private Collider[] _searchingEnemy;

        private int _currentFootstepSFX;
        private int _currentWoodImpactSFX;

        private void Start() 
        {
            _currentFootstepSFX = 0;
        }

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
            _searchingEnemy = Physics.OverlapBox(hurtBoxTransform.position, hurtBoxSize, hurtBoxTransform.rotation, enemyLayer);

            if(_searchingEnemy == null || _searchingEnemy.Length <= 0) return;

            AttackCameraShake();
            
            foreach(Collider enemy in _searchingEnemy)
            {
                if(enemy.GetComponent<IDamagable>() == null) return;

                enemy.GetComponent<IDamagable>().ApplyDamage(behaviour.Equipment.WeaponDamage);
            }

            WoodImpactSFX();
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

            ShootSFX();
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

        public void WalkingSFX()
        {
            if(_currentFootstepSFX > maxFootstepSFX) { _currentFootstepSFX = 0; }

            switch(_currentFootstepSFX)
            {
                case 0:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_FOOTSTEP_0);
                    break;
                case 1:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_FOOTSTEP_1);
                    break;
                case 2:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_FOOTSTEP_2);
                    break;
            }

            _currentFootstepSFX++;
        }

        private void ShootSFX()
        {
            behaviour.SFXManager.PlayAudioOneShoot(SFX_PISTOL_SHOOT);
        }

        public void ReloadMagOutSFX()
        {
            behaviour.SFXManager.PlayAudioOneShoot(SFX_PISTOL_MAG_OUT);
        }

        public void ReloadMagIntSFX()
        {
            behaviour.SFXManager.PlayAudioOneShoot(SFX_PISTOL_MAG_IN);
        }

        private void WoodImpactSFX()
        {
            if(_currentWoodImpactSFX > maxWoodImpactSFX) { _currentWoodImpactSFX = 0; }

            switch(_currentWoodImpactSFX)
            {
                case 0:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_WOOD_IMPACT_0);
                    break;
                case 1:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_WOOD_IMPACT_1);
                    break;
                case 2:
                    behaviour.SFXManager.PlayAudioOneShoot(SFX_WOOD_IMPACT_2);
                    break;
            }

            _currentWoodImpactSFX++;
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
