using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerController[] playersCanPush;
    public BoxCollider2D _innerCollider, outterCollider;
    SpriteRenderer _sprite;

    public ParticleSystem leaveUpVFX, leaveDownVFX;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();

        if (playersCanPush.Length == 1)
        {
            _sprite.color = playersCanPush[0].getCharacterColor();
        }
        
        var ps = leaveUpVFX.main;
        var ps2 = leaveDownVFX.main;
        ps.startColor = _sprite.color;
        ps2.startColor = _sprite.color;
        for (int x = 0; x < playersCanPush.Length; x++)
        {
            Physics2D.IgnoreCollision(outterCollider, playersCanPush[x].GetComponent<CapsuleCollider2D>());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.onPlayerPassThrowDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {

   

            player.onPlayerLeaveDoor();
            if (transform.position.y > player.transform.position.y)
            {
                leaveDownVFX.Play();
            }
            else
            {
                leaveUpVFX.Play();
            }
  
        

            
        }
    }


}
