using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChaseAndAttackEnemy : Enemy
{
    private Node rootNode;

    protected override void Start()
    {
        base.Start();
        ConstructBehaviourTree();
    }

    private void Update()
    {
        rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(transform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, transform, target);
        RangeNode searchRangeNode = new RangeNode(transform, target, myStats.attackRange * 5);

        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        IdleNode idleNode = new IdleNode(this);

        rootNode = new Selector(new List<Node> { attackSequence, movementSequence, idleNode });
    }

    public override void Attack()
    {
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        if (attacking)
            return;

        StartCoroutine("MyAttackTell");
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

        myHands.UseLeftHand();
        myHands.UseRightHand();

        attacking = false;
    }
}
