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

    private void Awake()
    {
        attackRangeTemp = attackRange;
    }

    void Update()
    {
        AIController.AI(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0) DeathAction();
    }

    private void DeathAction()
    {



        Destroy(gameObject);
    }
}

