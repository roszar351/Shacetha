using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    /*
     * This enemy should be similar to normal chase enemy in the fact it will have some form of weapon they attack the player with
     * and will try to get close to them, the main difference will be the movement and some kind of ability
     * currently thinking of spitting web maybe as a circle(similar to poison trap/slime) but slowing instead of damaging
     * another possibility is shooting web directly at the player which could just do damage or debuff, need experimentation to decide
     *
     */

    [SerializeField] private float rangeMultiplier = 10f;
    [SerializeField] private int howManyAttacksBeforeAbility = 3;
    [SerializeField] private GameObject slowTrap;
    
    private Node _rootNode;
    private int _attackCounter = 0;
    private bool _usingAbility = false;
    
    protected override void Start()
    {
        base.Start();
        name = "Spider";
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        _rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        Transform myTransform = transform;

        CheckBoolNode abilityCheckNode = new CheckBoolNode(this);
        AbilityNode spitCobwebAbilityNode = new AbilityNode(this);
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(myTransform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, myTransform, target);
        RangeNode searchRangeNode = new RangeNode(myTransform, target, myStats.attackRange * rangeMultiplier);

        Sequence abilitySequence = new Sequence(new List<Node> {abilityCheckNode, spitCobwebAbilityNode});
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        IdleNode idleNode = new IdleNode(this);

        _rootNode = new Selector(new List<Node> { abilitySequence, attackSequence, movementSequence, idleNode });
    }
    
    public override void Move()
    {
        if (target == null || attacking)
            return;

        AudioManager.instance.PlaySound("MovementEnemy");

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);
        
        // Experimenting with different offsets for the movement vector to make enemy more interesting
        // Using the Sin function has resulted in some interesting behaviour with the enemy often cutting of players escape route
        /*
        float randomXOffset = Random.Range(1f, 2f);
        if (movementVector.x < 0)
            randomXOffset *= -1;
        float randomYOffset = Random.Range(1f, 2f);
        if (movementVector.y < 0)
            randomYOffset *= -1;
        movementVector += new Vector2(randomXOffset, randomYOffset);
        */
        //Vector2 offset = new Vector2(.5f, .5f);
        Vector2 offset = new Vector2(Mathf.Clamp01(Mathf.Sin(Time.time)), Mathf.Clamp01(Mathf.Sin(Time.time)));
        //Vector2 offset = new Vector2(Mathf.Sin(Time.time) + 1, Mathf.Sin(Time.time) + 1);
        movementVector += offset;

        rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
    }
    
    public override void Attack()
    {
        if (attacking)
            return;
        
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));
        AudioManager.instance.StopSound("MovementEnemy");
        
        _attackCounter++;

        StartCoroutine(nameof(MyAttackTell));
    }
    
    private IEnumerator MyAttackTell()
    {
        attacking = true;

        myAnimations.PlayAttackTell();

        yield return new WaitForSeconds(.5f);
        
        bool leftAttack = myHands.UseLeftHand();
        bool rightAttack = myHands.UseRightHand();

        // only pause if one of the items got used
        if (leftAttack || rightAttack)
        {
            float time = myHands.GetHighestCooldown() / 3f;
            time = Mathf.Clamp(time, 0.1f, 1f);
            yield return new WaitForSeconds(time);
        }

        attacking = false;
    }

    public override bool UseAbility()
    {
        if (attacking || _usingAbility)
            return false;

        _attackCounter = 0;
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine(nameof(MyAbilityTell));
        return true;
    }
    
    IEnumerator MyAbilityTell()
    {
        attacking = true;
        _usingAbility = true;

        myAnimations.PlayAbilityTell();

        yield return new WaitForSeconds(.5f);

        AudioManager.instance.PlayOneShotSound("UseShield");
        StaticSlowingTrap.Create(transform.position, new Vector3(1.5f, 1.5f, 1f), slowTrap, 15f, 0.5f);

        _usingAbility = false;
        attacking = false;
    }

    public override bool CheckBool(int whichBool = 0)
    {
        if (_attackCounter == 0)
            return false;
        
        return _attackCounter % howManyAttacksBeforeAbility == 0;
    }
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == 17)
        {
            speed += speed * collision.GetComponent<StaticSlowingTrap>().GetModifier();
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 17)
        {
            speed = myStats.movementSpeed;
        }
    }
}
