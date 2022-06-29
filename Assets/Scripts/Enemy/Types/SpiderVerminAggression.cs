using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Movement;
using Endless.PlayerCore;

namespace Endless.Attacker
{
    public class SpiderVerminAggression : MonoBehaviour
    {
        public static void SpiderVerminAttack(GameObject self, EnemyCore owner, float distanceToPlayer = 0f)
        {
            GameObject player = GameObject.FindWithTag("Player");

            // Attack range, therefore murder
            if (distanceToPlayer < owner.attackRangeTemp)
            {
                owner.attackRangeTemp = owner.attackRange * 1.25f;
                Mover.Moving(false, self);

                // Ranged attack
                if (distanceToPlayer > owner.meleeRange && owner.rangedAttackCd < Time.time)
                {
                    owner.rangedAttackCd = Time.time + owner.rangedAttackSpeed;
                    owner.shotTarget = new Ray(self.transform.position, self.transform.forward);
                    owner.shotReady = Time.time + owner.rangedAimTime;
                }
                // Melee attack
                else
                {

                }

                // Actual Ranged attack
                if (owner.shotReady < Time.time)
                {
                    Physics.Raycast(owner.shotTarget, out RaycastHit hit);
                    if (hit.collider.tag == "Player")
                    {
                        hit.collider.GetComponent<PlayerCombat>().PlayerTakeDamage(owner.rangedDamage);
                    }
                    owner.shotReady = Time.time + owner.rangedAttackCd;
                }
            }

            // Chase range, therefore Cmerefamalam
            else if (distanceToPlayer < owner.aggressionDistance)
            {
                owner.attackRangeTemp = owner.attackRange;
                Mover.Moving(true, self, player, owner.speed);
            }
        }
    }
}