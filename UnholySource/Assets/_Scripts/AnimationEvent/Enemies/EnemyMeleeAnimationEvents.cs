using Core.Enemies;
using Core.Interfaces;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class EnemyMeleeAnimationEvents : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;

        [Space(10)]

        [SerializeField] private Transform hurtBoxTransform;

        [Header("Settings")]
        [SerializeField] private Vector3 searchingBoxSize;
        [SerializeField] private LayerMask searchingLayer;
        private Collider[] searchingPlayer;

        private bool _damageApplied;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(10)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color searchingBoxColor = Color.red;
        #endif
        #endregion

        public void AttackFinish()
        {
            _damageApplied = false;

            behaviour.Animation.IsAttacking = false;
        }

        public void ShowHurtBox()
        {
            if(_damageApplied) return;

            DrawHurtBox();
        }

        private void DrawHurtBox()
        {
            searchingPlayer = Physics.OverlapBox(hurtBoxTransform.position, searchingBoxSize, hurtBoxTransform.rotation, searchingLayer);

            if(searchingPlayer == null || searchingPlayer.Length <= 0) return;
            
            foreach(Collider player in searchingPlayer)
            {
                if(player.CompareTag("Player"))
                {
                    player.GetComponent<IDamagable>().ApplyDamage(behaviour.Status.Damage);

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
