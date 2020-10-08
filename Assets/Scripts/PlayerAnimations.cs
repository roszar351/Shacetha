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

    public void PlayLeftAttack()
    {
        playerAnimator.SetTrigger("LPunch");
    }

    public void PlayRightAttack()
    {
        playerAnimator.SetTrigger("RPunch");
    }
}
