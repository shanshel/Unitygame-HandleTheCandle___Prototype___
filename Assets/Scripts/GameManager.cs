using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singelton;
    public LevelInfo currentLevel;
    int currentPlayerCount;


    
    private void Awake()
    {
        if (singelton == null)
        {
            singelton = this;
        } else
        {
            Destroy(gameObject);
        }
            
    }

    private void Start()
    {
        
    }

 
    private void Update()
    {


    }

    public void playerReachGoalZone(PlayerController player)
    {
        currentPlayerCount++;
        if (currentPlayerCount == currentLevel.maxPlayerCount)
        {
            loadNextScene();
        }
    }

    void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


 


}
