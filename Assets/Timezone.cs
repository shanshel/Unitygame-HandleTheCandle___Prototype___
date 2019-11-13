using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timezone : MonoBehaviour
{
    public float timeScale;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && !player.isTimeScaled())
        {
            player.setLocalTimeScale(timeScale);
      
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && player.isTimeScaled())
        {
            player.setLocalTimeScale(1f);
        }
    }
}
