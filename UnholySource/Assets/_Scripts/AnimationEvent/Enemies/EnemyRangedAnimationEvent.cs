using Core.Enemies;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class EnemyRangedAnimationEvent : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private EnemyRangedShootState shootState;
        [SerializeField] private EnemyRangedDeathState deathState;

        public void Shoot()
        {
            shootState.InstantiateProjectile();
        }

        public void ShootingEnd()
        {
            shootState.ShootingEnded();
        }

        public void IsDead()
        {
            deathState.IsDead();
        }
    }
}
