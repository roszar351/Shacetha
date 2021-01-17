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

        rootNode = new Selector(new List<Node> { attackSequence, movementSequence });
    }

    public override void Attack()
    {
        // TODO: Add a tell before attacking

        myHands.UseLeftHand();
        myHands.UseRightHand();
    }

    public override void Move()
    {
        Vector2 movementVector = target.position - transform.position;
        rb.MovePosition(rb.position + movementVector.normalized * myStats.movementSpeed * Time.fixedDeltaTime);
    }
}
