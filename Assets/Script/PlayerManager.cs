using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerManager : MonoBehaviour
{
    private int player_HP;
    [SerializeField] private int curPuckCnt;
    private int maxPuckCnt;
    public static PlayerManager Instance;
    [SerializeField] private GameObject PuckPrefab;

    [SerializeField] private SteamVR_Action_Boolean Trigger;     // VR input
    [SerializeField] private Transform puckSpawnPoint;           // spqwn point + check empty

    private void Awake()
    {
        Instance = this;
    }

    private void Start() 
    {
        player_HP = 100;
        UIManager.Instance.SetLifePoint(player_HP);    
        curPuckCnt = 0;
        maxPuckCnt = 1;
        UIManager.Instance.SetPuckCnt(curPuckCnt);
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(1))
        {
            SpawnPuck(new Vector3(-0.064f, 0.05f, -0.788f));
        }
    }

    private void FixedUpdate() 
    {
        puckSpawn_Update();
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
        Debug.Log("puck generated on : " + pos);
        Instantiate(PuckPrefab, pos, Quaternion.identity);
        curPuckCnt ++;
    }
    
    // 업데이트 주기마다 체크해서 퍽 스폰하는 함수
    public void puckSpawn_Update(){
        bool b_LHTrigger      = Trigger.GetStateDown(SteamVR_Input_Sources.LeftHand);
        bool b_testTrigger    = Input.GetKeyDown(KeyCode.T);
        bool b_Trigger        = b_LHTrigger || b_testTrigger;
        bool b_puckCntAv      = (curPuckCnt < maxPuckCnt);
        bool b_isEmpty        = true;

        // check spawn point is available
        float collradius = 0.1f;
        int collLayer = 5 << 8;     // puck : 8, block : 10 layer ==> 10100_0000_00
        Collider[] coll = Physics.OverlapSphere(puckSpawnPoint.position, collradius, collLayer);
        if(coll.Length == 0){
            b_isEmpty = true;
        }else{
            b_isEmpty = false;
        }

        if(b_Trigger){
            if(b_puckCntAv && b_isEmpty){
                SpawnPuck(puckSpawnPoint.position);
            }

            if(!b_puckCntAv){
                Debug.Log("no available puck");
            }
            if(!b_isEmpty){
                Debug.Log("spawnpoint already occupied");
            }
        }
    }

}
