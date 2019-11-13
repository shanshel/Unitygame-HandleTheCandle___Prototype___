using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeZone : MonoBehaviour
{
    public float scale, mass;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && !player.isScaled())
        {
            player.setScale(scale);
            player.setMass(mass);
            player.setDashPower(20f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && player.isScaled())
        {
            player.setScale();
            player.setMass();
            player.setDashPower();

        }
    }
}
