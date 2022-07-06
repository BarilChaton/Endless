using Endless.PlayerCore;
using UnityEngine;

namespace Endless.Pickup
{
    public class Armour_Pickup : MonoBehaviour
    {
        [SerializeField] private bool isPercentageBased;
        [SerializeField] private float amount;
        [SerializeField]
        [Range(-0.01f, 25)]
        [Tooltip("Amount of time gravity affects this before freezing. (This is so it drops to the floor).\n0.01 means Gravity ALWAYS affects it.")]
        private float gravityEffectTime = 0;
        private PickupCore pickup;

        private void Awake()
        {
            if (!TryGetComponent(out pickup))
                pickup = gameObject.AddComponent<PickupCore>();
            pickup.GravityLength = gravityEffectTime;
            pickup.Create();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCombat effectee))
            {
                effectee.PlayerGetArmour(isPercentageBased ? (amount / 100 * PlayerCombat.maxArmour) : amount);
                Destroy(gameObject);
            }
        }
    }
}