using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless.Pickup
{
    public class Pickup : MonoBehaviour
    {
        private Rigidbody rb;
        private SphereCollider sphereColl;

        private void Awake()
        {
            if (!TryGetComponent(out rb))
                rb = gameObject.AddComponent<Rigidbody>();
            if (!TryGetComponent(out sphereColl))
                sphereColl = gameObject.AddComponent<SphereCollider>();
            sphereColl.radius = 0.25f;
            sphereColl.isTrigger = true;
            rb.useGravity = true;
            rb.freezeRotation = true;
        }
    }
}
