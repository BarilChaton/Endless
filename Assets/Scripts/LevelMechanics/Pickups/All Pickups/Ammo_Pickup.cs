using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;

namespace Endless.Pickup
{
    public class Ammo_Pickup : MonoBehaviour
    {
       /* [SerializeField] private float amount;
        [SerializeField]
        [Range(0, 25)]
        [Tooltip("Amount of time gravity affects this before freezing. (This is so it drops to the floor).\n0 means Gravity ALWAYS affects it.")]
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
                effectee.GetComponent<GunCore>(); // THIS NEEDS WORK
        }*/
    }
}