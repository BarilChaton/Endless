using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Control;
using Endless.TypeOfEnemies;

public class EnemyCore : MonoBehaviour
{
    [Header("Enemy Base Information")]
    [SerializeField] public EnemyTypes.Types enemyType;
    [SerializeField] public float aggressionDistance = 20f;
    [SerializeField] public GameObject hurtImpact;

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
    private Animator spriteAnim;
    private AngleToPlayer angleToPlayer;

    private void Awake()
    {
        spriteAnim = GetComponentInChildren<Animator>();
        angleToPlayer = GetComponent<AngleToPlayer>();
        attackRangeTemp = attackRange;
    }

    void Update()
    {
        // beginning of update set the animation to rotational index.
        spriteAnim.SetFloat("spriteRot", angleToPlayer.lastIndex);

        AIController.AI(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        spriteAnim.SetTrigger("IsHit");
        if (enemyHealth <= 0) DeathAction();
    }

    private void DeathAction()
    {
        spriteAnim.SetBool("Dead", true);


        //Destroy(gameObject);
    }
}

