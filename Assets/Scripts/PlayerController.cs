using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f, dashPower = 5f;

    public bool flipX, flipY;
    int xControlValue = 1;
    int yControlValue = 1;
    Rigidbody2D _rigidBody;
    SpriteRenderer _sprite;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
       
        if (flipX)
        {
            xControlValue = -1;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        if (flipY)
        {
            yControlValue = -1;
            transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }

        InvokeRepeating("stepSfxCheck", 0.0f, 0.15f);
    }

    // Update is called once per frame
    int lastDirection, lockedDirection;

    float dashTime = 0.2f, dashTimer = 0f;
    float dashCooldownTimer = 0f, dashCooldownTime = .8f;
    Vector2 lastMoveDir, lastPosition, velocity;

    float doorPushBackTimer, doorPushPower;
    
    void Update()
    {
        if (doorPushBackTimer > 0f)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space) && dashCooldownTimer <= 0f)
        {
            dashTimer = dashTime;
            dashCooldownTimer = dashCooldownTime;
            AudioManager.singlton.playSFX(Enums.SFXEnum.dashSFX);
            Invoke("onPlayerDoorPushedEnd", dashTime + 0.1f);


        }
    }

    void movementCheck()
    {

        if (doorPushBackTimer > 0f)
        {
            _rigidBody.AddForce(doorPushTo * doorPushPower);
            doorPushBackTimer -= Time.fixedDeltaTime;
            return;
        }
        if (dashTimer > 0f)
        {
            _rigidBody.AddForce(lastMoveDir * dashPower, ForceMode2D.Impulse);
            dashTimer -= Time.deltaTime;
            /*
            
            Vector2 disiredPosition = _rigidBody.position + lastMoveDir * speed * Time.deltaTime * 4f;
            _rigidBody.MovePosition(disiredPosition);
            isWalking = false;
            */
        }
        else
        {
            float horz = Input.GetAxis("Horizontal");
            float vetical = Input.GetAxis("Vertical");


            horz *= xControlValue;
            vetical *= yControlValue;


            Vector2 movement = new Vector2(horz, vetical);
            lastMoveDir = movement;
            Vector2 position = _rigidBody.position;
            Vector2 disiredPosition = position + movement * speed * Time.deltaTime;
            velocity = (_rigidBody.position - lastPosition) * 50;
           
            lastPosition = _rigidBody.position;
            _rigidBody.MovePosition(disiredPosition);

        }


        dashCooldownTimer -= Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        movementCheck();
        audioUpdate();
    }

    bool isWalking = false;
    float runWalkSFXTime = .8f;
    float runWalkSFXTimer = 0f;
    void audioUpdate()
    {
 

     
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        lockedDirection = lastDirection;
    }

    Vector2 doorPushTo;
    public void onPlayerHitDoor(GameObject doorObject)
    {

        doorPushPower = 5f;
        float yValue = 1;
        float xValue = 1;

        if (doorObject.transform.position.y > transform.position.y)
        {
            yValue = -1;
        }
        if (doorObject.transform.position.x > transform.position.x)
        {
            xValue = -1;
        }
       
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.angularVelocity = 0f;

        doorPushTo = new Vector2(xValue, yValue);
        doorPushBackTimer = .7f;
        Invoke("onPlayerDoorPushedEnd", 0.7f);
        //StartCoroutine(AudioManager.singlton.playSFXAfter(Enums.SFXEnum.playerPushedBackByDoorSFX, 0.2f));

    }

    public void onPlayerDoorPushedEnd()
    {
       
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.angularVelocity = 0f;
        
  

    }



    void stepSfxCheck()
    {
        float horz = Input.GetAxis("Horizontal");
        float vetical = Input.GetAxis("Vertical");
        if (( !Mathf.Approximately(horz, 0f) || !Mathf.Approximately(vetical, 0f) ) &&
            Vector2.Distance(_rigidBody.position, lastPosition) > 0.05f)
        {
            var rand = Random.Range(0, 2);

            if (rand == 0)
                AudioManager.singlton.playSFX(Enums.SFXEnum.step1SFX);
            else
                AudioManager.singlton.playSFX(Enums.SFXEnum.step2SFX);
         
           
        }
    }



}
