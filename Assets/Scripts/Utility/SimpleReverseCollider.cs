using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleReverseCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 14)
        {
            transform.position *= -1;
        }
    }
}
