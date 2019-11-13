using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevel { get; set; }
    public static GameManager singelton;
    int reachedPlayerCount;

    
    private void Awake()
    {
        
        if (singelton == null)
        {
            singelton = this;
            DontDestroyOnLoad(this.gameObject);
            PlayerPrefs.DeleteAll();

        } else
        {
            Destroy(gameObject);
        }
            
    }


    public void playerReachGoalZone(PlayerController player)
    {
        reachedPlayerCount++;
        if (reachedPlayerCount == LevelInfo.instance.maxPlayerCount)
        {
            reachedPlayerCount = 0;
            playerFinishLevel();
            loadNextScene();
        }
    }

    void playerFinishLevel()
    {
        int playerLastReachedLevel = PlayerPrefs.GetInt("playerReachedLevel", 0);
        Debug.Log("LastReached: " + playerLastReachedLevel + " ThisLevel:" + currentLevel);

        //update reached level if reached level is better
        if (currentLevel == playerLastReachedLevel)
            PlayerPrefs.SetInt("playerReachedLevel", currentLevel + 1);

        //update Level time if reached time is better
        float lastLevelTime = PlayerPrefs.GetFloat("LevelTime_" + currentLevel, 0f);
        if (LevelInfo.instance.levelTime > lastLevelTime)
            PlayerPrefs.SetFloat("LevelTime_" + currentLevel, LevelInfo.instance.levelTime);




    }
    void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


 


}
