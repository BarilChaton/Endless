using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Attacker
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] float ObjectSpeed = 60f;
        [SerializeField] bool isBomb = true;
        [SerializeField] float bombRemain = 5f;


        public void ThrowGrenade(GameObject Projectile, GameObject Player)
        {
            Vector3 spawnPosition = Player.transform.position + new Vector3(0, 0.5f, 0);

            // If the weapon is not a bomb, it will destroy itself on first collision
            Rigidbody rb = Instantiate(Projectile, spawnPosition, Camera.main.transform.rotation).GetComponent<Rigidbody>();

            // DIRECTIONAL STUFF IS HERE MATE
            rb.AddForce(Camera.main.transform.forward * ObjectSpeed, ForceMode.Impulse);

            // Adding rigidbody values
            rb.useGravity = isBomb ? true : false;

            // Actual Grenade stuff goes here
            if (isBomb)
            {
                Destroy(rb, bombRemain);
                // Kaboom explosions go here
                // Damoog goes here
            }
            else
            {
                print("Kaboom");
            }
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            int amount = 0;
            print(collision.transform.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                // The player was hit. Hurtie wurtie shmurtie.
                if (amount == 0) amount = damageAmount;
                collision.collider.GetComponent<PlayerCombat>().TakeDamage(amount);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject, 4f);
            }
            return;
        }*/
    }
}
