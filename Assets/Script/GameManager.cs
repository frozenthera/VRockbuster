using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject BlockPrefab;
    public int score = 0;

    private void Awake()
    {
        Instance = this;    
    }

    private void Start() 
    {
        Initialize();    
    }

    private void Initialize()
    {
        score = 0;
    }

    public void GetScore(int achieved)
    {
        score += achieved;
    }

    public void ResetGame()
    {
        
    }
}
