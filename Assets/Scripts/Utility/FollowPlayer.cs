using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 offSet = new Vector3(3f, -1.5f, 0f);
    private void LateUpdate()
    {
        transform.position = PlayerManager.instance.player.transform.position - offSet;
    }
}
