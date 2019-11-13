using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPoint : MonoBehaviour
{


    [Header("Level Info")]
    public int levelIndex;
    public float min3StarsTime = 30f, min2StarsTime = 50f;
    [Header("Colliders")]
    public BoxCollider2D top;
    public BoxCollider2D topRight, topLeft, bottom, bottomRight, bottomLeft;

    [Header("Sprite Pieces")]
    public GameObject onlyUnlocked;
    public GameObject lockedOnly, openDoor, closedDoor;
    public SpriteRenderer _sprite, torchOne, torchTwo, torchThree, framePicOne, framePictwo, framePicThree;
    public TextMesh levelTitle;

    bool isLocked = true, isPassed = false;

    LevelPoint prevousLevel, nextLevel;

    int torchNumbers;
    private void Awake()
    {

    }
    
    private void Start()
    {
        LevelManager.singleton.registerLevel(this);

        torchLightup();
        initAcceablity();
        levelTitle.text = "Level " + (levelIndex + 1);
    }

   
    void torchLightup()
    {
        float levelTime = PlayerPrefs.GetFloat("LevelTime_" + levelIndex,  -1);
        if (levelTime > 0)
        {
            torchNumbers = 1;

            if (levelTime <= min3StarsTime)
            {
                torchNumbers = 3;
            } else if (levelTime <= min2StarsTime)
            {
                torchNumbers = 2;
            }
        }
    }

    private void initAcceablity()
    {
        if (torchNumbers > 0)
        {
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
        }
        else
        {
            openDoor.SetActive(false);
            closedDoor.SetActive(true);
        }

        torchOne.color = Color.black;
        torchTwo.color = Color.black;
        torchThree.color = Color.black;

        if (torchNumbers >= 1)
        {
            torchOne.color = Color.white;
            framePicOne.color = Color.white;
        }
        if (torchNumbers >= 2)
        {
            torchTwo.color = Color.white;
            framePictwo.color = Color.white;
        }
        if (torchNumbers >= 3)
        {
            torchThree.color = Color.white;
            framePicThree.color = Color.white;
        }

       
        if (levelIndex <= LevelManager.singleton.getPlayerReachedLevel())
        {
            isLocked = false;
            onlyUnlocked.SetActive(true);
            lockedOnly.SetActive(false);
            

            _sprite.color = Color.white;
        }
        else
        {
            isLocked = true;
            onlyUnlocked.SetActive(false);
            lockedOnly.SetActive(true);
            openDoor.SetActive(false);

        }



    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log(collision.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform playerTransform = PlayerController.instance.transform;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(playerTransform.position.x, playerTransform.position.y + .35f), 1.3f, 1 << LayerMask.NameToLayer("Ground"));

        if (hitColliders.Length == 0)
        {
            return;
        }
        GameObject defaultGameObject = hitColliders[0].attachedRigidbody.gameObject;

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (!GameObject.ReferenceEquals(defaultGameObject, hitColliders[i].attachedRigidbody.gameObject))
            {
                return;
            }
            i++;
        }
        LevelPoint nextRoom = this;
        LevelPoint currentRoom = defaultGameObject.GetComponent<LevelPoint>();
        Debug.Log("all of circle in one room");

        if (!isLocked && nextRoom.levelIndex != currentRoom.levelIndex)
        {
            LevelManager.singleton.disableCollisionBetweenLevels(collision.otherCollider.name, currentRoom, nextRoom);

        }
        return;



    }

}
