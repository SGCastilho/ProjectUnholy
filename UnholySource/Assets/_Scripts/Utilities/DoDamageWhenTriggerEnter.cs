using Core.Interfaces;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Utilities
{
    [RequireComponent(typeof(Collider))]
    public class DoDamageWhenTriggerEnter : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private EnemyData data;

        [Header("Settings")]
        [SerializeField] private string damageTag = "Put the tag here.";

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(damageTag))
            {
                other.GetComponent<IDamagable>().ApplyDamage(data.Damage);

                gameObject.SetActive(false);
            }
        }
    }
}
