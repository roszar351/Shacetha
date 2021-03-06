using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingGoblin : Enemy
{
    /* 
     * This enemy should attack the player if they come in range but the main behaviour will be
     * running away, throwing some projectile(e.g. rock) at the player, stopping for small amount of time and repeat
     * Running away can be done same as the attack and runaway enemy but experiment making it a bit more random or use of the
     * movement patterns tested in the 'ZigzagBat'.
     *
     */
    [SerializeField] private float rangeMultiplier = 10f;
    [SerializeField] private float throwCooldown = 2f;
    [SerializeField] private Transform myTarget;
    [SerializeField] private GameObject projectilePrefab;
    
    private Node _rootNode;
    private bool _usingAbility = false;
    private float _currentThrowCooldown = 0f;
    
    protected override void Start()
    {
        base.Start();
        name = "Goblin";
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        _currentThrowCooldown -= Time.fixedDeltaTime;

        _rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        Transform myTransform = transform;

        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(myTransform, target, myStats.attackRange);
        CheckBoolNode abilityCheckNode = new CheckBoolNode(this);
        AbilityNode throwRockAbilityNode = new AbilityNode(this);
        // using run away node as dont want to change target when running away i.e. want the enemy to always be aiming at the player
        RangeNode searchRangeNode = new RangeNode(myTransform, target, myStats.attackRange * rangeMultiplier);
        RunAwayNode runAwayNode = new RunAwayNode(this);

        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        Sequence abilitySequence = new Sequence(new List<Node> { abilityCheckNode, throwRockAbilityNode});
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, runAwayNode });
        IdleNode idleNode = new IdleNode(this);

        _rootNode = new Selector(new List<Node> { attackSequence, abilitySequence, movementSequence, idleNode });
    }

    public override void MoveAway()
    {
        if (target == null)
            return;
        
        if (attacking)
            return;

        //myHands.UseRightHand();

        AudioManager.instance.PlaySound("MovementEnemy");

        Vector2 movementVector = myTarget.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
    }
    
    public override void Attack()
    {
        if (attacking)
            return;
        
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));
        AudioManager.instance.StopSound("MovementEnemy");

        StartCoroutine(nameof(MyAttackTell));
    }
    
    IEnumerator MyAttackTell()
    {
        attacking = true;

        myAnimations.PlayAttackTell();

        yield return new WaitForSeconds(1.5f);

        bool leftAttack = myHands.UseLeftHand();
        bool rightAttack = myHands.UseRightHand();

        // only pause if one of the items got used
        if(leftAttack || rightAttack)
            yield return new WaitForSeconds(myHands.GetHighestCooldown() / 3f);

        attacking = false;
    }

    public override bool UseAbility()
    {
        if (attacking || _usingAbility)
            return false;
        
        Vector2 movementVector = target.position - transform.position;
        myTarget.position = (Vector2)transform.position - movementVector.normalized;
        
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine(nameof(MyAbilityTell));
        return true;
    }
    
    IEnumerator MyAbilityTell()
    {
        attacking = true;
        _usingAbility = true;

        myAnimations.PlayAbilityTell();
        Vector3 rockTarget = PlayerManager.instance.player.transform.position;
        Vector3 offset = rockTarget - transform.position;

        yield return new WaitForSeconds(1f);
        
        Projectile.Create(transform.position + (offset.normalized * .5f), rockTarget, projectilePrefab, 1, 1, 1);
        _currentThrowCooldown = throwCooldown;
        AudioManager.instance.PlayOneShotSound("Swing3");

        _usingAbility = false;
        attacking = false;
    }

    public override bool CheckBool(int whichBool = 0)
    {
        return !_usingAbility && _currentThrowCooldown <= 0;
    }
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == 17)
        {
            speed *= collision.GetComponent<StaticSlowingTrap>().GetModifier();
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
