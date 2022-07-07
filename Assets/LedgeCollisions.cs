using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCollisions : MonoBehaviour
{
    private GameObject bulletHole;
    private GameObject Player;

    void Start()
    {
        bulletHole = GameObject.FindGameObjectWithTag("Bullet");
        Physics.IgnoreCollision(bulletHole.GetComponent<Collider>(), GetComponent<Collider>());

        Player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(Player.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
