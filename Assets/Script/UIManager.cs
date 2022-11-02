using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI curPuckCnt;
    [SerializeField] private TextMeshProUGUI curScore;
    [SerializeField] private TextMeshProUGUI curLifePoint;
    [SerializeField] private List<GameObject> bulletTimeImages = new();
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetUI();
    }

    public void ResetUI()
    {
        curPuckCnt.text = "1";
        curScore.text = "Score : 0";
        curLifePoint.text = "LIFE : 0";
    }

    public void SetPuckCnt(int cnt)
    {
        curPuckCnt.text = $"{cnt}";
    }

    public void SetScore(int cnt)
    {
        curScore.text = $"SCORE : {cnt}";
    }

    public void SetLifePoint(int cnt)
    {
        curLifePoint.text = $"LIFE : {cnt}";
    }

    public void SetBulletTimeGauge(int n)
    {
        for(int i=0; i< n; i++)
        {
            bulletTimeImages[i].SetActive(true);
        }
        for(int i=n; i< bulletTimeImages.Count; i++)
        {
            bulletTimeImages[i].SetActive(false);
        }
    }
}
