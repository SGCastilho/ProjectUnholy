using Core.Enemies;
using UnityEngine;

namespace Core.AnimationEvents
{
    public class PursuerAnimationEvent : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private PursuerBehaviour behaviour;

        public void FinishRunningAttack()
        {
            behaviour.Animation.IsRunningAttacking = false;
            behaviour.Animation.IsRunning = false;
        }

        public void DrawHurtBox()
        {
            Debug.Log("Apply dmg");
        }
    }
}
