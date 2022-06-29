using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.TypeOfEnemies;

namespace Endless.Attacker
{
    public class Attack : MonoBehaviour
    {
        // Reworking Enemy attacking
        public static void InitiateAttack(GameObject self, EnemyCore owner, float distanceToPlayer = 0f)
        {
            if (owner.enemyType == EnemyTypes.Types.SpiderVermin)
            {
                SpiderVerminAggression.SpiderVerminAttack(self, owner, distanceToPlayer);
            }
        }
    }
}