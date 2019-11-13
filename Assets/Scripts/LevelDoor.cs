using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelPoint level = gameObject.GetComponentInParent<LevelPoint>();
                if (level != null)
                {
                    GameManager.singelton.currentLevel = level.levelIndex;
                    LevelManager.singleton.loadLevelScene(level.levelIndex);
                }
            }
        }
    }

}
