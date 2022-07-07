using UnityEngine;
using Endless.GunSwap;

namespace Endless.Pickup
{
    [RequireComponent(typeof(AudioSource))]
    public class Shotgun_Pickup : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioClip pickupSound;
        [SerializeField] private int bulletAmount;
        [SerializeField]
        [Range(0, 25)]
        [Tooltip("Amount of time gravity affects this before freezing. (This is so it drops to the floor).\n0 means Gravity ALWAYS affects it.")]
        private float gravityEffectTime = 0;
        private PickupCore pickup;
        public GameObject gunPrefab;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (!TryGetComponent(out pickup))
                pickup = gameObject.AddComponent<PickupCore>();
            pickup.GravityLength = gravityEffectTime;
            pickup.Create();
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.TryGetComponent(out WeaponSwapper effectee))
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                effectee.AddGunToInventory(gunPrefab.name, bulletAmount);
                Destroy(gameObject);
            }
        }


    }
}