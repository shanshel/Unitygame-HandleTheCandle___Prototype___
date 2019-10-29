using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{

    public PlayerController[] playersCanPush;
    Rigidbody2D _rigidBody;
    PlayerController currentPlayerInTouch;
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
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
                        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                        //_rigidBody.mass = 10f;
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
                Invoke("poolFreeze", 1f);
                
            }
        }

    }

    void poolFreeze()
    {
        Debug.Log("freezed");
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }



}
