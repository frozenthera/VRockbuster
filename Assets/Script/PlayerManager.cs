using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int player_HP;
    [SerializeField] private int curPuckCnt;
    public static PlayerManager Instance;
    [SerializeField] private GameObject PuckPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() 
    {
        player_HP = 100;
        UIManager.Instance.SetLifePoint(player_HP);    
        curPuckCnt = 1;
        UIManager.Instance.SetPuckCnt(curPuckCnt);
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(1))
        {
            SpawnPuck(new Vector3(-0.064f, 0.05f, -0.788f));
        }
    }

    public void GetDamage(int damage)
    {
        player_HP -= damage;
        UIManager.Instance.SetLifePoint(player_HP);
        if(player_HP <= 0) GameManager.Instance.ResetGame();
    }

    public void GetPuck(int num)
    {
        curPuckCnt += 1;
        UIManager.Instance.SetPuckCnt(curPuckCnt);
    }

    public void SpawnPuck(Vector3 pos)
    {
        if(curPuckCnt < 1)
        {
            Debug.Log("You don't have puck!");
        }
        else
        {
            Instantiate(PuckPrefab, Vector3.zero, Quaternion.identity);
            curPuckCnt--;  
        }
    }
}
