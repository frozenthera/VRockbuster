using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public int Puck_damage => puck_Damage;
    [SerializeField] private int puck_Damage = 5;
    private Rigidbody rigid;
    private Collider coll;
    private Material mat;
    public bool isFirstSpawned = true;

    public float powerMultiplier = 1;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        StartCoroutine(InitialRetreive());
        mat = GetComponent<Renderer>().material;
        UpdateColor();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
    }

    private IEnumerator InitialRetreive()
    {
        float time = 0f;
        while(time < 5f)
        {
            yield return null;
            time += Time.deltaTime;
        }
        isFirstSpawned = false;
        if(rigid.velocity.sqrMagnitude == 0) Retrieve();
    }

    public void Hit_By_Player(Vector3 dir)
    {
        isFirstSpawned = false;
        rigid.velocity = dir;
        //rigid.AddForce(new Vector3(dir.x, 0, dir.z), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.CompareTag("PuckBottom"))
        {
            isFirstSpawned = false;
            Retrieve();
        }
    }

    public void Retrieve()
    {
        if (isFirstSpawned) return;
        Debug.Log("retrieved");
        PlayerManager.Instance.GetPuck(1);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log(rigid.velocity.magnitude);
    }

    public void AddBounce()
    {
        powerMultiplier += .2f;
        UpdateColor();
    }

    public void UpdateColor()
    {
        float refNum = (int)powerMultiplier * 50;
        mat.color = new Color(refNum/255, 0, 0, 1);
    }
}
