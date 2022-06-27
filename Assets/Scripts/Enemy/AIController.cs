using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Movement;
using Endless.Attacker;

namespace Endless.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 20f;
        [SerializeField] float attackRange = 5f;

        private void Update()
        {
            GameObject player = GameObject.FindWithTag("Player");

            // If player is found, act aggressive
            if (player != null)
            {
                transform.LookAt(player.transform);
                // Attack range, therefore murder
                if (DistanceToPlayer(player) < attackRange)
                {
                    GetComponent<Mover>().Moving(player, false);

                    // Ranged attack
                    if (DistanceToPlayer(player) > 1)
                    {
                        GetComponent<Attack>().RangedAttack(5);
                    }

                    // Melee attack
                    else
                    {
                    }
                }

                // Chase range, therefore Cmerefamalam
                else if (DistanceToPlayer(player) < chaseDistance)
                {
                    GetComponent<Mover>().Moving(player, true);
                }
            }

            // Otherwise just kinda roam idk
            else
            {
                print("Roam");
            }
        }


        private float DistanceToPlayer(GameObject player)
        {
            // Finds the distance to the object with tag Player in the Inspector
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }
}
