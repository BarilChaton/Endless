using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        private void Update()
        {
            
            if (DistanceToPlayer() < chaseDistance)
            {
                print("Should Chase!");
            }
        }

        private float DistanceToPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }
}
