using UnityEngine;
using Endless.TypeOfEnemies;
using Endless.Movement;
using Endless.PlayerCore;

namespace Endless.Attacker
{
    public class Attack : MonoBehaviour
    {
        private EnemyTypes.Types type;

        public void Awake()
        {
            type = gameObject.GetComponent<EnemyCore>().enemyType;
        }

        public void InitiateAggress()
        {
            switch (type)
            {
                case EnemyTypes.Types.SpiderVermin:
                    SpiderVerminAttack();
                    break;
            }
        }

        // SPIDER VERMIN ATTACKS
        public void SpiderVerminAttack()
        {
            GameObject player = GameObject.FindWithTag("Player");
            EnemyCore owner = transform.GetComponent<EnemyCore>();
            float distanceToPlayer = DistanceCalc.DistanceToPlayer(player, gameObject);

            // Attack range, therefore murder
            if (distanceToPlayer < owner.attackRangeTemp)
            {
                transform.TransformDirection(player.transform.position);
                owner.attackRangeTemp = owner.attackRange * 1.25f;
                Mover.Moving(false, gameObject);

                // Ranged attack. Will cancel if enemy runs inside range to instead melee smack
                if (distanceToPlayer > owner.meleeRange)
                {
                    if (owner.rangedAttackCd < Time.time)
                    {
                        owner.rangedAttackCd = Time.time + owner.rangedAttackSpeed;
                        owner.shotTarget = new Ray(transform.position, transform.forward);
                        owner.shotReady = Time.time + owner.rangedAimTime;
                    }
                    // Actual Ranged attack
                    if (owner.shotReady < Time.time)
                    {
                        Physics.Raycast(owner.shotTarget, out RaycastHit hit);
                        print("Name: " + gameObject.name + " has hit: " + hit.collider.name);
                        if (hit.collider.CompareTag("Player"))
                        {
                            hit.collider.GetComponent<PlayerCombat>().PlayerTakeDamage(owner.rangedDamage);
                        }
                        try { owner.spriteAnim.Play("AttackRanged"); }
                        catch { print("No shoot anim"); }
                        owner.shotReady = Time.time + owner.rangedAttackCd;
                    }
                }

                // Melee attack
                else
                {
                    if (distanceToPlayer <= owner.meleeRange && owner.meleeAttackCd < Time.time)
                    {
                        owner.meleeAttackCd = Time.time + owner.meleeAttackSpeed;
                        player.GetComponent<PlayerCombat>().PlayerTakeDamage(owner.meleeDamage);
                    }

                    // Resetting ranged attack CDs to avoid attack dump for running in and out of melee
                    // No attack of opportunity for this boyyo
                    owner.rangedAttackCd = Time.time + owner.rangedAttackSpeed;
                }
            }

            // Chase range, therefore Cmerefamalam
            else if (distanceToPlayer < owner.aggressionDistance)
            {
                owner.attackRangeTemp = owner.attackRange;
                Mover.Moving(true, gameObject, player, owner.speed);
            }

        }
    }
}