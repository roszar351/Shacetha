using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackAndRunAwayEnemy : Enemy
{
    private Node _rootNode;
    private bool _runAway = false;

    [SerializeField]
    private Transform myTarget;

    [SerializeField] private float rangeMultiplier = 20f;

    protected override void Start()
    {
        base.Start();
        name = "SimpleAttackAndRun";
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
        
        // using run away node as dont want to change target when running away i.e. want the enemy to always be aiming at the player
        RunAwayNode runAwayNode = new RunAwayNode(this);
        CheckBoolNode checkRunAwayNode = new CheckBoolNode(this);
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(myTransform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, myTransform, target);
        RangeNode searchRangeNode = new RangeNode(myTransform, target, myStats.attackRange * rangeMultiplier);
  
        IdleNode idleNode = new IdleNode(this);
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        Sequence runAwaySequence = new Sequence(new List<Node> { checkRunAwayNode, runAwayNode });

        _rootNode = new Selector(new List<Node> { runAwaySequence, attackSequence, movementSequence, idleNode });
    }

    public override void Attack()
    {
        if (attacking)
            return;

        AudioManager.instance.StopSound("MovementEnemy");
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine(nameof(MyAttackTell));
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

        rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
    }

    public override void MoveAway()
    {
        if (target == null)
            return;
        
        if (attacking)
            return;

        Vector2 movementVector = target.position - transform.position;
        myTarget.position = movementVector.normalized * -4;

        //myHands.UseRightHand();

        AudioManager.instance.PlaySound("MovementEnemy");

        movementVector = myTarget.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
    }

    public override bool CheckBool(int whichBool = 0)
    {
        return _runAway;
    }

    IEnumerator MyAttackTell()
    {
        attacking = true;

        myAnimations.PlayAttackTell();

        yield return new WaitForSeconds(1f);

        myHands.UseLeftHand();
        myHands.UseRightHand();

        yield return new WaitForSeconds(myHands.GetHighestCooldown() / 3f);

        _runAway = true;
        Invoke(nameof(ResetRunAway), 1f);

        attacking = false;
    }

    private void ResetRunAway()
    {
        _runAway = false;
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
