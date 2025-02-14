using Core.StateMachines;
using Core.Utilities;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedShootState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Classes")]
        [SerializeField] private EnemyRangedBehaviour enemyBehaviour;

        [Header("Settings")]
        [SerializeField] private bool isShooting;//Precisa resetar

        [Space(10)]

        [SerializeField] private int maxProjectiles = 4;
        [SerializeField] private int currentShottedProjectiles;

        [Space(10)]

        [SerializeField] private bool inCouldown = true;

        [Space(10)]

        [SerializeField] [Range(0.1f , 2f)] private float shootingCouldown = 1f;

        [Space(10)]

        [SerializeField] private Transform shootingPoint;

        [Space(5)]

        [SerializeField] private MoveObjectHorizontal[] projectiles;

        private int _maxInstancesProjectiles;
        private int _currentInstancesProjectile;
        private float _currentShootingCouldown;

        public override void StateAction()
        {
            if(currentShottedProjectiles > maxProjectiles)
            {
                GoToNextState();
            }

            if(inCouldown)
            {
                _currentShootingCouldown += Time.deltaTime;
                if(_currentShootingCouldown >= shootingCouldown)
                {
                    inCouldown = false;

                    _currentShootingCouldown = 0f;
                }
            }

            if(!isShooting && !inCouldown)
            {
                isShooting = true;

                enemyBehaviour.Movement.FlipToPlayer(enemyBehaviour.GetPlayerSide());
                enemyBehaviour.Animation.IsShooting = isShooting;
            }
        }

        void Start()
        {
            _maxInstancesProjectiles = projectiles.Length;
        }

        void OnDisable() => ResetState();

        public override void ResetState()
        {
            isShooting = false;
            inCouldown = true;
            _currentInstancesProjectile = 0;
            currentShottedProjectiles = 0;
            _currentShootingCouldown = 0f;
        }

        private void GoToNextState()
        {
            isShooting = false;
            inCouldown = true;
            _currentInstancesProjectile = 0;
            currentShottedProjectiles = 0;
            _currentShootingCouldown = 0f;

            stateMachine.ChangeState(nextState);
        }

        public void InstantiateProjectile()
        {
            projectiles[_currentInstancesProjectile].gameObject.SetActive(false);

            projectiles[_currentInstancesProjectile].MoveRight = enemyBehaviour.Movement.MoveRight;
            projectiles[_currentInstancesProjectile].transform.position = shootingPoint.position;

            projectiles[_currentInstancesProjectile].gameObject.SetActive(true);

            _currentInstancesProjectile++;

            if(_currentInstancesProjectile >= _maxInstancesProjectiles)
            {
                _currentInstancesProjectile = 0;
            }
        }

        public void ShootingEnded() 
        {
            currentShottedProjectiles++;

            isShooting = false;
            inCouldown = true;

            enemyBehaviour.Animation.IsShooting = isShooting;
        }
    }
}
