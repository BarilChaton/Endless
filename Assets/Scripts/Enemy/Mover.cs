using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Movement
{
    public class Mover : MonoBehaviour
    {
        public static bool Moving(bool chase, GameObject owner, GameObject target = null, float speed = 0f)
        {
            Vector3 targetDest;
            float rlSpeed;
            if (!chase)
            {
                rlSpeed = 0f;
            }
            else
            {
                targetDest = new Vector3(target.transform.position.x, owner.transform.position.y, target.transform.position.z);
                rlSpeed = speed;
                owner.transform.position = Vector3.MoveTowards(owner.transform.position, targetDest, rlSpeed * Time.deltaTime);
            }
            return (rlSpeed > 0f) ? true : false;
        }
    }
}