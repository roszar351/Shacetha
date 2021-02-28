using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public static int slimesPerSplit = 2;
    public LayerMask myEnemyLayers;
    
    [SerializeField]
    [Tooltip("Always splits into two smaller versions, this decides how many times it will split.")]
    private int splitsLeft = 2;
    
    [SerializeField] 
    private GameObject explosionParticles;

    [SerializeField]
    private GameObject slimePrefab;
    
    [SerializeField]
    private float jumpAttackCooldown = 1f;

    [SerializeField]
    private GameObject shadowObject;

    [SerializeField]
    private Transform spriteTransform;

    [SerializeField]
    private Collider2D myCollider;

    [SerializeField]
    private GameObject puddlePrefab;
    
    [SerializeField] private float rangeMultiplier = 10f;
    
    private Node _rootNode;
    private bool _isExploding = false;
    private float _damageMultiplier = 1.5f;
    private float _currentAttackCooldown = 0f;

    public static Slime Create(Vector3 position, GameObject slimePrefab, int splitsLeft)
    {
        GameObject slime = Instantiate(slimePrefab, position, Quaternion.identity);
        
        Slime slimeScript = slime.GetComponent<Slime>();
        slimeScript.SetUpSlime(splitsLeft);

        return slimeScript;
    }

    public void SetUpSlime(int splitsLeft)
    {
        _currentAttackCooldown = Random.Range(0f, jumpAttackCooldown);
        this.splitsLeft = splitsLeft;
        transform.localScale = transform.localScale * 0.5f;
        explosionParticles.SetActive(false);
        _damageMultiplier = .5f + .5f * splitsLeft;
        currentHp = (int) (currentHp * 0.75f);
        totalArmor = (int) (totalArmor * .5f);
        
        StartCoroutine(nameof(GrowOnStart));
    }

    IEnumerator GrowOnStart()
    {
        shadowObject.SetActive(false);
        float maxScale = transform.localScale.x;
        Vector3 currentScale = new Vector3(0.1f, 0.1f, 1f);
        bool done = false;
        transform.localScale = currentScale;
        
        while (!done)
        {
            currentScale += new Vector3(0.1f, 0.1f, 0);
            if (currentScale.x >= maxScale)
            {
                currentScale = new Vector3(maxScale, maxScale, maxScale);
                done = true;
            }

            transform.localScale = currentScale;
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void Start()
    {
        base.Start();
        name = "Slime";
        shadowObject.SetActive(false);
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        _currentAttackCooldown -= Time.fixedDeltaTime;
        _rootNode.Execute();
    }

    public float GetDamageMultiplier()
    {
        return _damageMultiplier;
    }

    private void ConstructBehaviourTree()
    {
        Transform myTransform = transform;
        
        AbilityNode abilityNode = new AbilityNode(this);
        CheckBoolNode checkBoolNode = new CheckBoolNode(this);
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(myTransform, target, myStats.attackRange * rangeMultiplier);

        IdleNode idleNode = new IdleNode(this);
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        Sequence abilitySequence = new Sequence((new List<Node> {checkBoolNode, abilityNode}));

        _rootNode = new Selector(new List<Node> { abilitySequence, attackSequence, idleNode });
    }

    public override bool UseAbility()
    {
        myAnimations.PlayAbilityTell();
        Invoke("Die", 1f);
        return true;
    }

    public override void Attack()
    {
        if (attacking || _currentAttackCooldown > 0)
            return;

        attacking = true;
        StartCoroutine(nameof(DoJumpAttack));
    }

    IEnumerator DoJumpAttack()
    {
        myCollider.enabled = false;
        // Start of jump
        AudioManager.instance.PlayOneShotSound("SlimeAttack");
        
        float addedVal = 0.2f;
        float currentY = 0f;
        float targetVariance = 1f + Random.Range(-0.3f, 0.3f);
        Vector2 movementVector = (target.position - transform.position) * targetVariance;
        bool done = false;
        bool goingUp = true;
        shadowObject.SetActive(true);

        while (!done)
        {
            currentY += addedVal;
            if (goingUp && currentY >= 1f)
            {
                currentY = 1f;
                goingUp = false;
                addedVal = addedVal * -1;
            }
            else if(!goingUp && currentY <= 0f)
            {
                currentY = 0f;
                done = true;
            }
            //transform.localScale = currentScale;
            //transform.position += new Vector3(0, addedVal, 0);
            spriteTransform.localPosition = new Vector3(0f, currentY, 0f);
            
            rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
            
            yield return new WaitForSeconds(0.1f);
        }

        // End of jump
        StaticTrap.Create(transform.position - new Vector3(0, .3f, 0),
            new Vector3(transform.localScale.x * 4, transform.localScale.y * 2f, 1), puddlePrefab, 10f, (int)(myStats.baseDamage * _damageMultiplier));
        shadowObject.SetActive(false);
        myCollider.enabled = true;
        AudioManager.instance.PlayOneShotSound("SlimeAttack");
        _currentAttackCooldown = jumpAttackCooldown;
        attacking = false;
    }

    public override void TakeDamage(int damageAmount)
    {
        if (_isExploding)
            return;
        
        base.TakeDamage(damageAmount);
        if ((float)currentHp / myStats.maxHp < .25f)
        {
            _isExploding = true;
        }
    }

    protected override void Die()
    {
        if (isDying)
            return;
        
        explosionParticles.SetActive(true);
        Collider2D[] damageArea = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * .9f, myEnemyLayers);
        if (damageArea.Length > 0)
        {
            PlayerManager.instance.DealDamageToPlayer((int) (myStats.baseDamage * (0.2f + _damageMultiplier)));
        }
        StaticTrap.Create(transform.position - new Vector3(0, .3f, 0),
            new Vector3(transform.localScale.x * 4f, transform.localScale.y * 2f, 1), puddlePrefab, 20f, (int)(myStats.baseDamage * _damageMultiplier));
        //explosionParticles.SetActive(false);

        if (splitsLeft > 0)
        {
            for (int i = 0; i < slimesPerSplit; ++i)
            {
                Vector3 randomOffSet = new Vector3(Random.Range(-0.5f, 0.5f),Random.Range(-0.5f, 0.5f),0);
                Slime.Create(transform.position + randomOffSet * 2, slimePrefab, splitsLeft-1);
            }
        }

        base.Die();
    }

    public override bool CheckBool(int whichBool = 0)
    {
        return _isExploding;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * 1f);
    }
}
