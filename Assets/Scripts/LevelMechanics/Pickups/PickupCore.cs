using UnityEngine;

namespace Endless.Pickup
{
    public class PickupCore : MonoBehaviour
    {
        private Rigidbody rb;
        private SphereCollider sphereColl;
        private BoxCollider boxColl;
        public float GravityLength;

        public void Create()
        {
            // Creating components
            if (!TryGetComponent(out boxColl))
                boxColl = gameObject.AddComponent<BoxCollider>();
            if (!TryGetComponent(out rb))
                rb = gameObject.AddComponent<Rigidbody>();
            // acts as a ground thing
            if (!TryGetComponent(out sphereColl))
                sphereColl = gameObject.AddComponent<SphereCollider>();
            boxColl.size = new Vector3(0.1f, 0.1f, 0.1f);
            boxColl.center = new Vector3(0, -0.2f, 0);

            // To hit to get the buff
            sphereColl.radius = 0.25f;
            sphereColl.isTrigger = true;

            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Invoking so it hits the ground first
            if (GravityLength > 0f) Invoke(nameof(GravityEffect), GravityLength);
        }

        private void GravityEffect()
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
