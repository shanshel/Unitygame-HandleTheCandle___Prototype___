using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPoint : MonoBehaviour
{


    [Header("Level Info")]
    public int levelIndex;

    [Header("Colliders")]
    public BoxCollider2D top;
    public BoxCollider2D topRight, topLeft, bottom, bottomRight, bottomLeft;

    [Header("Sprite Pieces")]
    public GameObject onlyUnlocked;
    public GameObject lockedOnly, openDoor, closedDoor;
    public SpriteRenderer _sprite, torchOne, torchTwo, torchThree;
    public TextMesh levelTitle;

    bool isLocked = true, isPassed = false;
    int torchNumbers;

    LevelPoint prevousLevel, nextLevel;
    private void Awake()
    {

    }
    
    private void Start()
    {
        LevelManager.singleton.registerLevel(this);

        test();
        initAcceablity();
        levelTitle.text = "Level " + (levelIndex + 1);

        /*
        if (levelIndex != 0)
        {
            prevousLevel = LevelManager.singleton.getLevel(levelIndex - 1);
            nextLevel = LevelManager.singleton.getLevel(levelIndex + 1);


            closeNextCollider;
            for (int x = 0; x < Colliders.Length; x++)
            {

            }
        }
        */
    }

   
    void test()
    {
        if (levelIndex == 0)
        {
            PlayerPrefs.SetInt("level_" + levelIndex + "_torches", 2);

        }


    }
    private void initAcceablity()
    {
        torchNumbers = PlayerPrefs.GetInt("level_" + levelIndex + "_torches", 0);
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
        }
        if (torchNumbers >= 2)
        {
            torchTwo.color = Color.white;
        }
        if (torchNumbers >= 3)
        {
            torchThree.color = Color.white;
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
        Transform playerTransform = PlayerController.singleton.transform;

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
