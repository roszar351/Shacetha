using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoss : Enemy
{
    [SerializeField]
    private float abilityCooldown = 10f;
    [SerializeField]
    private int dashDamage = 30;

    //TODO: Implement dashing boss, move some methods/logic into the base class
    private Node rootNode;
    private float currentAbilityCooldown = 0f;
    private bool usingAbility = false;

    protected override void Start()
    {
        base.Start();
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        currentAbilityCooldown -= Time.fixedDeltaTime;
        rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        AbilityNode abilityNode = new AbilityNode(this);
        RangeNode abilityRangeNode = new RangeNode(transform, target, myStats.attackRange * 4);
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(transform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, transform, target);
        RangeNode searchRangeNode = new RangeNode(transform, target, myStats.attackRange * 1000);

        IdleNode idleNode = new IdleNode(this);
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        Sequence abilitySequence = new Sequence(new List<Node> { abilityRangeNode, abilityNode });

        rootNode = new Selector(new List<Node> { abilitySequence, attackSequence, movementSequence, idleNode });
    }

    public override void Attack()
    {
        if (attacking)
            return;

        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine("MyAttackTell");
    }

    public override bool UseAbility()
    {
        if (currentAbilityCooldown > 0)
            return false;

        if (attacking)
            return false;

        currentAbilityCooldown = abilityCooldown;
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine("MyAbilityTell");
        return true;
    }

    public override void Move()
    {
        if (attacking)
            return;

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.MovePosition(rb.position + movementVector.normalized * myStats.movementSpeed * Time.fixedDeltaTime);
    }

    IEnumerator MyAttackTell()
    {
        attacking = true;

        myAnimations.PlayAttackTell();

        yield return new WaitForSeconds(1f);

        AudioManager.instance.PlayOneShotSound("ScaryGhost");

        myHands.UseLeftHand();
        myHands.UseRightHand();

        attacking = false;
    }

    IEnumerator MyAbilityTell()
    {
        attacking = true;

        myAnimations.PlayAbilityTell();

        yield return new WaitForSeconds(1f);

        usingAbility = true;

        AudioManager.instance.PlayOneShotSound("ScaryGhost");

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.AddForce(movementVector.normalized * myStats.movementSpeed * Time.fixedDeltaTime * 10000);

        yield return new WaitForSeconds(2f);

        rb.velocity = Vector3.zero;
        usingAbility = false;
        attacking = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (!usingAbility)
            return;

        if (collision.gameObject.layer == 8)
        {
            collision.GetComponent<PlayerController>().TakeDamage(dashDamage);
        }
    }
}
