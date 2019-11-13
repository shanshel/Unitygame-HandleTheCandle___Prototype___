using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && player.isDead())
        {
            player.revive();
        }
    }
}
