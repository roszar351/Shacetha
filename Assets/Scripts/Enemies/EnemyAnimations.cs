using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _myAnimator;
    private Enemy _enemy;
    
    #region caching
    // caching animator variables to improve performance
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int VerticalMovement = Animator.StringToHash("VerticalMovement");
    private static readonly int HorizontalMovement = Animator.StringToHash("HorizontalMovement");
    private static readonly int VerticalLook = Animator.StringToHash("VerticalLook");
    private static readonly int HorizontalLook = Animator.StringToHash("HorizontalLook");
    private static readonly int AttackTell = Animator.StringToHash("AttackTell");
    private static readonly int SpellTell = Animator.StringToHash("SpellTell");
    private static readonly int Invincibility = Animator.StringToHash("Invincibility");

    #endregion 

    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _enemy = GetComponentInParent<Enemy>();
    }

    public void PlayMovementAnimation(Vector2 movement)
    {
        if (movement.x == 0 && movement.y == 0)
            _myAnimator.SetBool(Moving, false);
        else
            _myAnimator.SetBool(Moving, true);

        _myAnimator.SetFloat(VerticalMovement, movement.y);
        _myAnimator.SetFloat(HorizontalMovement, movement.x);
    }

    public void UpdateIdleAnimation(Vector2 lookVector)
    {
        if (_myAnimator == null)
            return;
        
        _myAnimator.SetFloat(VerticalLook, lookVector.y);
        _myAnimator.SetFloat(HorizontalLook, lookVector.x);
    }

    public void UpdateInvincibilityBool(bool isInvincible)
    {
        _myAnimator.SetBool(Invincibility, isInvincible);
    }

    public void PlayAttackTell()
    {
        _myAnimator.SetBool(Moving, false);
        _myAnimator.SetTrigger(AttackTell);
    }

    public void PlayAbilityTell()
    {
        _myAnimator.SetBool(Moving, false);
        _myAnimator.SetTrigger(SpellTell);
    }
}
