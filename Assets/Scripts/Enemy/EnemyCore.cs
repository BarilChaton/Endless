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
    private AngleToPlayer angleToPlayer;
    private AIController aiController;

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
        angleToPlayer = gameObject.AddComponent<AngleToPlayer>();
        aiController = gameObject.AddComponent<AIController>();
        attackRangeTemp = attackRange;
    }

    void Update()
    {
        // beginning of update set the animation to rotational index.
        try
        {
            spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);
        }
        catch { }
        aiController.AI();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        try { spriteAnim.SetTrigger("IsHit"); }
        catch { }
        if (enemyHealth <= 0) DeathAction();
    }

    private void DeathAction()
    {
        try { spriteAnim.SetBool("Dead", true); }
        catch { Destroy(gameObject); }
        finally { enabled = false; }
    }
}

