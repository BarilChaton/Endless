using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Movement;
using Endless.Attacker;
using System;
using System.Linq;

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

        public bool canSeeTarget = false;
        public bool wasHit = false;

        private int seentar = 0;

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
            radius = core.aggressionDistance;
            angle = core.angle;

            StartCoroutine(FOVRoutine());
        }

        public void AI()
        {
            // If player is found, act aggressive
            if (canSeeTarget || wasHit)
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
            // Area in front of unit
            Collider[] frontChecks = (attack.target == player) ?
                Physics.OverlapSphere(transform.position, radius, playerMask) :
                Physics.OverlapSphere(transform.position, radius, allyMask);
            // Area around unit
            Collider[] meleeChecks = (attack.target == player) ?
                Physics.OverlapSphere(transform.position, core.meleeRange, playerMask) :
                Physics.OverlapSphere(transform.position, core.meleeRange, allyMask);

            Collider[] rangeChecks = frontChecks.Concat(meleeChecks).ToArray();
            foreach (Collider check in rangeChecks)
            {
                print(check);
            }

            if (rangeChecks.Length != 0)
            {
                Transform allyTarget = attack.target.transform;

                // Initial hostile target does not exist. Choose another enemy.
                if (allyTarget.gameObject.layer == default && rangeChecks.Length >= 2)
                {
                    // If new target is self, then.. don't? lol
                    allyTarget = (rangeChecks[0].gameObject == this.gameObject) ?
                        rangeChecks[1].transform : rangeChecks[0].transform;

                    attack.target = allyTarget.gameObject;
                    attack.GetComponentInChildren<EnemySpriteLook>().target = allyTarget;
                }
                else if (rangeChecks.Length == 1) attack.target = player;

                // Set target position
                Transform target = (attack.target == player) ?
                    rangeChecks[0].transform : allyTarget;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) canSeeTarget = true;
                    else canSeeTarget = false;
                }
                else canSeeTarget = false;
            }
            else if (attack.target != player)
            {
                attack.target = player;
                canSeeTarget = false;
            }
            else if (canSeeTarget) canSeeTarget = false;

            // SOUND GOES HERE OKAY
            if (canSeeTarget && seentar == 0)
            {
                AudioClip sound = core.aggroSound;
                if (sound != null) core.audioSource.PlayOneShot(sound);
                seentar++;
            }
            else if (!canSeeTarget) seentar = 0;
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
