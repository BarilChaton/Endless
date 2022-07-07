using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDisappearance : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] LayerMask layer;

    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
