using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.PlayerCore;

public class BasicZombieAggression : MonoBehaviour
{
    public static void BasicZombieAttack(GameObject self, EnemyCore ownerCore, float distanceToPlayer = 0)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (distanceToPlayer <= ownerCore.meleeRange && ownerCore.meleeAttackCd < Time.time)
        {
            ownerCore.meleeAttackCd = Time.time + ownerCore.meleeAttackSpeed;
            player.GetComponent<PlayerCombat>().PlayerTakeDamage(ownerCore.meleeDamage);
        }
    }
}
