using Endless.PlayerCore;
using UnityEngine;

namespace Endless.Pickup
{
    [RequireComponent(typeof(AudioSource))]
    public class Armour_Pickup : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioClip pickupSound;
        [SerializeField] private GameObject Player;
        [SerializeField] private bool isPercentageBased;
        [SerializeField] private float amount;
        [SerializeField]
        [Range(-0.01f, 25)]
        [Tooltip("Amount of time gravity affects this before freezing. (This is so it drops to the floor).\n0.01 means Gravity ALWAYS affects it.")]
        private float gravityEffectTime = 0;
        private PickupCore pickup;

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
            
            if (other.TryGetComponent(out PlayerCombat effectee) && PlayerCombat.maxArmour > effectee.GetComponent<PlayerCombat>().playerCurrArmour)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                effectee.PlayerGetArmour(isPercentageBased ? (amount / 100 * PlayerCombat.maxArmour) : amount);
                Destroy(gameObject);
            }
        }
    }
}