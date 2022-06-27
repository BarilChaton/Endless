using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Attacker
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] float attackCooldown = 2f;
        [SerializeField] GameObject Projectile;
        bool hasAttacked = false;


        public void RangedAttack(int amount)
        {
            if (!hasAttacked)
            {
                Rigidbody rb = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);

                hasAttacked = true;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }

        private void ResetAttack()
        {
            hasAttacked = false;
        }
    }
}