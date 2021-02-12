using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackAndRunAwayEnemy : Enemy
{
    private Node rootNode;
    private bool runAway = false;

    protected override void Start()
    {
        base.Start();
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        RunAwayNode runAwayNode = new RunAwayNode(this, transform, target);
        RangeNode runAwayRangeNode = new RangeNode(transform, target, myStats.attackRange * 5);
        CheckBoolNode checkRunAwayNode = new CheckBoolNode(this);
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(transform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, transform, target);
        RangeNode searchRangeNode = new RangeNode(transform, target, myStats.attackRange * 20);
  
        IdleNode idleNode = new IdleNode(this);
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        Sequence runAwaySequence = new Sequence(new List<Node> { checkRunAwayNode, runAwayRangeNode, runAwayNode });

        rootNode = new Selector(new List<Node> { runAwaySequence, attackSequence, movementSequence, idleNode });
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

    public override void MoveAway()
    {
        //myHands.UseRightHand();

        Vector2 movementVector = transform.position - target.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.MovePosition(rb.position + movementVector.normalized * myStats.movementSpeed * Time.fixedDeltaTime);
    }

    public override bool CheckBool(int whichBool = 0)
    {
        return runAway;
    }

    IEnumerator MyAttackTell()
    {
        attacking = true;

        myAnimations.PlayAttackTell();

        yield return new WaitForSeconds(1f);

        myHands.UseLeftHand();
        myHands.UseRightHand();

        yield return new WaitForSeconds(.5f);

        runAway = true;
        Invoke("ResetRunAway", 2f);

        attacking = false;
    }

    private void ResetRunAway()
    {
        runAway = false;
    }
}
