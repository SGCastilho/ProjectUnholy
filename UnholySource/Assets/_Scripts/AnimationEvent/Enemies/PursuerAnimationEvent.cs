using Core.Enemies;
using Core.Interfaces;
using Core.Player;
using UnityEngine;

namespace Core.AnimationEvents
{
    public class PursuerAnimationEvent : MonoBehaviour
    {
        #region Constants
        private const string SFX_FOOTSTEP_0 = "audio_footstep_0";
        private const string SFX_FOOTSTEP_1 = "audio_footstep_1";
        private const string SFX_FOOTSTEP_2 = "audio_footstep_2";
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PursuerBehaviour behaviour;

        [Space(10)]

        [SerializeField] private Transform hurtBoxTransform;

        [Header("Settings")]
        [SerializeField] private Vector3 searchingBoxSize;
        [SerializeField] private LayerMask searchingLayer;

        [Space(10)]

        [SerializeField] private int maxFootstepSFX = 2;

        private Transform _playerTransform;
        private Collider[] _searchingPlayer;

        private bool _damageApplied;

        private int _currentFootstepSFX;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(10)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color searchingBoxColor = Color.red;
        #endif
        #endregion

        private void Awake() 
        {
            _playerTransform = FindObjectOfType<PlayerBehaviour>().GetComponent<Transform>();
        }

        private void Start() 
        {
            _currentFootstepSFX = 0;
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
        
        public void FinishAttack()
        {
            behaviour.Animation.IsAttacking = false;

            _damageApplied = false;
        }

        public void FinishRunningAttack()
        {
            behaviour.Animation.IsRunningAttacking = false;
            behaviour.Animation.IsRunning = false;

            _damageApplied = false;
        }

        public void GetCloseToPlayer()
        {
            behaviour.Movement.DashTo(_playerTransform, behaviour.GetPlayerSide(), 20f);
        }

        public void DrawHurtBox()
        {
            if(_damageApplied) return;

            _searchingPlayer = Physics.OverlapBox(hurtBoxTransform.position, searchingBoxSize, hurtBoxTransform.rotation, searchingLayer);

            if(_searchingPlayer == null || _searchingPlayer.Length <= 0) return;
            
            foreach(Collider player in _searchingPlayer)
            {
                if(player.CompareTag("Player"))
                {
                    player.GetComponent<IDamagable>().ApplyDamage(behaviour.Damage);

                    _damageApplied = true;
                    
                    break;
                }
            }
        }

        #region Editor Function
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                if(hurtBoxTransform == null) return;

                Gizmos.color = searchingBoxColor;
                Gizmos.DrawWireCube(hurtBoxTransform.position, searchingBoxSize);
            }    
        }
        #endif
        #endregion
    }
}
