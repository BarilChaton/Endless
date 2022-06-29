using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float speed = 10f;

        public bool Moving(GameObject target, bool chase)
        {
            Vector3 targetDest = target.transform.position;
            float rlSpeed;
            if (!chase)
            {
                rlSpeed = 0f;
            }
            else rlSpeed = speed;
            transform.position = Vector3.MoveTowards(transform.position, targetDest, rlSpeed * Time.deltaTime);
            return (rlSpeed > 0f) ? true : false;
        }
    }
}