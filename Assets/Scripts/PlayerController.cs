using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //Player Options
    public float speed = 5f, dashPower = 5f;
    public bool flipX, flipY;
    public float torchTime;
    public TextMesh torchText;
    public BoxCollider2D privateCollider;
    public bool isUnkownColor;

    //Components
    Rigidbody2D _rigidBody;
    SpriteRenderer _sprite;
    Animator _animator;

    //Flipping
    int xControlValue = 1;
    int yControlValue = 1;

    //TimeScale
    float localTimeScale;


    bool isCharacterDied = false;


    int lastDirection, lockedDirection;

    float dashTime = 0.2f, dashTimer = 0f;
    float dashCooldownTimer = 0f, dashCooldownTime = .8f;
    Vector2 lastMoveDir, lastPosition, velocity;

    float doorPushBackTimer, doorPushPower;


    //Bases
    Vector3 baseScale;
    float baseMass, baseDashPower;
    Color baseColor;
    

    //UnkownColors
    Color[] unkownColors = new Color[] { Color.blue, Color.red, Color.green, Color.yellow, Color.white, Color.cyan };
    Color nextColor;
    float colorTimer;

    private void Awake()
    {
        initComponents();
        instance = this;
        
    }

    void Start()
    {
      
        setLocalTimeScale();
        initCharacterFlipping();
        initStepSound();
        initTorchTimer();
        initBases();
        initUnkownColors();

    
       
    }

    void initComponents()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void setLocalTimeScale(float timeScale = 1f)
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

    void initBases()
    {
        baseScale = transform.localScale;
        baseMass = _rigidBody.mass;
        baseColor = _sprite.color;
        baseDashPower = dashPower;

    }
    void initUnkownColors()
    {
        if (isUnkownColor)
        {
            nextColor = Color.blue;
            _sprite.color = Color.white;

        }
    }

    // Update is called once per frame

    
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

            if (lookDirection.x < 0)
            {
                _animator.SetTrigger("IsFlippedDash");

            }
            else
            {
                _animator.SetTrigger("IsDash");

            }

        }
    }




    Vector2 lookDirection = new Vector2(0, 0);
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
            _rigidBody.AddForce(lastMoveDir * dashPower * _rigidBody.mass, ForceMode2D.Impulse);
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


            if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
            {
                lookDirection.Set(movement.x, movement.y);
                lookDirection.Normalize();
                _animator.SetFloat("Look X", lookDirection.x);
                _animator.SetFloat("Look Y", lookDirection.y);
            }

            Vector2 position = _rigidBody.position;
            Vector2 disiredPosition = position + movement * speed * Time.deltaTime * localTimeScale;
            velocity = (_rigidBody.position - lastPosition) * 50;
           
            lastPosition = _rigidBody.position;
            _rigidBody.MovePosition(disiredPosition);

         
            _animator.SetFloat("Speed", movement.magnitude);

        }

       

        dashCooldownTimer -= Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {


        if (torchTime > 0f)
            torchTime -= Time.deltaTime * localTimeScale;
        if (isCharacterDied) return;
        colorCheck();


        movementCheck();

    }

    void colorCheck()
    {
        if (isUnkownColor)
        {
            _sprite.color = Color.Lerp(_sprite.color, nextColor, 3f * Time.deltaTime);


            colorTimer -= Time.deltaTime;
            if (colorTimer <= 0f)
            {
                colorTimer = .8f;
                nextColor = unkownColors[Random.Range(0, unkownColors.Length)];
            }

        }
        
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

    public void onPlayerPassThrowDoor(bool isPushBack = false)
    {
        if (isPushBack)
        {
            privateCollider.enabled = true;
            return;
        }

        localTimeScale = 0.3f;
        AudioManager.singlton.playSFX(Enums.SFXEnum.playerPassDoor);
    }

    public void onPlayerLeaveDoor()
    {
        localTimeScale = 1f;
        privateCollider.enabled = false;
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
        torchText.text = Mathf.CeilToInt(torchTime).ToString();

        if (torchTime <= 0f)
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

    public bool isDead()
    {
        return isCharacterDied;
    }

    public void revive()
    {
        _sprite.color = baseColor;
        isCharacterDied = false;
        torchText.text = "";
    }


    public Color getCharacterColor()
    {
        return _sprite.color;
    }



    public void setScale(float scale = 1f)
    {
        if (scale == 1f)
        {
            transform.localScale = baseScale;
        }
        else
        {
            transform.localScale = baseScale * scale;
        }
    }

    public bool isScaled()
    {
        return baseScale != transform.localScale;
    }


    public void setMass(float mass = 0f)
    {
        if (mass == 0f)
        {
            _rigidBody.mass = baseMass;
            return;
        }

        _rigidBody.mass = mass;
    }
   
    public bool isTimeScaled()
    {
        return localTimeScale != 1f;
    }

    public void setDashPower(float power = 0f)
    {
        if (power == 0f)
        {
            dashPower = baseDashPower;
            return;
        }
        dashPower = power;
    }

}
