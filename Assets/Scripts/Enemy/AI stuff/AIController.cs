using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Movement;
using Endless.Attacker;

namespace Endless.Control
{
    public class AIController : MonoBehaviour
    {
        public LayerMask playerMask;
        public LayerMask obstructionMask;
        public LayerMask allyMask;
        private Attack attack;

        [HideInInspector] private float radius = 20f;
        [Range(0, 360)]
        [HideInInspector] private float angle = 40f;
        [HideInInspector] public GameObject player;
        [HideInInspector] Animator spriteAnim;
        [HideInInspector] AngleToPlayer angleToPlayer;
        [HideInInspector] private EnemyCore core;

        public bool canSeeTarget;

        private void Awake()
        {
            spriteAnim = GetComponentInChildren<Animator>();
            if (!TryGetComponent(out attack)) attack = gameObject.AddComponent<Attack>();
            if (!TryGetComponent(out angleToPlayer)) angleToPlayer = gameObject.AddComponent<AngleToPlayer>();

            playerMask = LayerMask.GetMask("Player");
            allyMask = LayerMask.GetMask("Enemies");
            obstructionMask = LayerMask.GetMask("Walls", "Ground");

            core = gameObject.GetComponent<EnemyCore>();
            player = core.player;
            radius = core.radius;
            angle = core.angle;

            StartCoroutine(FOVRoutine());
        }

        public void AI()
        {
            // If player is found, act aggressive
            if (canSeeTarget)
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

        private void OnEnable()
        {
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new(0.1f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
                RotationCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Debug.Log("Start of FoVCheck: " + attack.target.name);
            Collider[] rangeChecks = (attack.target = player) ?
                Physics.OverlapSphere(transform.position, radius, playerMask) :
                Physics.OverlapSphere(transform.position, radius, allyMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = (attack.target = player) ? rangeChecks[0].transform : attack.target.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) canSeeTarget = true;
                    else canSeeTarget = false;
                }
                else canSeeTarget = false;
            }
            else if (canSeeTarget) canSeeTarget = false;

            Debug.Log("End of FoVCheck: " + attack.target.name);
        }

        private void RotationCheck()
        {
            try
            {
                spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);
            }
            catch { }
        }
    }
}
