using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Movement
{
    public class Mover : MonoBehaviour
    {
        public static void Moving(bool chase, GameObject owner, GameObject target = null, float speed = 0f)
        {
            Vector3 ownerPos = owner.transform.position;
            Vector3 targetPos = target != null ? target.transform.position : Vector3.zero;
            Vector3 targetDest = chase ? targetPos : ownerPos;


            owner.transform.position = Vector3.MoveTowards(owner.transform.position, targetDest, speed * Time.deltaTime);
            owner.GetComponent<EnemyCore>().spriteAnim.SetBool("Walking", ownerPos != targetDest);
        }
    }
}