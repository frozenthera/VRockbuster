using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int player_HP;
    private int curPuckCnt;
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
        Debug.Log(pos);
        Instantiate(PuckPrefab, Vector3.zero, Quaternion.identity);
    }
}
