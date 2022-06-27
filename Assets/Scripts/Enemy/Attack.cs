using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Attacker
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] GameObject Projectile;
        [SerializeField] float shootRateInSeconds = 2f;
        [SerializeField] float bulletSpeed = 60f;
        [SerializeField] bool isBomb = false;
        [SerializeField] float bombRemain = 0f; 
        bool hasAttacked = false;


        public void RangedAttack()
        {
            if (!hasAttacked)
            {
                // If the weapon is not a bomb, it will destroy itself on collision
                Rigidbody rb = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
                hasAttacked = true;
                Invoke(nameof(ResetAttack), shootRateInSeconds);
            }
        }

        private void ResetAttack()
        {
            hasAttacked = false;
        }
    }
}