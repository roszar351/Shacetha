using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagBat : Enemy
{
    /*
     * This ai should move towards the player in a zig zag manner(or some other random movement that is not a straight line towards the player)
     * https://docs.unity3d.com/ScriptReference/Mathf.PingPong.html can be used to alternate between ranges of values
     * This enemy will do damage by contact only, the 'tell' would be the movement pattern that the player could respond to
     * To achieve the movement pattern it will probably require to not update its target's position all the time but need to experiment to see
     * After trying out some different movement possibilities they might be used in other ais
     *
     * Final choice: after some experimentation rather than updating the players position every 'x' seconds it will update whenever player
     * uses one of their items, simulating the bat hearing the player and going towards them, once they reach this position they will move in a circle
     * until player uses an item again
     * 
     */
    [SerializeField] private float rangeMultiplier = 10f;
    //[SerializeField] private float aimTimer = 5f;
    [SerializeField] private float zigzagTimer = 2f;
    [SerializeField] private Transform myFollowTarget;
    [SerializeField] private KeepConstantPosition followTargetPosition;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform rightHandPosition;

    private Node _rootNode;
    private bool _aimAgain = false;
    private bool _goingForLeftHand = true;
    //private float _currentAimTimer = 0f;
    private float _currentZigzagTimer = 0f;
    private Vector3 _playersLastPosition;
    private float _angle;
    private bool _coroutineRunning = false;

    protected override void Start()
    {
        base.Start();
        name = "Bat";
        ConstructBehaviourTree();
        // initially start orbiting its spawn point, waiting for player to use an item
        _playersLastPosition = transform.position;
        followTargetPosition.UpdatePosition(_playersLastPosition);
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        //_currentAimTimer -= Time.fixedDeltaTime;
        _currentZigzagTimer -= Time.fixedDeltaTime;
        //if (_currentAimTimer <= 0)
        //{
        //    _aimAgain = true;
        //}

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
        ChaseNode chaseNode = new ChaseNode(this, myTransform, myFollowTarget, .2f);
        RangeNode searchRangeNode = new RangeNode(myTransform, myFollowTarget, myStats.attackRange * rangeMultiplier);
        RangeNode reachedTargetNode = new RangeNode(myTransform, myFollowTarget, myStats.attackRange * 1.05f);
        Inverter invertNode = new Inverter(reachedTargetNode);

        IdleNode idleNode = new IdleNode(this);
        Sequence movementSequence = new Sequence(new List<Node> { invertNode, searchRangeNode, chaseNode });
        Sequence aimSequence = new Sequence(new List<Node> { aimAgainNode, reaimAbilityNode });

        _rootNode = new Selector(new List<Node> { aimSequence, movementSequence, idleNode });
    }

    public override void Idle()
    {
        base.Idle();
        // move in circular motion around the players last position
        _angle += speed * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, _playersLastPosition) <= myStats.attackRange * 1.1f)
        {
            Vector3 offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0f) * myStats.attackRange;
            //transform.position = _playersLastPosition + offset;
            transform.position = Vector3.Lerp(transform.position, _playersLastPosition + offset,
                speed * Time.fixedDeltaTime);
        }

        // If reached the target position reduce wait time for re-aiming
        //_currentAimTimer -= Time.fixedDeltaTime;
    }

    public override void Move()
    {
        if (target == null)
            return;

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
    // Now only called after player uses an item
    public override bool UseAbility()
    {
        //_currentAimTimer = aimTimer;
        _aimAgain = false;
        //myFollowTarget.position = PlayerManager.instance.player.transform.position;
        // using an extra script that will keep the target stay at the position supplied
        // i.e. bat will got towards players last known position and will update once player uses an item
        _playersLastPosition = PlayerManager.instance.player.transform.position;
        followTargetPosition.UpdatePosition(_playersLastPosition);

        return true;
    }

    public override bool CheckBool(int whichBool = 0)
    {
        return _aimAgain;
    }

    public void AimAgain()
    {
        if (_coroutineRunning)
            return;
        
        StartCoroutine(nameof(AimDelay));
    }

    private IEnumerator AimDelay()
    {
        _coroutineRunning = true;
        myAnimations.PlayAbilityTell();
        
        yield return new WaitForSeconds(0.4f);

        _coroutineRunning = false;
        _aimAgain = true;
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
