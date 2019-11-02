using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager singleton;
    private int playerReachedLevel;
    public int currentlyPlayerLevelRoom = -1;
   
    Dictionary<int, LevelPoint> levels = new Dictionary<int, LevelPoint>();

    private void Awake()
    {
        singleton = this;
        playerReachedLevel = PlayerPrefs.GetInt("playerReachedLevel", 2);
       
    }

    public int getPlayerReachedLevel()
    {
       
        return playerReachedLevel;
    }


    public void registerLevel(LevelPoint level)
    {
        if (!levels.ContainsKey(level.levelIndex))
        {
            levels.Add(level.levelIndex, level);
        }
    }

    public LevelPoint getLevel(int index)
    {
        if (levels.ContainsKey(index))
        {
            return levels[index];
        }
        return null;
    }

    public void getClosesetLevel(int currentLevelIndex, Transform collisionTransform)
    {
        float lowestDistance = 10000f;
        LevelPoint closesetLevel = null;
        for (var x = 0; x < levels.Count; x++)
        {
            if (currentLevelIndex == levels[x].levelIndex) continue;
            float currDistance = Vector2.Distance(levels[x].transform.position, collisionTransform.position);
            if (currDistance < lowestDistance)
            {
                lowestDistance = currDistance;
                closesetLevel = levels[x];
            }
        }

        //Debug.Log(closesetLevel.levelIndex + " Distance: " + lowestDistance);

    }


    public void disableCollisionBetweenLevels(string colliderName, LevelPoint currentRoom, LevelPoint otherRoom)
    {
   
        BoxCollider2D thisRoomCollider = null, otherRoomCollider = null;
        if (colliderName == "Bottom")
        {
            otherRoomCollider = otherRoom.bottom;
            thisRoomCollider = currentRoom.top;
        }
        else if (colliderName == "Top")
        {
            otherRoomCollider = otherRoom.top;
            thisRoomCollider = currentRoom.bottom;
        }
        else if (colliderName == "BottomRight")
        {
            otherRoomCollider = otherRoom.bottomRight;
            thisRoomCollider = currentRoom.topLeft;
        }
        else if (colliderName == "BottomLeft")
        {
            otherRoomCollider = otherRoom.bottomLeft;
            thisRoomCollider = currentRoom.topRight;
        }
        else if (colliderName == "TopLeft")
        {
            otherRoomCollider = otherRoom.topLeft;
            thisRoomCollider = currentRoom.bottomRight;
        }
        else if (colliderName == "TopRight")
        {
            otherRoomCollider = otherRoom.topRight;
            thisRoomCollider = currentRoom.bottomLeft;
        }
        else{
            Debug.Log("error Happen");
        }
        thisRoomCollider.enabled = false;
        otherRoomCollider.enabled = false;
        Debug.Log("will disable " + otherRoomCollider.name + " and " + thisRoomCollider.name);
        return;
      
        
    }
}
