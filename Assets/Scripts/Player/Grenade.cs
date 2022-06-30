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
        [HideInInspector] private float bombExplosion;
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
                Destroy(rb);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            print(collision.transform.name);


            if (collision.gameObject.CompareTag("Player"))
            {
                // The player was hit. Hurtie wurtie shmurtie.
                /*                if (amount == 0) amount = damageAmount;
                                collision.collider.GetComponent<PlayerCombat>().TakeDamage(amount);
                                Destroy(this.gameObject);
                */
            }
            else
            {
                Destroy(this.gameObject, 4f);
            }
            return;
        }

        private void Explode()
        {
            GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject unit in enemyUnits)
            {
                if (Vector3.Distance(unit.transform.position, this.transform.position) < bombExplosionRadius)
                {
                    unit.GetComponent<EnemyCore>().TakeDamage(grenadeDamage);
                }
            }
            Destroy(this.gameObject);
        }
    }
}
