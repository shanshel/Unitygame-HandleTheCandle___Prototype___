using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{

    public PlayerController[] playersCanPush;
    Rigidbody2D _rigidBody;
    PlayerController currentPlayerInTouch;

    public SpriteRenderer _sprite;
    public ParticleSystem moveVFX;
    Color mainColor;

    SpriteRenderer[] _sprites;

    private void Start()
    {

        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        if (_sprite != null)
        {
        }
        else
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }

        setupColors();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

    
    }

    void setupColors()
    {
        if (playersCanPush.Length == 1)
        {
            mainColor = playersCanPush[0].getCharacterColor();
        }
        else
        {
            mainColor = _sprite.color;
        }


        var ps = moveVFX.main;

        if (_sprites != null && _sprites.Length > 0)
        {
            Debug.Log("length: " + _sprites.Length);
            for (int x = 0; x < _sprites.Length; x++)
            {
                _sprites[x].color = mainColor;
            }
        }
        else
        {
            _sprite.color = mainColor;

        }
        ps.startColor = mainColor;

     
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
       
        if (player != null)
        {
            CancelInvoke("poolFreeze");
            if (playersCanPush.Length == 0)
            {
                _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                for (int x = 0; x < playersCanPush.Length; x++)
                {
                    if (collision.collider.gameObject.name == playersCanPush[x].gameObject.name)
                    {
                        currentPlayerInTouch = player;
                        player.setDashPower(40f);
                        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                }
            }
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null && currentPlayerInTouch != null)
        {
            
            if (player.gameObject.name == currentPlayerInTouch.gameObject.name)
            {
                player.setDashPower();

                Invoke("poolFreeze", 1f);
                
            }
        }

    }

    void poolFreeze()
    {
     
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }



}
