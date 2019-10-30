using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player Options
    public float speed = 5f, dashPower = 5f;
    public bool flipX, flipY;
    public float torchTime;
    public TextMesh torchText;


    //Components
    Rigidbody2D _rigidBody;
    SpriteRenderer _sprite;

    //Flipping
    int xControlValue = 1;
    int yControlValue = 1;

    //TimeScale
    float localTimeScale;


    bool isCharacterDied = false;

    private void Awake()
    {
        initComponents();
    }

    void Start()
    {
        setLocalTimeScale();
        initCharacterFlipping();
        initStepSound();
        initTorchTimer();
    }

    void initComponents()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void setLocalTimeScale(float timeScale = 1f)
    {
        localTimeScale = timeScale;
    }

    void initCharacterFlipping()
    {
        float xRotate = 0f, yRotate = 0f;
        if (flipX)
        {
            xControlValue = -1;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            yRotate = 180f;
        }
        if (flipY)
        {
            yControlValue = -1;
            transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            xRotate = 180f;
        }
        torchText.gameObject.transform.Rotate(new Vector2(xRotate, yRotate));

    }

    void initStepSound()
    {
        InvokeRepeating("stepSfxCheck", 0.0f, 0.15f);
    }

    void initTorchTimer()
    {
        if (torchTime != 0f)
        {
            InvokeRepeating("torchTimerCheckLazy", 0f, 1f);
        }
        else
        {
            torchText.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    int lastDirection, lockedDirection;

    float dashTime = 0.2f, dashTimer = 0f;
    float dashCooldownTimer = 0f, dashCooldownTime = .8f;
    Vector2 lastMoveDir, lastPosition, velocity;

    float doorPushBackTimer, doorPushPower;
    
    void Update()
    {
     

        if (doorPushBackTimer > 0f || isCharacterDied)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space) && dashCooldownTimer <= 0f && localTimeScale == 1f)
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
            Vector2 disiredPosition = position + movement * speed * Time.deltaTime * localTimeScale;
            velocity = (_rigidBody.position - lastPosition) * 50;
           
            lastPosition = _rigidBody.position;
            _rigidBody.MovePosition(disiredPosition);

        }


        dashCooldownTimer -= Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        if (isCharacterDied) return;
        movementCheck();

    }

    bool isWalking = false;
    float runWalkSFXTime = .8f;
    float runWalkSFXTimer = 0f;





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

    public void onPlayerPassThrowDoor()
    {
        localTimeScale = 0.3f;
        AudioManager.singlton.playSFX(Enums.SFXEnum.playerPassDoor);
    }

    public void onPlayerLeaveDoor()
    {
        localTimeScale = 1f;
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



    //-------------------------- Torch ------------------------------
    void torchTimerCheckLazy()
    {
        if (torchTime > 0)
        {
            torchTime -= 1f;
            torchText.text = Mathf.CeilToInt(torchTime).ToString();
        }
        else
        {
            CancelInvoke("torchTimerCheckLazy");
            onPlayerTorchTimeEnd();
        }
    }

    void onPlayerTorchTimeEnd()
    {
        die();
    }


    void die()
    {
        _sprite.color = Color.black;
        isCharacterDied = true;
        AudioManager.singlton.playSFX(Enums.SFXEnum.characterDieSFX);
    }


    public Color getCharacterColor()
    {
        return _sprite.color;
    }
}
