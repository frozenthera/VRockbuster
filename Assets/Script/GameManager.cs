using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public float spawnRate = 1f;
    public float gameTimer = 0f;

    private void Awake()
    {
        Instance = this;    
    }

    private void Start() 
    {
        Initialize();    
    }

    private void Update()
    {
        spawnRate = .16f + (score / 1000) + (gameTimer / 200f);
    }

    private void Initialize()
    {
        score = 0;
        StartCoroutine(Timer());
    }

    public void GetScore(int achieved)
    {
        score += achieved;
        PlayerManager.Instance.UpdatedScore(score);
        UIManager.Instance.SetScore(score);
    }

    public void ResetGame()
    {
        
    }

    private IEnumerator Timer()
    {
        while(true)
        {
            yield return null;
            gameTimer += Time.deltaTime;
        }
    }
}
