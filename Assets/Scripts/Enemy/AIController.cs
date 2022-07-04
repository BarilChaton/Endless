using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Movement;
using Endless.Attacker;

namespace Endless.Control
{
    public class AIController : MonoBehaviour
    {
        public LayerMask targetMask;
        public LayerMask obstructionMask;
        private Attack attack;

        [HideInInspector] private float radius = 20f;
        [Range(0, 360)]
        [HideInInspector] private float angle = 40f;
        [HideInInspector] public GameObject player;

        public bool canSeePlayer;

        private void Start()
        {
            EnemyCore core = gameObject.GetComponent<EnemyCore>();
            targetMask = LayerMask.GetMask("Player");
            obstructionMask = LayerMask.GetMask("Walls", "Ground");
            player = core.player;
            attack = gameObject.AddComponent<Attack>();
            radius = core.radius;
            angle = core.angle;
            StartCoroutine(FOVRoutine());
        }

        public void AI()
        {
            // If player is found, act aggressive
            if (canSeePlayer)
            {
                attack.InitiateAggress();
            }

            // Otherwise just kinda roam idk
            else
            {
                // Finally, stop moving
                Mover.Moving(false, gameObject);
            }
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new(0.2f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) canSeePlayer = true;
                    else canSeePlayer = false;
                }
                else canSeePlayer = false;
            }
            else if (canSeePlayer) canSeePlayer = false;
        }
    }
}
