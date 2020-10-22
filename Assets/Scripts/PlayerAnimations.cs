using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerController player;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerController>();
    }

    public void StopPlayerMovement(float forHowLong)
    {
        player.StopMovement(forHowLong);
    }

    public void ResumePlayerMovement()
    {
        player.ResumeMovement();
    }

    public void PlayMovementAnimation(Vector2 movement)
    {
        if(movement.x == 0 && movement.y == 0)
            playerAnimator.SetBool("Moving", false);
        else
            playerAnimator.SetBool("Moving", true);

        playerAnimator.SetFloat("VerticalMovement", movement.y);
        playerAnimator.SetFloat("HorizontalMovement", movement.x);
    }

    public void PlayLeftAttack()
    {
        playerAnimator.SetBool("Moving", false);
        playerAnimator.SetTrigger("LPunch");
    }

    public void PlayRightAttack()
    {
        playerAnimator.SetBool("Moving", false);
        playerAnimator.SetTrigger("RPunch");
    }
}
