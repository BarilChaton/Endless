using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Control;
using Endless.TypeOfEnemies;
using Endless.Movement;

public class EnemyCore : MonoBehaviour
{
    [Header("Enemy Base Information")]
    [SerializeField] public EnemyTypes.Types enemyType;
    [SerializeField] public float aggressionDistance = 20f;
    [SerializeField] public GameObject hurtImpact = null;

    [Header("Stats")]
    [SerializeField] private float enemyHealth = 5;
    [SerializeField] public float speed = 10f;
    [SerializeField] public int rangedDamage = 10;
    [SerializeField] public int meleeDamage = 20;

    [Header("Attacking")]
    [SerializeField] public float attackRange = 5f;
    [SerializeField] public float rangedAttackSpeed = 2f;
    [HideInInspector] public float rangedAttackCd;
    [SerializeField] public float rangedAimTime = 0.1f;
    [HideInInspector] public float shotReady;
    [HideInInspector] public Ray shotTarget;

    [SerializeField] public float meleeRange = 1f;
    [SerializeField] public float meleeAttackSpeed = 2f;
    [HideInInspector] public float meleeAttackCd;

    [HideInInspector] public float attackRangeTemp;

    [Header("EnemyAnimations")]
    [HideInInspector] public Animator spriteAnim;
    private EnemySpriteLook enemySpriteLook;
    private AIController aiController;
    private Rigidbody rb;

    [Header("AI controls")]
    public float radius = 20f;
    [Range(0, 360)]
    public float angle = 40f;
    public GameObject player;

    [HideInInspector] public bool canSeePlayer;

    private void Awake()
    {
        enabled = true;
        spriteAnim = GetComponentInChildren<Animator>();
        enemySpriteLook = GetComponentInChildren<EnemySpriteLook>();

        if (!TryGetComponent(out aiController))
            aiController = gameObject.AddComponent<AIController>();

        if (!TryGetComponent(out rb))
            rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        attackRangeTemp = attackRange;
    }

    void Update()
    {
        aiController.AI();
    }

    public void TakeDamage(float damage)
    {
        // Take damage
        enemyHealth -= damage;

        // Animation attempt
        try { spriteAnim.SetTrigger("IsHit"); }
        catch { }

        // Look at Player
        enemySpriteLook.StareAtPlayer(true);

        // Death stuff
        if (enemyHealth <= 0) DeathAction();
    }

    private void DeathAction()
    {
        try { spriteAnim.SetBool("Dead", true); }
        catch { Destroy(gameObject); }
        finally { enabled = false; }
    }
}

