using UnityEngine;
using Endless.TypeOfEnemies;
using Endless.Movement;
using Endless.PlayerCore;
using Endless.Control;

namespace Endless.Attacker
{
    public class Attack : MonoBehaviour
    {
        private EnemyTypes.Types type;
        public GameObject target;
        private EnemyCore owner;

        public void Start()
        {
            target = FindObjectOfType<PlayerController>().gameObject;
            owner = transform.GetComponent<EnemyCore>();
            type = owner.enemyType;
        }

        public void InitiateAggress()
        {
            Debug.Log("Start of aggression: " + target.name);
            switch (type)
            {
                case EnemyTypes.Types.Base:
                    Debug.Log(owner.name + "I can't do anything because I'm basic. (lol)");
                    break;

                case EnemyTypes.Types.BasicRanged:
                    BasicRangedAttack();
                    break;

                case EnemyTypes.Types.BasicMeleeAndRanged:
                    BasicRangedAndMelee();
                    break;
            }
        }

        // I can do both! :o
        private void BasicRangedAndMelee()
        {
            float distanceToTarget = DistanceCalc.DistanceToPlayer(target, gameObject);

            // Attack range, therefore murder
            if (distanceToTarget > owner.meleeRange)
            {
                BasicRangedAttack();
            }
            else
            {
                BasicMeleeAttack();

            }
        }

        private void BasicRangedAttack()
        {
            Debug.Log("Start of Attack: " + target.name);
            float distanceToTarget = DistanceCalc.DistanceToPlayer(target, gameObject);

            // Time to shoot m8
            if (distanceToTarget < owner.attackRangeTemp)
            {
                transform.TransformDirection(target.transform.position);
                owner.attackRangeTemp = owner.attackRange * 1.25f;
                Mover.Moving(false, gameObject);

                // Ranged attack. Will cancel if enemy runs inside range to instead melee smack
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

                    // Dealing damage to the target based on if enemy or player
                    if (hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<PlayerCombat>().PlayerTakeDamage(owner.rangedDamage);
                    }
                    else if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<EnemyCore>().TakeDamage(owner.rangedDamage);
                        if (target != hit.collider.gameObject)
                        {
                            target = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Attack>().target = gameObject;
                            hit.collider.gameObject.GetComponentInChildren<EnemySpriteLook>().target = gameObject.transform;
                        }
                    }

                    // Animation stuff goes here
                    try { owner.spriteAnim.Play("AttackRanged"); }
                    catch { Debug.Log("No shoot anim"); }
                    owner.shotReady = Time.time + owner.rangedAttackCd;
                    Debug.Log("End of Attack: " + target.name);
                }
            }

            // Out of ranged but inside aggression range
            else if (distanceToTarget < owner.aggressionDistance)
            {
                owner.attackRangeTemp = owner.attackRange;
                Mover.Moving(true, gameObject, target, owner.speed);
            }
        }

        private void BasicMeleeAttack()
        {
            // Distance calc + stare at target
            float distanceToTarget = DistanceCalc.DistanceToPlayer(target, gameObject);
            transform.TransformDirection(target.transform.position);

            // If within range
            if (distanceToTarget <= owner.meleeRange)
            {
                // Sorting out movement
                Mover.Moving(false, gameObject);

                // The attack
                if (owner.meleeAttackCd < Time.time)
                {
                    // Cooldown + damage
                    owner.meleeAttackCd = Time.time + owner.meleeAttackSpeed;
                    if (target.CompareTag("Player")) target.GetComponent<PlayerCombat>().PlayerTakeDamage(owner.meleeDamage);
                    else if (target.CompareTag("Enemy")) target.GetComponent<EnemyCore>().TakeDamage(owner.meleeDamage);

                    // animation
                    try { owner.spriteAnim.Play("MeleeAttack"); }
                    catch { Debug.Log("No melee anim"); }
                }
            }

            // Within range of anger but not to hit
            else if (distanceToTarget < owner.aggressionDistance)
            {
                owner.attackRangeTemp = owner.attackRange;
                Mover.Moving(true, gameObject, target, owner.speed);
            }
        }
    }
}