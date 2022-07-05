using UnityEngine;
using Endless.PlayerCore;
using Endless.Pickup;

public class Armour : MonoBehaviour
{
    [SerializeField] private float amount;
    private Pickup pickup;

    private void Awake()
    {
        if (!TryGetComponent(out pickup))
            gameObject.AddComponent<Pickup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Payer")) other.GetComponent<PlayerCombat>().PlayerGetArmour(amount);
    }
}
