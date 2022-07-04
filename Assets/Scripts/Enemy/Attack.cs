using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.TypeOfEnemies;

namespace Endless.Attacker
{
    public class Attack : MonoBehaviour
    {
        // Reworking Enemy attacking

        // When you get it working don't forget to add somewhere a "Shooting" bolean to true while attacking for the animator.
        public static void InitiateAttack(GameObject self, EnemyCore owner, float distanceToPlayer = 0f)
        {
            if (owner.enemyType == EnemyTypes.Types.SpiderVermin)
            {
                SpiderVerminAggression.SpiderVerminAttack(self, owner, distanceToPlayer);
            }
        }
    }
}