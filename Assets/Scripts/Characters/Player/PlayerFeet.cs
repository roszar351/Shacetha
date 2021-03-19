using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for when player walks over traps(or other things that should only trigger on feet touching and not the whole character
// e.g. makes no sense for player to take damage when their head touches a poison puddle or spikes)
public class PlayerFeet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 17)
        {
            // should probably keep reference to player in this script rather than calling playermanager
            PlayerManager.instance.SlowPlayMovement(other.GetComponent<StaticSlowingTrap>().GetModifier());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // trap moves by a very small amount causing physics update which will keep triggering this method
        if (other.gameObject.layer == 16)
        {
            PlayerManager.instance.DealDamageToPlayer(other.GetComponent<StaticTrap>().GetDamage());
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 17)
        {
            PlayerManager.instance.ResetPlayerMovementValue();
        }
    }
}
