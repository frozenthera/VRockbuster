using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public int Puck_damage => puck_Damage;
    [SerializeField] private int puck_Damage = 5;
    private Rigidbody rigid;
    private Collider coll;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        StartCoroutine(InitialRetreive());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(new Vector3(1f, 0, 1f), ForceMode.Impulse);
        }

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
        if(rigid.velocity.sqrMagnitude == 0) Retrieve();
    }

    public void Hit_By_Player(Vector3 dir)
    {
        rigid.velocity = dir;
        //rigid.AddForce(new Vector3(dir.x, 0, dir.z), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.CompareTag("PuckBottom"))
        {
            Retrieve();
        }
    }

    public void Retrieve()
    {
        Debug.Log("retrieved");
        PlayerManager.Instance.GetPuck(1);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //Debug.Log(rigid.velocity.magnitude);
    }
}
