using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap02 : MonoBehaviour
{
    public GameObject player;
    public GameObject falseWall;
    public GameObject activator;
    private bool isActivated = false;
    private void OnTriggerEnter(Collider player)
    {
        if (falseWall.activeInHierarchy)
        {
            Destroy(falseWall);
            isActivated = true;
        }

        if (isActivated)
        {
            Destroy(activator);
        }
    }
}
