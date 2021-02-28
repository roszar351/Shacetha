using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingGoblin : Enemy
{
    /* TODO: Implement
     * This enemy should attack the player if they come in range but the main behaviour will be
     * running away, throwing some projectile(e.g. rock) at the player, stopping for small amount of time and repeat
     * Running away can be done same as the attack and runaway enemy but experiment making it a bit more random or use of the
     * movement patterns tested in the 'ZigzagBat'.
     *
     *
     */
    
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
