using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelInfo : MonoBehaviour
{
    public int maxPlayerCount;
    public static LevelInfo instance;

    public float levelTime;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        levelTime += Time.deltaTime;
    }
}
