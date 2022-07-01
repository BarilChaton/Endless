using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Attacker
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] float ObjectSpeed = 60f;
        [SerializeField] float grenadeDamage = 5f;
        [SerializeField] bool isBomb = true;
        [SerializeField] float bombRemain = 5f;
        [SerializeField] float bombExplosionRadius = 5f;


        public void ThrowGrenade(GameObject Projectile)
        {
            // If the weapon is not a bomb, it will destroy itself on first collision
            Rigidbody rb = Projectile.GetComponent<Rigidbody>();

            // DIRECTIONAL STUFF IS HERE MATE
            rb.AddForce(Camera.main.transform.forward * ObjectSpeed, ForceMode.Impulse);

            // Adding rigidbody values
            rb.useGravity = isBomb ? true : false;

            // Actual Grenade stuff goes here
            if (isBomb)
            {
                Invoke("Explode", bombRemain);


                // Kaboom explosions go here
            }
            else
            {
                print("Kaboom");
                Destroy(Projectile);
            }
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, bombExplosionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy")) collider.GetComponent<EnemyCore>().TakeDamage(grenadeDamage);
            }
            Destroy(this.gameObject);
        }
    }
}
