using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{

    public PlayerController _character;
    BoxCollider2D _boxCollider;
    public GameObject torch;

    SpriteRenderer _sprite;
    Color __characterColorCache;

    public SpriteRenderer circle;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        __characterColorCache = _character.getCharacterColor();


        _sprite.color = __characterColorCache;
        circle.color = __characterColorCache;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController _hitPlayer = collision.gameObject.GetComponent<PlayerController>();

        if (_hitPlayer != null)
        {
            if (_hitPlayer.gameObject.name == _character.gameObject.name)
            {
                _boxCollider.enabled = false;
                Destroy(_hitPlayer.gameObject);
                GameManager.singelton.playerReachGoalZone(_hitPlayer);
                AudioManager.singlton.playSFX(Enums.SFXEnum.playerReachGoolSFX);
                torch.SetActive(true);

            }
        }
    }
   
  
}
