using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalc : MonoBehaviour
{
    public static float DistanceToPlayer(GameObject player, GameObject owner)
    {
        // Finds the distance to the object with tag Player in the Inspector
        return Vector3.Distance(player.transform.position, owner.transform.position);
    }
}
