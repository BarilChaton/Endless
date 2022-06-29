using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;

namespace Endless.Damage
{
    public class PlayerDamage : MonoBehaviour
    {
        [SerializeField] int damageAmount = 5;

        private void OnCollisionEnter(Collision collision)
        {
            int amount = 0;
            print(collision.transform.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                // The player was hit. Hurtie wurtie shmurtie.
                if (amount == 0) amount = damageAmount;
                collision.collider.GetComponent<PlayerCombat>().PlayerTakeDamage(amount);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject, 4f);
            }
            return;
        }
    }
}
