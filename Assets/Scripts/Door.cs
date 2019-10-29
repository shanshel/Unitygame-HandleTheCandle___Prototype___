using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerController[] playersCanPush;
    BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();

        for (int x = 0; x < playersCanPush.Length; x++)
        {
            Physics2D.IgnoreCollision(_boxCollider, playersCanPush[x].GetComponent<CapsuleCollider2D>());

        }
        
    }

    Vector2 pushTo;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.onPlayerHitDoor(gameObject);
            AudioManager.singlton.playSFX(Enums.SFXEnum.playerPassThrowDoorEarlySFX);
        }
    }

  
 
}
