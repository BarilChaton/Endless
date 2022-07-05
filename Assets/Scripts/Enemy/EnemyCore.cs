using UnityEngine;
using Endless.Control;
using Endless.TypeOfEnemies;

public class EnemyCore : MonoBehaviour
{
    // Actual base stuff
    [SerializeField] public EnemyTypes.Types enemyType;
    [SerializeField] public float aggressionDistance = 20f;
    [SerializeField] public GameObject hurtImpact = null;

    // Base stuff
    public float enemyHealth = 5;
    public float maxHealth = 5;
    [SerializeField] private float armour = 0;
    public float speed = 10f;

    // Ranged stuff
    public int rangedDamage = 10;
    public float attackRange = 5f;
    public float rangedAttackSpeed = 2f;
    public float rangedAimTime = 0.1f;
    public float rangedAttackCd;
    public float shotReady;
    [HideInInspector] public Ray shotTarget;

    // Melee stuff
    public int meleeDamage = 20;
    public float meleeRange = 1f;
    public float meleeAttackSpeed = 2f;
    [HideInInspector] public float meleeAttackCd;
    [HideInInspector] public float attackRangeTemp;

    // Variables created
    public Animator spriteAnim;
    private EnemySpriteLook enemySpriteLook;
    private AIController aiController;
    private Rigidbody rb;

    // Debug information
    public float radius = 20f;
    [Range(0, 360)]
    public float angle = 40f;
    public GameObject player;

    [HideInInspector] public bool canSeeTarget;


    public bool showVisionInfo;
    public bool showDebugInfo;

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

        enemyHealth = maxHealth;

        attackRangeTemp = attackRange;
    }

    void FixedUpdate()
    {
        aiController.AI();
    }

    public void TakeDamage(float damage, bool isHeal = false)
    {
        // Take damage
        float result = armour - damage;
        if (!isHeal) enemyHealth += Mathf.Clamp(result, -damage, 0f);

        // Animation attempt
        try { spriteAnim.SetTrigger("IsHit"); }
        catch { }

        // Look at who hit them
        enemySpriteLook.StareAtShooter(true);

        // Death stuff
        if (enemyHealth <= 0) DeathAction();
    }

    public void ActDead()
    {
        try { spriteAnim.SetBool("Dead", true); }
        catch { Destroy(gameObject); }
    }

    private void DeathAction()
    {
        Component[] components = GetComponents<Component>();
        foreach (Component t in components)
        {
            if (t is EnemyCore || t is Transform)
                continue;
            Destroy(t);
        }
        gameObject.layer = default;
        GetComponentInChildren<Transform>().gameObject.layer = default;
        ActDead();
        enabled = false;
    }
}

