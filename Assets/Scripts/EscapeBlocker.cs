using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EscapeBlocker : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] public Vector3 target;
    private void Update()
    {
        var playerPosition = Player.position;
        if (Mathf.Approximately(playerPosition.x, -152) && Mathf.Approximately(playerPosition.y, 90) &&
            Mathf.Approximately(playerPosition.z, -152))
        {
            playerPosition.x = target.x;
            playerPosition.y = target.y;
            playerPosition.z = target.z;
        }
    }
}

