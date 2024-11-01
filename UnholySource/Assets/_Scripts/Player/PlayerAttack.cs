using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAttack : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private bool isAiming;

        [Space(10)]

        [SerializeField] [Range(0.1f, 1.6f)] private float meleeAttackCouldown = 1f;
        [SerializeField] [Range(0.1f, 1.6f)] private float rangedAttackCouldown = 0.4f;

        private bool _isAttacking;

        private float _attackCouldown;
        private float _currentAttackCouldown;

        private void Update() 
        {
            if(_isAttacking)
            {
                _currentAttackCouldown += Time.deltaTime;
                if(_currentAttackCouldown >= _attackCouldown)
                {
                    _currentAttackCouldown = 0f;

                    _isAttacking = false;
                }
            }
        }

        public void StartAttack()
        {
            if(_isAttacking) return;

            if(isAiming)
            {

            }
            else
            {
                if(!behaviour.Equipment.MeleeEquipped)
                {
                    behaviour.Equipment.EquipMeleeWeapon();
                }

                //FAZER O JOGADOR SE REPOSICIONAR NO INIMIGO USANDO RAYCAST

                behaviour.Animation.CallAttackTrigger();

                _attackCouldown = meleeAttackCouldown;

                _isAttacking = true;
            }
        }
    }
}
