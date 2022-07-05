using Endless.PlayerCore;
using UnityEngine;
using System.Collections;

namespace Endless.Control
{
    public class EnemySpriteLook : MonoBehaviour
    {
        public Transform target;
        private AIController controller;

        public bool canLookVertically;

        void Start()
        {
            target = FindObjectOfType<PlayerController>().transform;
            controller = transform.parent.GetComponent<AIController>();
            StartCoroutine(StareRoutine());
        }

        void OnEnable()
        {
            if (transform.GetComponentInParent<EnemyCore>().enemyHealth <= 0)
            {
                transform.GetComponentInParent<EnemyCore>().ActDead();
            }
            else
                StartCoroutine(StareRoutine());
        }

        private IEnumerator StareRoutine()
        {
            WaitForSeconds wait = new(0.1f);

            while (true)
            {
                yield return wait;
                StareAtShooter();
            }
        }

        public void StareAtShooter(bool gotHit = false)
        {
            Vector3 modifiedTarget = new(target.position.x, transform.position.y, target.position.z);

            if (controller.canSeeTarget || gotHit) transform.parent.LookAt(canLookVertically ? target.position : modifiedTarget);
            else transform.LookAt(canLookVertically ? target.position : modifiedTarget);
        }
    }
}