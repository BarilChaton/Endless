using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Movement;
using Endless.Attacker;

namespace Endless.Control
{
    public class AIController : MonoBehaviour
    {
        public static void AI(GameObject self)
        {
            GameObject player = GameObject.FindWithTag("Player");
            EnemyCore selfCore = self.transform.GetComponent<EnemyCore>();
            float distanceToPlayer = DistanceCalc.DistanceToPlayer(player, self);

            // If player is found, act aggressive
            if (player != null && distanceToPlayer < selfCore.aggressionDistance)
            {
                Attack.InitiateAttack(self, selfCore, distanceToPlayer);
            }

            // Otherwise just kinda roam idk
            else
            {

            }
        }
    }
}
