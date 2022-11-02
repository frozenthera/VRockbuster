using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerManager : MonoBehaviour
{
    private int player_HP;
    [SerializeField] private int curPuckCnt;
    [SerializeField] private int maxPuckCnt;
    public static PlayerManager Instance;
    [SerializeField] private GameObject PuckPrefab;
    [SerializeField] private Handle handle;

    [SerializeField] private SteamVR_Action_Boolean Trigger;     // VR input
    [SerializeField] private Transform puckSpawnPoint;           // spqwn point + check empty
    [SerializeField] private SteamVR_Action_Boolean bulletTime_action;     // VR input for bulletTime
    
    private float curTimeScale = 1f;
    public bool IsBulletTime => isBulletTime;
    private bool isBulletTime = false;
    private float GetPuckThreshold = 50f;
    private float CurGetPuckThreshold;

    private int BTtoken_max = 5;
    private int BTtoken_cur = 5;
    private float BTtime_for_token = 2.0f;
    private bool isTokenUsing = false;
    private float BTregen_interval = 1.0f;
    private bool isRegen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() 
    {
        player_HP = 30;
        UIManager.Instance.SetLifePoint(player_HP);    
        curPuckCnt = 1;
        maxPuckCnt = 1;
        UIManager.Instance.SetPuckCnt(curPuckCnt);
        BTtoken_cur = BTtoken_max;
    }

    private void Update()
    {
        BulletTimeUpdate();
        CurGetPuckThreshold = GetPuckThreshold * GameManager.Instance.spawnRate;
    }

    public void BulletTimeUpdate()
    {
        bool b_LTState = bulletTime_action.GetState(SteamVR_Input_Sources.LeftHand);
        bool b_LTDown = bulletTime_action.GetStateDown(SteamVR_Input_Sources.LeftHand);
        bool b_LTUp = bulletTime_action.GetStateUp(SteamVR_Input_Sources.LeftHand);

        bool b_RTState = bulletTime_action.GetState(SteamVR_Input_Sources.RightHand);
        bool b_RTDown = bulletTime_action.GetStateDown(SteamVR_Input_Sources.RightHand);
        bool b_RTUp = bulletTime_action.GetStateUp(SteamVR_Input_Sources.RightHand);


        if ((b_LTDown && b_RTState) || (b_LTState && b_RTDown))
        {
            ActivateBulletTime();
        }

        if (b_LTUp || b_RTUp){
            isTokenUsing = false;
            DeactivateBulletTime();
        }

        if (isBulletTime)
        {
            if (isTokenUsing == false)
            {
                if (BTtoken_cur > 0)
                {
                    BTtoken_cur--;
                    UIManager.Instance.SetBulletTimeGauge(BTtoken_cur);
                    print("token : " + BTtoken_cur + "/" + BTtoken_max);
                    isTokenUsing = true;
                    StartCoroutine(tokenTimer());
                }
                else
                {
                    DeactivateBulletTime();
                }
            }
        }
        else
        {
            if (isRegen == false)
            {
                if (BTtoken_cur < BTtoken_max)
                {
                    isRegen = true;
                
                    UIManager.Instance.SetBulletTimeGauge(BTtoken_cur);
                    print("token : " + BTtoken_cur + "/" + BTtoken_max);
                    StartCoroutine(regenTimer());
                }
            }
        }
    }
    private IEnumerator tokenTimer()
    {
        StopCoroutine(regenTimer());
        yield return new WaitForSecondsRealtime(BTtime_for_token);
        isTokenUsing = false;
    }
    private IEnumerator regenTimer()
    {
        StopCoroutine(tokenTimer());
        yield return new WaitForSecondsRealtime(BTregen_interval);
        isRegen = false;
        BTtoken_cur++;
        UIManager.Instance.SetBulletTimeGauge(BTtoken_cur);
    }

    private IEnumerator BTActiveTimer()
    {
        BTtoken_cur--;
        UIManager.Instance.SetBulletTimeGauge(BTtoken_cur);
        float time = BTtime_for_token;
        while(isBulletTime && time > 0f)
        {
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            time -= Time.unscaledDeltaTime;
        }
        DeactivateBulletTime();
    }

    private IEnumerator BTRegenTimer()
    {
        float time = BTregen_interval;
        while (!isBulletTime && time > 0f)
        {
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            time -= Time.unscaledDeltaTime;
        }
        BTtoken_cur++;
        UIManager.Instance.SetBulletTimeGauge(BTtoken_cur);
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
        curPuckCnt--;
        UIManager.Instance.SetPuckCnt(curPuckCnt);
    }
    
    // 업데이트 주기마다 체크해서 퍽 스폰하는 함수
    public void puckSpawn_Update(){
        bool b_LHTrigger      = Trigger.GetStateDown(SteamVR_Input_Sources.LeftHand);
        bool b_testTrigger    = Input.GetKeyDown(KeyCode.T);
        bool b_Trigger        = b_LHTrigger || b_testTrigger;
        bool b_puckCntAv      = (curPuckCnt <= maxPuckCnt) && (curPuckCnt > 0);
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

    public void UpdatedScore(int score)
    {
        if((score / CurGetPuckThreshold) >= maxPuckCnt)
        {
            maxPuckCnt++;
            curPuckCnt++;
            Debug.Log($"{curPuckCnt}, {maxPuckCnt}, {score}");
        }
    }

    private void ActivateBulletTime()
    {
        isBulletTime = true;
        StartCoroutine(handle.StartEcho());
        StartCoroutine(BulletTimeActivate());
    }

    private void DeactivateBulletTime()
    {
        isBulletTime = false;
        StartCoroutine(BulletTimeDeactivate());
    }

    private IEnumerator BulletTimeActivate()
    {
        curTimeScale = .8f;
        while(curTimeScale > .1f)
        {
            if (!isBulletTime)
            {
                StartCoroutine(BulletTimeDeactivate());
                yield break;
            }
            curTimeScale -= Time.unscaledDeltaTime;
            Time.timeScale = curTimeScale;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        curTimeScale = .1f;
        Time.timeScale = curTimeScale;
    }

    private IEnumerator BulletTimeDeactivate()
    {
        while (curTimeScale < 1f)
        {
            if (isBulletTime)
            {
                StartCoroutine(BulletTimeActivate());
                yield break;
            }
            curTimeScale += Time.unscaledDeltaTime;
            Time.timeScale = curTimeScale;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        curTimeScale = 1f;
        Time.timeScale = curTimeScale;
    }
}
