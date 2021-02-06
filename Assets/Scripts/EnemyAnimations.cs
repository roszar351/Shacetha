using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator myAnimator;
    private Enemy enemy;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();
    }

    public void PlayMovementAnimation(Vector2 movement)
    {
        if (movement.x == 0 && movement.y == 0)
            myAnimator.SetBool("Moving", false);
        else
            myAnimator.SetBool("Moving", true);

        myAnimator.SetFloat("VerticalMovement", movement.y);
        myAnimator.SetFloat("HorizontalMovement", movement.x);
    }

    public void UpdateIdleAnimation(Vector2 lookVector)
    {
        myAnimator.SetFloat("VerticalLook", lookVector.y);
        myAnimator.SetFloat("HorizontalLook", lookVector.x);
    }

    public void PlayAttackTell()
    {
        myAnimator.SetBool("Moving", false);
        myAnimator.SetTrigger("AttackTell");
    }

    public void PlayLeftAttack()
    {
        myAnimator.SetBool("Moving", false);
        myAnimator.SetTrigger("LPunch");
    }

    public void PlayRightAttack()
    {
        myAnimator.SetBool("Moving", false);
        myAnimator.SetTrigger("RPunch");
    }
}
