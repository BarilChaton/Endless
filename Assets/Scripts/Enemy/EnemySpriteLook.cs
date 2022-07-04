using Endless.PlayerCore;
using UnityEngine;

public class EnemySpriteLook : MonoBehaviour
{
    [SerializeField] private Transform target;
    private EnemyCore core;
    public bool canLookVertically;


    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        core = transform.parent.GetComponent<EnemyCore>();
    }
    void Update()
    {
        Vector3 modifiedTarget = new(target.position.x, transform.position.y, target.position.z);

        /// FIX ME PLEAAAAAAAAAAAAAAAAAASE thanks
        if (core.canSeePlayer) transform.parent.LookAt(canLookVertically ? target.position : modifiedTarget);

        transform.LookAt(canLookVertically ? target.position : modifiedTarget);
    }
}
