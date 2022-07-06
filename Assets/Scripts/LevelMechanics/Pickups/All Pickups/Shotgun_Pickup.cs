using UnityEngine;
using Endless.GunSwap;

namespace Endless.Pickup
{
    public class Shotgun_Pickup : MonoBehaviour
    {
        [SerializeField] private int bulletAmount;
        [SerializeField]
        [Range(0, 25)]
        [Tooltip("Amount of time gravity affects this before freezing. (This is so it drops to the floor).\n0 means Gravity ALWAYS affects it.")]
        private float gravityEffectTime = 0;
        private PickupCore pickup;
        public GameObject gunPrefab;

        private void Awake()
        {
            if (!TryGetComponent(out pickup))
                pickup = gameObject.AddComponent<PickupCore>();
            pickup.GravityLength = gravityEffectTime;
            pickup.Create();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out WeaponSwapper effectee))
            {
                effectee.AddGunToInventory(gunPrefab.name, bulletAmount);
                Destroy(gameObject);
            }
        }


    }
}