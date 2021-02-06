using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoss : Enemy
{
    //TODO: Implement dashing boss, move some methods/logic into the base class
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
        base.Attack();
    }

    public override bool UseAbility()
    {
        return base.UseAbility();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
    }

    public override void Idle()
    {
        base.Idle();
    }
}
