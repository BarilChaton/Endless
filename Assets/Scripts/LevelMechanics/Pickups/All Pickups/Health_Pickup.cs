using Endless.PlayerCore;
using UnityEngine;

namespace Endless.Pickup
{
    [RequireComponent(typeof(AudioSource))]
    public class Health_Pickup : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioClip pickupSound;
        [SerializeField] private bool isPercentageBased;
        [SerializeField] private float amount;
        [SerializeField]
        [Range(0, 25)]
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
            
            if (other.TryGetComponent(out PlayerCombat effectee) && PlayerCombat.maxHp > effectee.GetComponent<PlayerCombat>().playerCurrHp)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                effectee.PlayerHealed(isPercentageBased ? (amount / 100 * PlayerCombat.maxHp) : amount);
                Destroy(gameObject);
            }
        }
    }
}