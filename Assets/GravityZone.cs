using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public Vector2 gravity;
    float power = 50f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            collision.attachedRigidbody.AddRelativeForce(gravity * power);
        }
    }


}
