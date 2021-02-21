using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChaseAndAttackEnemy : Enemy
{
    private Node _rootNode;

    protected override void Start()
    {
        base.Start();
        name = "SimpleChaseAndAttack";
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
        
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(myTransform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, myTransform, target);
        RangeNode searchRangeNode = new RangeNode(myTransform, target, myStats.attackRange * 20);

        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        IdleNode idleNode = new IdleNode(this);

        _rootNode = new Selector(new List<Node> { attackSequence, movementSequence, idleNode });
    }

    public override void Attack()
    {
        if (attacking)
            return;

        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));
        AudioManager.instance.StopSound("MovementEnemy");

        StartCoroutine("MyAttackTell");
    }

    public override void Move()
    {
        if (target == null)
            return;

        if (attacking)
            return;

        AudioManager.instance.PlaySound("MovementEnemy");

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.MovePosition(rb.position + movementVector.normalized * (myStats.movementSpeed * Time.fixedDeltaTime));
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
