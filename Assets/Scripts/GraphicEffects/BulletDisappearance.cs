using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDisappearance : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
