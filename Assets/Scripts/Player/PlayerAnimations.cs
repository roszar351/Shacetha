using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _playerAnimator;
    private PlayerController _player;
    
    #region caching
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int VerticalMovement = Animator.StringToHash("VerticalMovement");
    private static readonly int HorizontalMovement = Animator.StringToHash("HorizontalMovement");
    private static readonly int VerticalLook = Animator.StringToHash("VerticalLook");
    private static readonly int HorizontalLook = Animator.StringToHash("HorizontalLook");
    #endregion

    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _player = GetComponentInParent<PlayerController>();
    }

    public void StopPlayerMovement(float forHowLong)
    {
        _player.StopMovement(forHowLong);
    }

    public void ResumePlayerMovement()
    {
        _player.ResumeMovement();
    }

    public void PlayMovementAnimation(Vector2 movement)
    {
        if(movement.x == 0 && movement.y == 0)
            _playerAnimator.SetBool(Moving, false);
        else
            _playerAnimator.SetBool(Moving, true);

        _playerAnimator.SetFloat(VerticalMovement, movement.y);
        _playerAnimator.SetFloat(HorizontalMovement, movement.x);
    }

    public void UpdateIdleAnimation(Vector2 lookVector)
    {
        _playerAnimator.SetFloat(VerticalLook, lookVector.y);
        _playerAnimator.SetFloat(HorizontalLook, lookVector.x);
    }
}
