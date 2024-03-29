﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoss : Enemy
{
    [SerializeField] private float abilityCooldown = 10f;
    [SerializeField] private int dashDamage = 30;

    //TODO: move some methods/logic into the base class if time allows
    private Node _rootNode;
    private float _currentAbilityCooldown = 0f;
    private bool _usingAbility = false;

    protected override void Start()
    {
        base.Start();
        name = "DashBoss";
        ConstructBehaviourTree();
    }

    private void FixedUpdate()
    {
        if (isDying)
            return;

        _currentAbilityCooldown -= Time.fixedDeltaTime;
        _rootNode.Execute();
    }

    private void ConstructBehaviourTree()
    {
        Transform myTransform = transform;
        
        AbilityNode abilityNode = new AbilityNode(this);
        RangeNode abilityRangeNode = new RangeNode(myTransform, target, myStats.attackRange * 4);
        AttackNode attackNode = new AttackNode(this);
        RangeNode attackRangeNode = new RangeNode(myTransform, target, myStats.attackRange);
        ChaseNode chaseNode = new ChaseNode(this, myTransform, target);
        RangeNode searchRangeNode = new RangeNode(myTransform, target, myStats.attackRange * 1000);

        IdleNode idleNode = new IdleNode(this);
        Sequence movementSequence = new Sequence(new List<Node> { searchRangeNode, chaseNode });
        Sequence attackSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });
        Sequence abilitySequence = new Sequence(new List<Node> { abilityRangeNode, abilityNode });

        _rootNode = new Selector(new List<Node> { abilitySequence, attackSequence, movementSequence, idleNode });
    }

    public override void Attack()
    {
        if (attacking)
            return;

        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine(nameof(MyAttackTell));
    }

    public override bool UseAbility()
    {
        if (target == null)
            return false;

        if (_currentAbilityCooldown > 0)
            return false;

        if (attacking)
            return false;

        _currentAbilityCooldown = abilityCooldown;
        myAnimations.PlayMovementAnimation(new Vector2(0f, 0f));

        StartCoroutine(nameof(MyAbilityTell));
        return true;
    }

    public override void Move()
    {
        if (target == null)
            return;

        if (attacking)
            return;

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.MovePosition(rb.position + movementVector.normalized * (speed * Time.fixedDeltaTime));
    }

    private IEnumerator MyAttackTell()
    {
        attacking = true;

        myAnimations.PlayAttackTell();

        yield return new WaitForSeconds(1f);

        AudioManager.instance.PlayOneShotSound("ScaryGhost");

        bool leftAttack = myHands.UseLeftHand();
        bool rightAttack = myHands.UseRightHand();

        // only pause if one of the items got used
        if(leftAttack || rightAttack)
        {
            float time = myHands.GetHighestCooldown() / 3f;
            time = Mathf.Clamp(time, 0.35f, 5f);
            yield return new WaitForSeconds(time);
        }

        attacking = false;
    }

    private IEnumerator MyAbilityTell()
    {
        attacking = true;

        myAnimations.PlayAbilityTell();

        yield return new WaitForSeconds(1f);

        _usingAbility = true;

        AudioManager.instance.PlayOneShotSound("ScaryGhost");

        Vector2 movementVector = target.position - transform.position;

        myAnimations.PlayMovementAnimation(movementVector);

        rb.AddForce(movementVector.normalized * (speed * Time.fixedDeltaTime * 10000));

        yield return new WaitForSeconds(2f);

        rb.velocity = Vector3.zero;
        _usingAbility = false;
        attacking = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (!_usingAbility)
            return;

        if (collision.gameObject.layer == 8)
        {
            collision.GetComponent<PlayerController>().TakeDamage(dashDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_usingAbility)
            return;

        if (other.gameObject.layer == 8)
        {
            other.GetComponent<PlayerController>().TakeDamage(dashDamage);
        }
    }
}
