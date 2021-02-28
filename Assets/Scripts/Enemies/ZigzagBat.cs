using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagBat : Enemy
{
    /* TODO: Implement
     * This ai should move towards the player in a zig zag manner(or some other random movement that is not a straight line towards the player)
     * https://docs.unity3d.com/ScriptReference/Mathf.PingPong.html can be used to alternate between ranges of values
     * This enemy will do damage by contact only, the 'tell' would be the movement pattern that the player could respond to
     * To achieve the movement pattern it will probably require to not update its target's position all the time but need to experiment to see
     * After trying out some different movement possibilities they might be used in other ais
     * 
     * 
     */
    [SerializeField] private float rangeMultiplier = 10f;
    [SerializeField] private float aimTimer = 5f;
    [SerializeField] private float zigzagTimer = 2f;
    [SerializeField] private Transform myFollowTarget;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform rightHandPosition;

    private Node _rootNode;
    private bool _aimAgain = true;
    private bool _goingForLeftHand = true;
    private float _currentAimTimer = 0f;
    private float _currentZigzagTimer = 0f;

    protected override void Start()
    {
        base.Start();
        name = "Bat";
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        _currentAimTimer -= Time.fixedDeltaTime;
        _currentZigzagTimer -= Time.fixedDeltaTime;
        if (_currentAimTimer <= 0)
        {
            _aimAgain = true;
        }

        if (_currentZigzagTimer <= 0)
        {
            _goingForLeftHand = !_goingForLeftHand;
            _currentZigzagTimer = zigzagTimer;
        }
        
        _rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        Transform myTransform = transform;
        
        CheckBoolNode aimAgainNode = new CheckBoolNode(this);
        AbilityNode reaimAbilityNode = new AbilityNode(this);
        ChaseNode chaseNode = new ChaseNode(this, myTransform, myFollowTarget, 0.2f);
        RangeNode searchRangeNode = new RangeNode(myTransform, myFollowTarget, myStats.attackRange * rangeMultiplier);
  
        IdleNode idleNode = new IdleNode(this);
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence aimSequence = new Sequence(new List<Node> { aimAgainNode, reaimAbilityNode });

        _rootNode = new Selector(new List<Node> { aimSequence, movementSequence, idleNode });
    }

    public override void Idle()
    {
        base.Idle();
        AudioManager.instance.StopSound("MovementEnemy");
        // If reached the target position reduce wait time for re-aiming
        _currentAimTimer -= Time.fixedDeltaTime;
    }

    public override void Move()
    {
        if (target == null)
            return;

        AudioManager.instance.PlaySound("MovementEnemy");

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        //movementVector = movementVector.normalized;
        // the two hands can be used to create a zigzag/wave motion as the bat moves towards the target
        if (_goingForLeftHand)
        {
            movementVector += (Vector2)(leftHandPosition.position - transform.position);
            //movementVector += (Vector2)(leftHandPosition.position);
            
        }
        else
        {
            movementVector += (Vector2)(rightHandPosition.position - transform.position);
            //movementVector += (Vector2)(rightHandPosition.position);
            
        }

        rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
    }
    
    // Ability used to re-aim the target (i.e. using bats sonar to find the player)
    public override bool UseAbility()
    {
        _currentAimTimer = aimTimer;
        _aimAgain = false;
        myFollowTarget.position = PlayerManager.instance.player.transform.position;
        
        return true;
    }

    public override bool CheckBool(int whichBool = 0)
    {
        return _aimAgain;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == 8)
        {
            collision.GetComponent<PlayerController>().TakeDamage(myStats.baseDamage);
        }
    }

    // prevent players from moving with the bat to prevent future damage
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<PlayerController>().TakeDamage(myStats.baseDamage);
        }
    }
}
