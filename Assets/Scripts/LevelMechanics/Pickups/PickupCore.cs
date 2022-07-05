using UnityEngine;

namespace Endless.Pickup
{
    public class PickupCore : MonoBehaviour
    {
        private Rigidbody rb;
        private SphereCollider sphereColl;
        public float GravityLength;

        public void Create()
        {            
            if (!TryGetComponent(out rb))
                rb = gameObject.AddComponent<Rigidbody>();
            if (!TryGetComponent(out sphereColl))
                sphereColl = gameObject.AddComponent<SphereCollider>();
            sphereColl.radius = 0.25f;
            sphereColl.isTrigger = true;
            rb.useGravity = true;
            // Invoking so it hits the ground first
            if (GravityLength > 0f) Invoke(nameof(GravityEffect), 0.3f);
        }

        private void GravityEffect()
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
