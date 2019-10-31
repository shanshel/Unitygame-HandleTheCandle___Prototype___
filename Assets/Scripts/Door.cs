using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerController[] playersCanPush;
    public BoxCollider2D _innerCollider, outterCollider;
    public SpriteRenderer _sprite, arrowOne, arrowTwo;

    public bool disableSideOne, disableSideTwo;
    public ParticleSystem leaveUpVFX, leaveDownVFX;
    Color mainColor;

    Vector2 pushTo;
    
    private void Start()
    {
        setupColors();
        setupColission();
    }

    void setupColors()
    {
        if (playersCanPush.Length == 1)
        {
            mainColor = playersCanPush[0].getCharacterColor();
        }


        _sprite.color = mainColor;
        if (arrowOne != null && arrowTwo != null)
        {
            float h, s, v;
            Color.RGBToHSV(mainColor, out h, out s, out v);
            s = .65f;
            arrowOne.color = Color.HSVToRGB(h, s, v);
            arrowTwo.color = Color.HSVToRGB(h, s, v);
            if (disableSideOne)
                arrowOne.color = Color.black;
            if (disableSideTwo)
                arrowTwo.color = Color.black;
        }

        var ps = leaveUpVFX.main;
        var ps2 = leaveDownVFX.main;
        ps.startColor = _sprite.color;
        ps2.startColor = _sprite.color;
    }

    void setupColission()
    {
        for (int x = 0; x < playersCanPush.Length; x++)
        {
            Physics2D.IgnoreCollision(outterCollider, playersCanPush[x].GetComponent<CapsuleCollider2D>());
        }
    }

   

    

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
            var playerDirection = transform.InverseTransformPoint(player.transform.position);
            bool isGoingUp = false, pushBack = false;

            if (playerDirection.y < -.2f)
                isGoingUp = true;

            if (isGoingUp && disableSideOne)
            {
                pushBack = true;
            }
            else if (!isGoingUp && disableSideTwo)
            {
                pushBack = false;
            }

   

            player.onPlayerPassThrowDoor(pushBack);
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
